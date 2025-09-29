using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace M3uEditorWinForms
{
    public partial class MainForm : Form
    {
        private readonly string _jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "textboxdata.json");
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            WriteIndented = true
        };

        private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
        private static readonly Encoding Sjis = Encoding.GetEncoding(932);

        private readonly HashSet<string> _excludeNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly BindingList<FileRow> _fileRows = new();

        public sealed class FileRow : INotifyPropertyChanged
        {
            private bool _selected = true;
            private string _fileName = string.Empty;
            private string _fullPath = string.Empty;
            private string _status = string.Empty;

            public bool Selected
            {
                get => _selected;
                set { if (_selected == value) return; _selected = value; OnPropertyChanged(nameof(Selected)); }
            }
            public string FileName
            {
                get => _fileName;
                set { if (_fileName == value) return; _fileName = value; OnPropertyChanged(nameof(FileName)); }
            }
            public string FullPath
            {
                get => _fullPath;
                set { if (_fullPath == value) return; _fullPath = value; OnPropertyChanged(nameof(FullPath)); }
            }
            public string Status
            {
                get => _status;
                set { if (_status == value) return; _status = value; OnPropertyChanged(nameof(Status)); }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            private void OnPropertyChanged(string propertyName)
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainForm()
        {
            InitializeComponent();
            var baseDir = AppContext.BaseDirectory;
            _jsonPath = Path.Combine(baseDir, AppConstants.JsonFileName);

            ucFoobar2000.TitleText = AppConstants.Titlefoobar2000;
            ucMedia.TitleText = AppConstants.TitleMedia;
            ucPhone.TitleText = AppConstants.TitlePhone;

            InitFileGrid();
            btnMakeMedia.Click += async (_, __) => await BtnMakeMedia_ClickAsync();
            ucFoobar2000.ValidFolderConfirmed += (_, normalizedPath) => RefreshM3UListFromFoobarFolder(normalizedPath);
            tsbAddExclude.Click += (_, __) => AddExcludeFromCombo();
            tsbRemoveExclude.Click += (_, __) => RemoveSelectedExclude();
            tscbExclude.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) { AddExcludeFromCombo(); e.Handled = true; e.SuppressKeyPress = true; }
                else if (e.KeyCode == Keys.Escape) { tscbExclude.Text = string.Empty; e.Handled = true; }
                else if (e.KeyCode == Keys.Delete && tscbExclude.DroppedDown == true)
                {
                    RemoveSelectedExclude();
                    e.Handled = true;
                }
            };
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            LoadTextBoxData();
            if (Directory.Exists(ucFoobar2000.FolderPath))
                RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            SaveTextBoxData();
        }

        private void LoadTextBoxData()
        {
            if (File.Exists(_jsonPath))
            {
                try
                {
                    var json = File.ReadAllText(_jsonPath, Encoding.UTF8);
                    var state = JsonSerializer.Deserialize<AppState>(json, _jsonOptions);
                    if (state is not null)
                    {
                        ucFoobar2000.FolderPath = state.Foobar2000 ?? string.Empty;
                        ucMedia.FolderPath = state.MediaServer ?? string.Empty;
                        ucPhone.FolderPath = state.Smartphone ?? string.Empty;
                        tscbExclude.Items.Clear();
                        foreach (var name in state.ExcludeFileNames ?? Enumerable.Empty<string>())
                        {
                            if (!string.IsNullOrWhiteSpace(name))
                                tscbExclude.Items.Add(name.Trim());
                        }
                        RebuildExcludeSetFromUI();
                        if (Directory.Exists(ucFoobar2000.FolderPath))
                            RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
                    }
                }
                catch
                {
                    // 壊れた場合は無視
                }
            }
        }

        private void SaveTextBoxData()
        {
            var state = new AppState
            {
                Foobar2000 = ucFoobar2000.FolderPath,
                MediaServer = ucMedia.FolderPath,
                Smartphone = ucPhone.FolderPath,
                ExcludeFileNames = tscbExclude.Items
                    .Cast<object>()
                    .Select(x => (x?.ToString() ?? string.Empty).Trim())
                    .Where(s => s.Length > 0)
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .OrderBy(s => s, StringComparer.OrdinalIgnoreCase)
                    .ToList()
            };

            var json = JsonSerializer.Serialize(state, _jsonOptions);
            Directory.CreateDirectory(Path.GetDirectoryName(_jsonPath)!);
            File.WriteAllText(_jsonPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
        }

        private void btnQuit_Click(object sender, EventArgs e) => Close();

        private void InitFileGrid()
        {
            dgvFiles.AutoGenerateColumns = false;
            dgvFiles.ReadOnly = false;
            dgvFiles.AllowUserToAddRows = false;
            dgvFiles.AllowUserToDeleteRows = false;
            dgvFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvFiles.MultiSelect = false;

            var colSel = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(FileRow.Selected),
                HeaderText = "選択",
                Frozen = true,
                ReadOnly = false,
                Width = 38
            };
            var colName = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(FileRow.FileName),
                HeaderText = "ファイル名",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 70,
                ReadOnly = true
            };
            var colStatus = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(FileRow.Status),
                HeaderText = "ステータス",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 40,
                ReadOnly = true
            };

            dgvFiles.Columns.Clear();
            dgvFiles.Columns.Add(colSel);
            dgvFiles.Columns.Add(colName);
            dgvFiles.Columns.Add(colStatus);

            dgvFiles.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvFiles.IsCurrentCellDirty)
                {
                    dgvFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            };

            dgvFiles.DataSource = _fileRows;
        }

        private async Task BtnMakeMedia_ClickAsync()
        {
            dgvFiles.EndEdit();
            var cm = (CurrencyManager?)BindingContext[_fileRows];
            cm?.EndCurrentEdit();

            var srcDir = ucFoobar2000.FolderPath;
            var dstDir = ucMedia.FolderPath;

            if (string.IsNullOrWhiteSpace(srcDir) || !Directory.Exists(srcDir))
            {
                MessageBox.Show("foobar2000 フォルダが正しく設定されていません。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(dstDir) || !Directory.Exists(dstDir))
            {
                MessageBox.Show("メディアサーバー フォルダが正しく設定されていません。", "エラー",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var existingSelection = _fileRows.ToDictionary(r => r.FullPath, r => r.Selected, StringComparer.OrdinalIgnoreCase);
            _fileRows.Clear();
            foreach (var path in Directory.EnumerateFiles(srcDir, "*.m3u", SearchOption.TopDirectoryOnly))
            {
                _fileRows.Add(new FileRow
                {
                    Selected = existingSelection.TryGetValue(path, out var sel) ? sel : true,
                    FileName = Path.GetFileName(path),
                    FullPath = path,
                    Status = "待機中"
                });
            }

            var targets = _fileRows
                .Select((r, idx) => (Row: r, Index: idx))
                .Where(x => x.Row.Selected)
                .ToList();

            for (int i = 0; i < _fileRows.Count; i++)
            {
                _fileRows[i].Status = _fileRows[i].Selected ? "待機中" : "除外";
            }

            if (targets.Count == 0)
            {
                MessageBox.Show("選択された m3u ファイルがありません。", "情報",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnMakeMedia.Enabled = false;
            try
            {
                foreach (var x in targets)
                {
                    UpdateStatus(x.Index, "処理中…");
                    var row = x.Row;
                    var mediaRootForReplace = ucMedia.FolderPath.TrimEnd('\\', '/');
                    var ok = await M3uFileHelper.ConvertM3UToUtf8Async(
                        srcPath: row.FullPath,
                        dstDir: dstDir,
                        mediaRootForReplace: mediaRootForReplace
                    );
                    UpdateStatus(x.Index, ok ? "成功" : "失敗");
                }

                MessageBox.Show("終了しました。", "完了",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                btnMakeMedia.Enabled = true;
            }
        }

        private void UpdateStatus(int rowIndex, string status)
        {
            if (rowIndex < 0 || rowIndex >= _fileRows.Count) return;
            _fileRows[rowIndex].Status = status;
            var currencyManager = (CurrencyManager?)BindingContext[_fileRows];
        }

        private void RebuildExcludeSetFromUI()
        {
            _excludeNames.Clear();
            foreach (var item in tscbExclude.Items)
            {
                var s = (item?.ToString() ?? string.Empty).Trim();
                if (s.Length > 0) _excludeNames.Add(s);
            }
        }

        private void AddExcludeFromCombo()
        {
            var raw = (tscbExclude.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(raw)) return;
            foreach (var item in tscbExclude.Items)
            {
                if (string.Equals(item?.ToString(), raw, StringComparison.OrdinalIgnoreCase))
                {
                    tscbExclude.Text = string.Empty;
                    return;
                }
            }
            tscbExclude.Items.Add(raw);
            tscbExclude.Text = string.Empty;
            RebuildExcludeSetFromUI();
            RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
        }

        private void RemoveSelectedExclude()
        {
            if (tscbExclude.SelectedIndex >= 0)
            {
                tscbExclude.Items.RemoveAt(tscbExclude.SelectedIndex);
                RebuildExcludeSetFromUI();
                RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
            }
        }

        private void RefreshM3UListFromFoobarFolder(string foobarDir)
        {
            if (!Directory.Exists(foobarDir)) return;
            RebuildExcludeSetFromUI();
            _fileRows.Clear();
            foreach (var path in Directory.EnumerateFiles(foobarDir, "*.m3u", SearchOption.TopDirectoryOnly))
            {
                var name = Path.GetFileName(path);
                bool isExcludedByName = _excludeNames.Contains(name);
                _fileRows.Add(new FileRow
                {
                    Selected = !isExcludedByName,
                    FileName = name,
                    FullPath = path,
                    Status = ""
                });
            }
        }
    }
}

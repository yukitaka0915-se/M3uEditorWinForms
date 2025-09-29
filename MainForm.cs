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

      tspbOverall.Visible = false;                     // 初期は隠す（開始時に見せる）
      tspbOverall.AutoSize = false;                     // 幅が潰れないよう固定に
      tspbOverall.Width = 220;                       // 好きな幅に（150〜300）
      tspbOverall.Style = ProgressBarStyle.Blocks;   // 不確定なら Marquee に切替可
      tspbOverall.Overflow = ToolStripItemOverflow.Never; // …に折り畳まれないよう固定
      tspbOverall.Alignment = ToolStripItemAlignment.Right; // 右寄せにしたい場合（任意）


      var baseDir = AppContext.BaseDirectory;
        _jsonPath = Path.Combine(baseDir, AppConstants.JsonFileName);

        ucFoobar2000.TitleText = AppConstants.Titlefoobar2000;
        ucMedia.TitleText = AppConstants.TitleMedia;
        ucParent.TitleText = AppConstants.TitleParent;
        ucPhone.TitleText = AppConstants.TitlePhone;

        InitFileGrid();
        btnMakeMedia.Click += async (_, __) => await BtnMakeMedia_ClickAsync();
        btnMakePhoneCopy.Click += async (_, __) => await BtnMakePhoneCopy_ClickAsync();
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
                    ucParent.FolderPath = state.Parent ?? string.Empty;
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
            Parent = ucParent.FolderPath,
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

    private async Task BtnMakePhoneCopy_ClickAsync()
    {
      // DGVのチェックを確定
      dgvFiles.EndEdit();
      (BindingContext[_fileRows] as CurrencyManager)?.EndCurrentEdit();

      var foobarDir = ucFoobar2000.FolderPath;
      var parentDir = ucParent.FolderPath;
      var phoneDir = ucPhone.FolderPath;

      if (!Directory.Exists(foobarDir))
      {
        MessageBox.Show("foobar2000 フォルダが無効です。", "エラー",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      if (!Directory.Exists(parentDir))
      {
        MessageBox.Show("親フォルダが無効です。", "エラー",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      if (!Directory.Exists(phoneDir))
      {
        MessageBox.Show("スマートフォン フォルダが無効です。", "エラー",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      // 選択列が true の m3u のみ対象
      var targets = _fileRows
          .Select((r, idx) => (Row: r, Index: idx))
          .Where(x => x.Row.Selected)
          .ToList();

      if (targets.Count == 0)
      {
        MessageBox.Show("選択された m3u ファイルがありません。", "情報",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      // 危険パス防止
      if (M3uFileHelper.IsDangerousPath(phoneDir) ||
          M3uFileHelper.PathEquals(phoneDir, foobarDir) ||
          M3uFileHelper.PathEquals(phoneDir, parentDir) ||
          M3uFileHelper.PathEquals(phoneDir, ucMedia.FolderPath))
      {
        MessageBox.Show("削除対象のパスが不正です。処理を中止しました。", "エラー",
            MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      if (MessageBox.Show(
          $"スマートフォンフォルダの直下をすべて削除してからコピーします。\r\nよろしいですか？\r\n\r\n{phoneDir}",
          "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
          != DialogResult.Yes)
      {
        return;
      }

      btnMakePhoneCopy.Enabled = false;
      try
      {
        // 直下削除
        await ClearTopLevelAsync(phoneDir);

        int totalCopied = 0, totalFailed = 0, totalSkipped = 0;

        var parentBS = parentDir.TrimEnd('\\', '/');
        var parentSL = parentBS.Replace('\\', '/');
        var phoneBS = phoneDir.TrimEnd('\\', '/');

        // ★ 全体トラック件数をプリスキャン
        int totalTracks = await CountTotalTracksAsync(targets);
        int processedTracks = 0;

        // 0 件なら Marquee で「動いてる感」だけ出す
        if (totalTracks <= 0)
        {
          if (InvokeRequired)
          {
            BeginInvoke(new Action(() =>
            {
              tspbOverall.Style = ProgressBarStyle.Marquee;
              tspbOverall.Visible = true;
              toolStrip1.PerformLayout();
              toolStrip1.Refresh();
            }));
          }
          else
          {
            tspbOverall.Style = ProgressBarStyle.Marquee;
            tspbOverall.Visible = true;
            toolStrip1.PerformLayout();
            toolStrip1.Refresh();
          }
        }
        else
        {
          ShowOverallProgress(totalTracks);
        }

        foreach (var t in targets)
        {
          int rowIndex = t.Index;
          string m3u = t.Row.FullPath;

          if (!File.Exists(m3u))
          {
            UpdateStatus(rowIndex, "ファイルなし");
            totalFailed++;
            continue;
          }

          // m3u を SJIS で読み取り
          string text;
          try
          {
            using var reader = new StreamReader(m3u, Sjis, detectEncodingFromByteOrderMarks: false);
            text = await reader.ReadToEndAsync();
          }
          catch
          {
            UpdateStatus(rowIndex, "読込失敗");
            totalFailed++;
            continue;
          }

          var allLines = text.Replace("\r\n", "\n").Replace('\r', '\n').Split('\n');
          var trackLines = allLines.Where(l => l.Length > 0 && l[0] != '#').ToList();
          int total = trackLines.Count, ok = 0, ng = 0, sk = 0;

          UpdateStatus(rowIndex, total > 0 ? $"コピー中… 0/{total}" : "コピー対象なし");

          for (int i = 0; i < trackLines.Count; i++)
          {
            var srcPath = ResolveTrackPath(trackLines[i], parentBS, parentSL);
            if (srcPath is null) { ng++; goto NEXT_ITEM; }

            var dstPath = MapParentToPhone(srcPath, parentBS, parentSL, phoneBS);
            if (dstPath is null) { ng++; goto NEXT_ITEM; }

            try
            {
              if (File.Exists(dstPath))
              {
                sk++; // 既存 → スキップ
              }
              else
              {
                Directory.CreateDirectory(Path.GetDirectoryName(dstPath)!);
                File.Copy(srcPath, dstPath, overwrite: false);
                ok++;
              }
            }
            catch
            {
              ng++;
            }

            NEXT_ITEM:
            int processedLocal = ok + ng + sk;
            UpdateStatus(rowIndex, $"コピー中… {processedLocal}/{total} (成功:{ok} 失敗:{ng} スキップ:{sk})");

            // ★ 全体進捗を 1 曲ごとに前進
            processedTracks++;
            StepOverallProgress(processedTracks, totalTracks);
          }

          UpdateStatus(rowIndex, $"完了 成功:{ok} 失敗:{ng} スキップ:{sk}");

          totalCopied += ok;
          totalFailed += ng;
          totalSkipped += sk;
        }

        MessageBox.Show(
            $"終了しました。\r\nコピー成功: {totalCopied} 件\r\n失敗: {totalFailed} 件\r\nスキップ: {totalSkipped} 件",
            "完了", MessageBoxButtons.OK, MessageBoxIcon.Information);
      }
      finally
      {
        // ★ プログレスバーを隠す
        HideOverallProgress();
        btnMakePhoneCopy.Enabled = true;
      }
    }

    /// <summary>
    /// m3uの行を絶対パスに解決する。'.' 始まりは parent を基準に解決。
    /// それ以外は、そのまま（\ と / を許容）。
    /// </summary>
    private static string? ResolveTrackPath(string line, string parentBS, string parentSL)
    {
      var s = line.Trim();

      // URL系は対象外
      if (s.StartsWith("http:", StringComparison.OrdinalIgnoreCase) ||
          s.StartsWith("https:", StringComparison.OrdinalIgnoreCase) ||
          s.StartsWith("file:", StringComparison.OrdinalIgnoreCase))
        return null;

      // 相対指定 "./" or ".\"
      if (s.StartsWith("./") || s.StartsWith(@".\"))
      {
        var rel = s.StartsWith("./") ? s.Substring(2) : s.Substring(2);
        var combined = Path.Combine(parentBS, rel.Replace('/', '\\'));
        return combined;
      }

      // それ以外は原文のセパレータを \ に寄せて返す（UNC含む）
      var norm = s.Replace('/', '\\');

      // 簡易に存在チェックはしない（存在しなくてもコピーで失敗カウント）
      return norm;
    }

    /// <summary>
    /// src の先頭が parent なら、parent を phone に置換し、以降のサブパスを引き継ぐ。
    /// </summary>
    private static string? MapParentToPhone(string srcPath, string parentBS, string parentSL, string phoneBS)
    {
      // すべて \ に寄せて判定
      var srcBS = srcPath.Replace('/', '\\');

      if (srcBS.StartsWith(parentBS, StringComparison.OrdinalIgnoreCase))
      {
        var rest = srcBS.Substring(parentBS.Length);       // 例: \Artist\Album\track.flac
        // 先頭の区切りは残っている想定なので、そのまま結合
        return phoneBS + rest;
      }

      // スラッシュ表記での一致も一応サポート（保険）
      var srcSL = srcPath.Replace('\\', '/');
      if (srcSL.StartsWith(parentSL, StringComparison.OrdinalIgnoreCase))
      {
        var rest = srcSL.Substring(parentSL.Length);       // 例: /Artist/Album/track.flac
        var restBS = rest.Replace('/', '\\');
        return phoneBS + restBS;
      }

      // parent に一致しない（ポータブル外など）は対象外
      return null;
    }

    private static Task ClearTopLevelAsync(string root)
    {
      return Task.Run(() =>
      {
        // ファイル（直下のみ）
        foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.TopDirectoryOnly))
        {
          try { File.SetAttributes(file, FileAttributes.Normal); File.Delete(file); }
          catch { /* ログ等が必要ならここで */ }
        }

        // ディレクトリ（直下のみ・再帰削除）
        foreach (var dir in Directory.EnumerateDirectories(root, "*", SearchOption.TopDirectoryOnly))
        {
          try
          {
            // 読取専用等でも消せるよう属性クリア（中もまとめて）
            ClearAttributesRecursive(dir);
            Directory.Delete(dir, recursive: true);
          }
          catch { /* 必要ならログ */ }
        }
      });
    }

    private static void ClearAttributesRecursive(string path)
    {
      foreach (var f in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
      {
        try { File.SetAttributes(f, FileAttributes.Normal); } catch { }
      }
      foreach (var d in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
      {
        try { File.SetAttributes(d, FileAttributes.Directory); } catch { }
      }
    }

    // 進捗バーの表示/更新/終了ヘルパー
    private void ShowOverallProgress(int total)
    {
      if (InvokeRequired) { BeginInvoke(new Action<int>(ShowOverallProgress), total); return; }

      tspbOverall.Minimum = 0;
      tspbOverall.Maximum = Math.Max(1, total);
      tspbOverall.Value = 0;
      tspbOverall.Style = ProgressBarStyle.Blocks;
      tspbOverall.ToolTipText = $"0 / {total}";
      tspbOverall.Visible = true;

      // 描画を即反映（ToolStripのレイアウトが走っていない場合の保険）
      toolStrip1.PerformLayout();
      toolStrip1.Refresh();
    }

    private void StepOverallProgress(int current, int total)
    {
      if (InvokeRequired) { BeginInvoke(new Action<int, int>(StepOverallProgress), current, total); return; }

      // 範囲ガード
      var capped = Math.Min(Math.Max(current, tspbOverall.Minimum), tspbOverall.Maximum);
      tspbOverall.Value = capped;
      tspbOverall.ToolTipText = $"{current} / {total}";
    }

    private void HideOverallProgress()
    {
      if (InvokeRequired) { BeginInvoke(new Action(HideOverallProgress)); return; }

      tspbOverall.Visible = false;
      tspbOverall.Value = 0;
      tspbOverall.ToolTipText = string.Empty;

      toolStrip1.PerformLayout();
      toolStrip1.Refresh();
    }

    private async Task<int> CountTotalTracksAsync(IEnumerable<(FileRow Row, int Index)> targets)
    {
      int total = 0;

      foreach (var t in targets)
      {
        var m3u = t.Row.FullPath;
        if (!File.Exists(m3u)) continue;

        try
        {
          using var reader = new StreamReader(m3u, Sjis, detectEncodingFromByteOrderMarks: false);
          string? line;
          while ((line = await reader.ReadLineAsync().ConfigureAwait(false)) is not null)
          {
            if (line.Length == 0) continue;
            if (line[0] == '#') continue;
            total++;
          }
        }
        catch
        {
          // 読めないファイルは 0 件としてスキップ
        }
      }

      return total;
    }
  

  }
}

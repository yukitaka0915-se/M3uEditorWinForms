using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using System.Windows.Forms;
using static M3uEditorWinForms.MainForm;

namespace M3uEditorWinForms
{
  public partial class MainForm : Form
  {
    private readonly string _jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "textboxdata.json");
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
      WriteIndented = true // 可読性
                           // 特殊文字のエスケープは System.Text.Json のデフォルトで有効
    };

    private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    private static readonly Encoding Sjis = Encoding.GetEncoding(932); // CP932

    // ★追加：除外ファイル名（小文字・Trim済）を保持
    private readonly HashSet<string> _excludeNames = new(StringComparer.OrdinalIgnoreCase);

    // グリッドのデータソース
    private readonly BindingList<FileRow> _fileRows = new();

    // DataGridView用の1行モデル
    public sealed class FileRow : INotifyPropertyChanged
    {
      private bool _selected = true;          // ★：選択可否（既定 true）
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
        set
        {
          if (_fileName == value) return;
          _fileName = value;
          OnPropertyChanged(nameof(FileName));
        }
      }

      // 内部処理用
      public string FullPath
      {
        get => _fullPath;
        set
        {
          if (_fullPath == value) return;
          _fullPath = value;
          OnPropertyChanged(nameof(FullPath));
        }
      }

      public string Status
      {
        get => _status;
        set
        {
          if (_status == value) return;
          _status = value;
          OnPropertyChanged(nameof(Status));
        }
      }

      public event PropertyChangedEventHandler? PropertyChanged;
      private void OnPropertyChanged(string propertyName)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainForm()
    {

      InitializeComponent();

      // 実行ファイルのあるフォルダに保存
      var baseDir = AppContext.BaseDirectory;
      _jsonPath = Path.Combine(baseDir, AppConstants.JsonFileName);

      ucFoobar2000.TitleText = AppConstants.Titlefoobar2000; ;
      ucMedia.TitleText = AppConstants.TitleMedia;
      ucPhone.TitleText = AppConstants.TitlePhone;

      // DataGridView 初期化
      InitFileGrid();

      // ボタンクリック
      btnMakeMedia.Click += async (_, __) => await BtnMakeMedia_ClickAsync();

      // ucFoobar2000 の有効フォルダ通知を購読
      ucFoobar2000.ValidFolderConfirmed += (_, normalizedPath) => RefreshM3UListFromFoobarFolder(normalizedPath);

      // ★除外テキスト変更時（必要ならその場で反映）
      tsbAddExclude.Click += (_, __) => AddExcludeFromCombo();
      tsbRemoveExclude.Click += (_, __) => RemoveSelectedExclude();

      // ★Enter で追加、Esc でテキストクリア
      tscbExclude.KeyDown += (s, e) =>
      {
        if (e.KeyCode == Keys.Enter) { AddExcludeFromCombo(); e.Handled = true; e.SuppressKeyPress = true; }
        else if (e.KeyCode == Keys.Escape) { tscbExclude.Text = string.Empty; e.Handled = true; }
        else if (e.KeyCode == Keys.Delete && tscbExclude.DroppedDown == true)
        {
          // ドロップダウン表示中に Delete で選択項目削除（任意）
          RemoveSelectedExclude();
          e.Handled = true;
        }
      };

    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
      LoadTextBoxData();
      if (Directory.Exists(ucFoobar2000.FolderPath)) RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
      SaveTextBoxData();
    }

    private void LoadTextBoxData()
    {
      // JSON 読み込み（存在しなければ空）
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

            // ★：ComboBox Items を復元
            tscbExclude.Items.Clear();
            foreach (var name in state.ExcludeFileNames ?? Enumerable.Empty<string>())
            {
              if (!string.IsNullOrWhiteSpace(name))
                tscbExclude.Items.Add(name.Trim());
            }
            RebuildExcludeSetFromUI();

            // 起動時点で有効なら初回スキャン（任意）
            if (Directory.Exists(ucFoobar2000.FolderPath))
              RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
          }
        }
        catch
        {
          // 壊れた場合は無視して新規として扱う（必要ならメッセージ表示）
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

    private void btnQuit_Click(object sender, EventArgs e)
    {
      Close();
    }

    private void InitFileGrid()
    {
      dgvFiles.AutoGenerateColumns = false;
      dgvFiles.ReadOnly = false; // 全体は編集可能にする

      dgvFiles.AllowUserToAddRows = false;
      dgvFiles.AllowUserToDeleteRows = false;
      dgvFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dgvFiles.MultiSelect = false;

      // チェックボックス列（編集可）
      var colSel = new DataGridViewCheckBoxColumn
      {
        DataPropertyName = nameof(FileRow.Selected),
        HeaderText = "選択",
        Frozen = true,
        ReadOnly = false, // ← 編集可能
        Width = 38
      };

      // ファイル名列（編集不可）
      var colName = new DataGridViewTextBoxColumn
      {
        DataPropertyName = nameof(FileRow.FileName),
        HeaderText = "ファイル名",
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        FillWeight = 70,
        ReadOnly = true // ← 編集不可
      };

      // ステータス列（編集不可）
      var colStatus = new DataGridViewTextBoxColumn
      {
        DataPropertyName = nameof(FileRow.Status),
        HeaderText = "ステータス",
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        FillWeight = 40,
        ReadOnly = true // ← 編集不可
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
      // ★ 追加：UI の未確定編集を確定
      dgvFiles.EndEdit(); // 現在のセル編集を確定
      var cm = (CurrencyManager?)BindingContext[_fileRows];
      cm?.EndCurrentEdit();
      
      // ガード（存在チェック）
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

      // 既存の選択状態を保存しておく
      var existingSelection = _fileRows.ToDictionary(r => r.FullPath, r => r.Selected, StringComparer.OrdinalIgnoreCase);

      // グリッドに一覧を作る（トップディレクトリのみ）
      // 既存選択を保持して再構築
      _fileRows.Clear();
      foreach (var path in Directory.EnumerateFiles(srcDir, "*.m3u", SearchOption.TopDirectoryOnly))
      {
        _fileRows.Add(new FileRow
        {
          Selected = existingSelection.TryGetValue(path, out var sel) ? sel : true, // 既存の選択を復元、なければ true（既定）
          FileName = Path.GetFileName(path),
          FullPath = path,
          Status = "待機中"
        });
      }

      // ★ 選択列が true の行だけを対象に
      var targets = _fileRows
        .Select((r, idx) => (Row: r, Index: idx))
        .Where(x => x.Row.Selected)
        .ToList();

      // ★ Selected=false の行は Status="除外" にする
      for (int i = 0; i < _fileRows.Count; i++)
      {
        if (!_fileRows[i].Selected)
        {
          _fileRows[i].Status = "除外";
        }
        else
        {
          _fileRows[i].Status = "待機中"; // 必要なら初期化
        }
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

          var ok = await ConvertM3UToUtf8Async(
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

      // BindingList の変更通知で自動反映されるが、描画を即時反映させたい場合は以下
      var currencyManager = (CurrencyManager?)BindingContext[_fileRows];

    }

    private async Task<bool> ConvertM3UToUtf8Async(string srcPath, string dstDir, string mediaRootForReplace)
    {
      try
      {
        // 1) 読み込み（Shift-JIS 固定）
        string text;
        using (var reader = new StreamReader(srcPath, Sjis, detectEncodingFromByteOrderMarks: false))
        {
          text = await reader.ReadToEndAsync().ConfigureAwait(false);
        }

        var dstName = "UTF8_" + Path.GetFileName(srcPath);
        var dstPath = Path.Combine(dstDir, dstName);

        // ★ ここから：CRLF で書き出す（NewLine を明示）
        using (var writer = new StreamWriter(dstPath, append: false, Utf8NoBom))
        {
          writer.NewLine = "\r\n";

          var rootBS = mediaRootForReplace;                    // 例: D:\Media
          var rootSL = mediaRootForReplace.Replace('\\', '/'); // 例: D:/Media

          using var sr = new StringReader(text);
          string? line;

          while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) is not null)
          {
            string outLine =
                (line.Length > 0 && line[0] != '#')
                ? ReplaceRootPrefixWithDot(line, rootBS, rootSL)
                : line;

            // WriteLine を使うので、出力は常に CRLF
            // 逐次書き込みでメモリ使用も抑えます
            await writer.WriteLineAsync(outLine).ConfigureAwait(false);

          }

        }
        return true;
      }
      catch
      {
        return false;
      }

    }

    // 先頭が mediaRoot に一致していれば、その一致部分を '.' に置換（大文字小文字は無視）
    private static string ReplaceRootPrefixWithDot(string line, string rootBackslash, string rootSlash)
    {
      // 例: line = "D:\\Media\\Artist\\song.flac"
      //     rootBackslash = "D:\\Media"  → 出力: ".\\Artist\\song.flac"
      if (StartsWithOrdinalIgnoreCase(line, rootBackslash))
        return "." + line.Substring(rootBackslash.Length);

      // 例: line = "D:/Media/Artist/song.flac"
      //     rootSlash = "D:/Media"       → 出力: "./Artist/song.flac"
      if (StartsWithOrdinalIgnoreCase(line, rootSlash))
        return "." + line.Substring(rootSlash.Length);

      return line;
    }

    private static bool StartsWithOrdinalIgnoreCase(string s, string prefix)
    {
      return s.Length >= prefix.Length &&
             s.AsSpan(0, prefix.Length).Equals(prefix.AsSpan(), StringComparison.OrdinalIgnoreCase);
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

    //★ 追加
    private void AddExcludeFromCombo()
    {
      var raw = (tscbExclude.Text ?? string.Empty).Trim();
      if (string.IsNullOrEmpty(raw)) return;

      // 重複ガード（大文字小文字無視）
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

      // 内部セットを再構築
      RebuildExcludeSetFromUI();

      //既に一覧が表示済みなら、選択状態を更新したい場合は再スキャン
       RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
    }

    //★ 追加
    private void RemoveSelectedExclude()
    {
      if (tscbExclude.SelectedIndex >= 0)
      {
        tscbExclude.Items.RemoveAt(tscbExclude.SelectedIndex);
        RebuildExcludeSetFromUI();
        RefreshM3UListFromFoobarFolder(ucFoobar2000.FolderPath);
      }
    }

    //★ 追加
    private void RefreshM3UListFromFoobarFolder(string foobarDir)
    {
      if (!Directory.Exists(foobarDir)) return;

      // 除外セットを最新化（UIから）
      RebuildExcludeSetFromUI(); // _excludeNames を更新（大文字小文字無視）

      _fileRows.Clear();

      foreach (var path in Directory.EnumerateFiles(foobarDir, "*.m3u", SearchOption.TopDirectoryOnly))
      {
        var name = Path.GetFileName(path);

        // tscbExclude に載っている名前だけ未選択(false)
        bool isExcludedByName = _excludeNames.Contains(name);

        _fileRows.Add(new FileRow
        {
          Selected = !isExcludedByName,   // ← これだけで制御
          FileName = name,
          FullPath = path,
          Status = ""
        });
      }
    }


  }
}

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

    // ← 追加：グリッドのデータソース
    private readonly BindingList<FileRow> _fileRows = new();

    private static readonly Encoding Utf8NoBom = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false);
    private static readonly Encoding Sjis = Encoding.GetEncoding(932); // CP932

    // 追加：DataGridView用の1行モデル
    public sealed class FileRow : INotifyPropertyChanged
    {
      private string _fileName = string.Empty;
      private string _fullPath = string.Empty;
      private string _status = string.Empty;

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

    public MainForm()
    {

      InitializeComponent();

      // 実行ファイルのあるフォルダに保存
      var baseDir = AppContext.BaseDirectory;
      _jsonPath = Path.Combine(baseDir, AppConstants.JsonFileName);

      ucFoobar2000.TitleText = AppConstants.Titlefoobar2000; ;
      ucMedia.TitleText = AppConstants.TitleMedia;
      ucPhone.TitleText = AppConstants.TitlePhone;

      this.Load += MainForm_Load;
      this.FormClosing += MainForm_FormClosing;

      // ★追加：DataGridView 初期化
      InitFileGrid();

      // ★追加：ボタンクリック
      btnMakeMedia.Click += async (_, __) => await BtnMakeMedia_ClickAsync();

    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
      LoadTextBoxData();
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
        Smartphone = ucPhone.FolderPath
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
      dgvFiles.ReadOnly = true;
      dgvFiles.AllowUserToAddRows = false;
      dgvFiles.AllowUserToDeleteRows = false;
      dgvFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
      dgvFiles.MultiSelect = false;

      // 列定義（FileName, Status）
      var colName = new DataGridViewTextBoxColumn
      {
        DataPropertyName = nameof(FileRow.FileName),
        HeaderText = "ファイル名",
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        FillWeight = 70
      };
      var colStatus = new DataGridViewTextBoxColumn
      {
        DataPropertyName = nameof(FileRow.Status),
        HeaderText = "ステータス",
        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
        FillWeight = 30
      };

      dgvFiles.Columns.Clear();
      dgvFiles.Columns.Add(colName);
      dgvFiles.Columns.Add(colStatus);

      dgvFiles.DataSource = _fileRows;
    }

    private async Task BtnMakeMedia_ClickAsync()
    {
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

      // グリッドに一覧を作る（トップディレクトリのみ）
      _fileRows.Clear();
      foreach (var path in Directory.EnumerateFiles(srcDir, "*.m3u", SearchOption.TopDirectoryOnly))
      {
        _fileRows.Add(new FileRow
        {
          FileName = Path.GetFileName(path),
          FullPath = path,
          Status = "待機中"
        });
      }

      if (_fileRows.Count == 0)
      {
        MessageBox.Show("m3u ファイルが見つかりません。", "情報",
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        return;
      }

      btnMakeMedia.Enabled = false;
      try
      {
        // 上から順に1件ずつ処理（逐次・awaitでUIは固まらない）
        for (int i = 0; i < _fileRows.Count; i++)
        {
            UpdateStatus(i, "処理中…");
            var row = _fileRows[i];

            // ★ 追加：置換用ルート（末尾の \ を除外）
            var mediaRootForReplace = ucMedia.FolderPath.TrimEnd('\\', '/');

            var ok = await ConvertM3UToUtf8Async(
                srcPath: row.FullPath,
                dstDir:  dstDir,
                mediaRootForReplace: mediaRootForReplace
            );

            UpdateStatus(i, ok ? "成功" : "失敗");
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






  }

}

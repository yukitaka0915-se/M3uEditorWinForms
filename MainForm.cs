using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;


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

  }
}

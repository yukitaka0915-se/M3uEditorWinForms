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

    private readonly string _filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "textboxdata.json");

    public MainForm()
    {
      InitializeComponent();
      this.Load += MainForm_Load;
      this.FormClosing += MainForm_FormClosing;
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      uCtrl_FldrBrowse_NASPlayLists.lbl_title.Text = "プレイリスト群を参照するフォルダを指定してください。";
      uCtrl_FldrBrowse_uPnpPlayLists.lbl_title.Text = "プレイリスト群を保存するフォルダを指定してください。";
      uCtrl_FldrBrowse_PhoneFavorite.lbl_title.Text = "スマートフォンへファイルを転送するフォルダを指定してください。";
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
      if (!File.Exists(_filePath)) return;

      var json = File.ReadAllText(_filePath);
      var data = JsonSerializer.Deserialize<TextBoxData>(json);

      if (data == null) return;

      foreach (var ctrl in this.Controls.OfType<UserControl_FolderBrowse>())
      {
        if (!string.IsNullOrEmpty(ctrl.Identifier) && data.TryGetValue(ctrl.Identifier, out string? value))
        {
          ctrl.SetText(value ?? string.Empty);
        }
      }
    }

    private void SaveTextBoxData()
    {
      var data = new TextBoxData();

      foreach (var ctrl in this.Controls.OfType<UserControl_FolderBrowse>())
      {
        if (!string.IsNullOrEmpty(ctrl.Identifier))
        {
          // 改行コードなし、エスケープは自動
          data[ctrl.Identifier] = ctrl.GetText();
        }
      }

      var options = new JsonSerializerOptions
      {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true
      };

      string json = JsonSerializer.Serialize(data, options);
      File.WriteAllText(_filePath, json);
    }

  }
}

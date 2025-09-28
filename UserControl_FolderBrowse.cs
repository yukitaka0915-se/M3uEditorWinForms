using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace M3uEditorWinForms
{
  public partial class UserControl_FolderBrowse : UserControl
  {
    public UserControl_FolderBrowse()
    {
      InitializeComponent();

      // 推奨プロパティ
      txt_folderpath.Multiline = true;
      txt_folderpath.AcceptsReturn = false; // 改行を入れない
      txt_folderpath.AcceptsTab = false;
      txt_folderpath.WordWrap = true;

      txt_folderpath.TextChanged += txt_folderpath_TextChanged;
      btnBrowse.Click += btn_folderbrows_Click;

    }

    private void btnBrowse_Click(object sender, EventArgs e)
    {

      using var fbd = new FolderBrowserDialog
      {
        RootFolder = Environment.SpecialFolder.Desktop,
        SelectedPath = FolderPath,
        ShowNewFolderButton = false,
        Description = "フォルダを選択してください。"
      };
      if (Directory.Exists(FolderPath))
        fbd.SelectedPath = FolderPath;

      if (fbd.ShowDialog(FindForm()) == DialogResult.OK)
      {
        FolderPath = fbd.SelectedPath;
      }


    }

    private void btn_folderbrows_Click(object? sender, EventArgs e)
    {

      //folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
      //folderBrowserDialog1.SelectedPath = txt_folderpath.Text;
      //folderBrowserDialog1.Description = "フォルダを選択してください。";
      //if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
      //{
      //  this.SetText(Folderpath_molding(folderBrowserDialog1.SelectedPath));
      //}

    }
    private void txtPath_TextChanged(object sender, EventArgs e)
    {

      if (_internalUpdate) return;

      try
      {
        _internalUpdate = true;

        var original = txtPath.Text;
        var normalized = NormalizePath(original);

        if (!string.Equals(original, normalized, StringComparison.Ordinal))
          txtPath.Text = normalized;

        txtPath.BackColor = Directory.Exists(normalized)
            ? SystemColors.Window
            : Color.LightPink;
      }
      finally
      {
        _internalUpdate = false;
        // キャレットを末尾へ（必要なら）
        txtPath.SelectionStart = txtPath.TextLength;
        txtPath.SelectionLength = 0;
      }

    }

    private void txt_folderpath_TextChanged(object? sender, EventArgs e)
    {


      //DirectoryInfo di = new DirectoryInfo(txt_folderpath.Text);
      //if (di.Exists)
      //{
      //  txt_folderpath.BackColor = Color.White;
      //}
      //else
      //{
      //  txt_folderpath.BackColor = Color.Pink;
      //}


    }

    private static string NormalizePath(string? input)
    {
      if (string.IsNullOrWhiteSpace(input)) return string.Empty;

      var s = input.Trim();

      // 改行除去
      s = s.Replace("\r", string.Empty).Replace("\n", string.Empty);

      // 全角空白→半角（任意）
      s = s.Replace('　', ' ');

      // ダブルクォート除去（クリップボード貼付対策）
      s = s.Trim('"');

      // / を \ に統一
      s = s.Replace('/', '\\');

      // 先頭が UNC かどうか維持しつつ重複 \ を整理（先頭の \\ は残す）
      // 例: "\\\\server\\share\\path" → "\\server\share\path"
      if (s.StartsWith(@"\\"))
      {
        s = @"\\" + Regex.Replace(s.Substring(2), @"\\{2,}", @"\");
      }
      else
      {
        s = Regex.Replace(s, @"\\{2,}", @"\");
      }

      // 末尾に \ を付与（空やドライブ直下も \ を保持）
      if (!s.EndsWith(@"\"))
        s += @"\";

      return s;
    }






    //private void txt_folderpath_Leave(object sender, EventArgs e)
    //{
    //  if (txt_folderpath.BackColor == Color.White)
    //  {

    //    this.SetText(Folderpath_molding(txt_folderpath.Text));

    //  }
    //}

    //private static string Folderpath_molding (string s)
    //{

    //  if (!s.EndsWith("\\"))
    //  {
    //    s += "\\";
    //  }

    //  return s;
    //}

  }

}

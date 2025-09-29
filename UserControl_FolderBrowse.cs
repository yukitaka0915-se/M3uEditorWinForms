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

    public event EventHandler<string>? ValidFolderConfirmed; // ★追加（引数は正規化後パス）
                                                             // 追加：共通処理
    public UserControl_FolderBrowse()
    {
      InitializeComponent();

      // 推奨プロパティ
      txtPath.Multiline = true;
      txtPath.AcceptsReturn = false; // 改行を入れない
      txtPath.AcceptsTab = false;
      txtPath.WordWrap = true;

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
        ApplyNormalizationAndValidation(); // ←追加
      }

    }

    private void txtPath_Leave(object sender, EventArgs e)
    {
      ApplyNormalizationAndValidation();

    }

    private void ApplyNormalizationAndValidation()
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
        
        // 有効フォルダになったら通知
        ValidFolderConfirmed?.Invoke(this, txtPath.Text);
      }

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



  }

}

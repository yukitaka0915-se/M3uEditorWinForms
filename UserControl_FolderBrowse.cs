using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
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
    }

    private void btn_folderblows_Click(object sender, EventArgs e)
    {

      folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;
      folderBrowserDialog1.SelectedPath = txt_folderpath.Text;
      folderBrowserDialog1.Description = "フォルダを選択してください。";
      if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
      {
        this.SetText(Folderpath_molding(folderBrowserDialog1.SelectedPath));
      }

    }

    private void txt_folderpath_TextChanged(object sender, EventArgs e)
    {
      DirectoryInfo di = new DirectoryInfo(txt_folderpath.Text);
      if (di.Exists)
      {
        txt_folderpath.BackColor = Color.White;
      }
      else
      {
        txt_folderpath.BackColor = Color.Pink;
      }
    }

    private void txt_folderpath_Leave(object sender, EventArgs e)
    {
      if (txt_folderpath.BackColor == Color.White)
      {

        this.SetText(Folderpath_molding(txt_folderpath.Text));

      }
    }

    private static string Folderpath_molding (string s)
    {

      if (!s.EndsWith("\\"))
      {
        s += "\\";
      }

      return s;
    }

  }

}

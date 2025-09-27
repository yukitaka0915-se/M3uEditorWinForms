using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.Json;

namespace M3uEditorWinForms
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    private void Form1_Load(object sender, EventArgs e)
    {
      uCtrl_FldrBrowse_NASPlayLists.lbl_title.Text = "プレイリスト群を参照するフォルダを指定してください。";
      uCtrl_FldrBrowse_uPnpPlayLists.lbl_title.Text = "プレイリスト群を保存するフォルダを指定してください。";
      uCtrl_FldrBrowse_PhoneFavorite.lbl_title.Text = "スマートフォンへファイルを転送するフォルダを指定してください。";
    }
  }
}

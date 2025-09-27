namespace M3uEditorWinForms
{
  partial class MainForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      uCtrl_FldrBrowse_NASPlayLists = new UserControl_FolderBrowse();
      uCtrl_FldrBrowse_PhoneFavorite = new UserControl_FolderBrowse();
      uCtrl_FldrBrowse_uPnpPlayLists = new UserControl_FolderBrowse();
      SuspendLayout();
      // 
      // uCtrl_FldrBrowse_NASPlayLists
      // 
      uCtrl_FldrBrowse_NASPlayLists.Location = new Point(12, 12);
      uCtrl_FldrBrowse_NASPlayLists.Name = "uCtrl_FldrBrowse_NASPlayLists";
      uCtrl_FldrBrowse_NASPlayLists.Size = new Size(618, 76);
      uCtrl_FldrBrowse_NASPlayLists.TabIndex = 0;
      // 
      // uCtrl_FldrBrowse_PhoneFavorite
      // 
      uCtrl_FldrBrowse_PhoneFavorite.Location = new Point(12, 176);
      uCtrl_FldrBrowse_PhoneFavorite.Name = "uCtrl_FldrBrowse_PhoneFavorite";
      uCtrl_FldrBrowse_PhoneFavorite.Size = new Size(618, 76);
      uCtrl_FldrBrowse_PhoneFavorite.TabIndex = 1;
      // 
      // uCtrl_FldrBrowse_uPnpPlayLists
      // 
      uCtrl_FldrBrowse_uPnpPlayLists.Location = new Point(12, 94);
      uCtrl_FldrBrowse_uPnpPlayLists.Name = "uCtrl_FldrBrowse_uPnpPlayLists";
      uCtrl_FldrBrowse_uPnpPlayLists.Size = new Size(618, 76);
      uCtrl_FldrBrowse_uPnpPlayLists.TabIndex = 2;
      // 
      // Form1
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(uCtrl_FldrBrowse_uPnpPlayLists);
      Controls.Add(uCtrl_FldrBrowse_PhoneFavorite);
      Controls.Add(uCtrl_FldrBrowse_NASPlayLists);
      Name = "Form1";
      Text = "Foobar2000用m3uファイルの再設定";
      Load += Form1_Load;
      ResumeLayout(false);
    }

    #endregion

    private UserControl_FolderBrowse uCtrl_FldrBrowse_NASPlayLists;
    private UserControl_FolderBrowse uCtrl_FldrBrowse_PhoneFavorite;
    private UserControl_FolderBrowse uCtrl_FldrBrowse_uPnpPlayLists;
  }
}
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
      SuspendLayout();
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Name = "MainForm";
      Text = "Foobar2000用m3uファイルの再設定";
      Load += MainForm_Load;
      ResumeLayout(false);
    }

    #endregion

    private UserControl_FolderBrowse uCtrl_FldrBrowse_NASPlayLists;
    private UserControl_FolderBrowse uCtrl_FldrBrowse_PhoneFavorite;
    private UserControl_FolderBrowse uCtrl_FldrBrowse_uPnpPlayLists;
  }
}
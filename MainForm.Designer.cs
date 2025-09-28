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
      ucFoobar2000 = new UserControl_FolderBrowse();
      ucPhone = new UserControl_FolderBrowse();
      ucMedia = new UserControl_FolderBrowse();
      SuspendLayout();
      // 
      // ucFoobar2000
      // 
      ucFoobar2000.FolderPath = "";
      ucFoobar2000.Identifier = null;
      ucFoobar2000.Location = new Point(12, 12);
      ucFoobar2000.Name = "ucFoobar2000";
      ucFoobar2000.Size = new Size(618, 76);
      ucFoobar2000.TabIndex = 0;
      ucFoobar2000.TitleText = "タイトルを表示";
      // 
      // ucPhone
      // 
      ucPhone.FolderPath = "";
      ucPhone.Identifier = null;
      ucPhone.Location = new Point(12, 176);
      ucPhone.Name = "ucPhone";
      ucPhone.Size = new Size(618, 76);
      ucPhone.TabIndex = 1;
      ucPhone.TitleText = "タイトルを表示";
      // 
      // ucMedia
      // 
      ucMedia.FolderPath = "";
      ucMedia.Identifier = null;
      ucMedia.Location = new Point(12, 94);
      ucMedia.Name = "ucMedia";
      ucMedia.Size = new Size(618, 76);
      ucMedia.TabIndex = 2;
      ucMedia.TitleText = "タイトルを表示";
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(800, 450);
      Controls.Add(ucMedia);
      Controls.Add(ucPhone);
      Controls.Add(ucFoobar2000);
      Name = "MainForm";
      Text = "Foobar2000用m3uファイルの再設定";
      Load += MainForm_Load;
      ResumeLayout(false);
    }
    private UserControl_FolderBrowse ucFoobar2000;
    private UserControl_FolderBrowse ucPhone;
    private UserControl_FolderBrowse ucMedia;

    #endregion

  }
}
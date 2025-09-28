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
      btnMakeMedia = new Button();
      btnMakePhoneCpy = new Button();
      btnQuit = new Button();
      dgvFiles = new DataGridView();
      FileName = new DataGridViewTextBoxColumn();
      FullPath = new DataGridViewTextBoxColumn();
      Status = new DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)dgvFiles).BeginInit();
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
      // btnMakeMedia
      // 
      btnMakeMedia.Font = new Font("メイリオ", 9.75F);
      btnMakeMedia.Location = new Point(636, 66);
      btnMakeMedia.Name = "btnMakeMedia";
      btnMakeMedia.Size = new Size(141, 50);
      btnMakeMedia.TabIndex = 3;
      btnMakeMedia.Text = "メディアサーバーへプレイリストを転送";
      btnMakeMedia.UseVisualStyleBackColor = true;
      // 
      // btnMakePhoneCpy
      // 
      btnMakePhoneCpy.Font = new Font("メイリオ", 9.75F);
      btnMakePhoneCpy.Location = new Point(636, 156);
      btnMakePhoneCpy.Name = "btnMakePhoneCpy";
      btnMakePhoneCpy.Size = new Size(141, 50);
      btnMakePhoneCpy.TabIndex = 4;
      btnMakePhoneCpy.Text = "スマートフォン転送用ファイルを構築";
      btnMakePhoneCpy.UseVisualStyleBackColor = true;
      // 
      // btnQuit
      // 
      btnQuit.Font = new Font("メイリオ", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
      btnQuit.ForeColor = Color.Red;
      btnQuit.Location = new Point(636, 503);
      btnQuit.Name = "btnQuit";
      btnQuit.Size = new Size(141, 44);
      btnQuit.TabIndex = 5;
      btnQuit.Text = "終了";
      btnQuit.UseVisualStyleBackColor = true;
      btnQuit.Click += btnQuit_Click;
      // 
      // dgvFiles
      // 
      dgvFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dgvFiles.Columns.AddRange(new DataGridViewColumn[] { FileName, FullPath, Status });
      dgvFiles.Location = new Point(12, 258);
      dgvFiles.Name = "dgvFiles";
      dgvFiles.Size = new Size(765, 239);
      dgvFiles.TabIndex = 6;
      // 
      // FileName
      // 
      FileName.HeaderText = "FileName";
      FileName.Name = "FileName";
      // 
      // FullPath
      // 
      FullPath.HeaderText = "FullPath";
      FullPath.Name = "FullPath";
      FullPath.Visible = false;
      // 
      // Status
      // 
      Status.HeaderText = "Status";
      Status.Name = "Status";
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(798, 559);
      Controls.Add(dgvFiles);
      Controls.Add(btnQuit);
      Controls.Add(btnMakePhoneCpy);
      Controls.Add(btnMakeMedia);
      Controls.Add(ucMedia);
      Controls.Add(ucPhone);
      Controls.Add(ucFoobar2000);
      Name = "MainForm";
      Text = "Foobar2000用m3uファイルの再設定";
      Load += MainForm_Load;
      ((System.ComponentModel.ISupportInitialize)dgvFiles).EndInit();
      ResumeLayout(false);
    }
    private UserControl_FolderBrowse ucFoobar2000;
    private UserControl_FolderBrowse ucPhone;
    private UserControl_FolderBrowse ucMedia;

    #endregion

    private Button btnMakeMedia;
    private Button btnMakePhoneCpy;
    private Button btnQuit;
    private DataGridView dgvFiles;
    private DataGridViewTextBoxColumn FileName;
    private DataGridViewTextBoxColumn FullPath;
    private DataGridViewTextBoxColumn Status;
  }
}
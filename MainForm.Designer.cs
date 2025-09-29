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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
      ucFoobar2000 = new UserControl_FolderBrowse();
      ucPhone = new UserControl_FolderBrowse();
      ucMedia = new UserControl_FolderBrowse();
      btnMakeMedia = new Button();
      btnMakePhoneCpy = new Button();
      btnQuit = new Button();
      dgvFiles = new DataGridView();
      toolStrip1 = new ToolStrip();
      tslExclude = new ToolStripLabel();
      tscbExclude = new ToolStripComboBox();
      tsbAddExclude = new ToolStripButton();
      tsbRemoveExclude = new ToolStripButton();
      Selected = new DataGridViewCheckBoxColumn();
      FileName = new DataGridViewTextBoxColumn();
      FullPath = new DataGridViewTextBoxColumn();
      Status = new DataGridViewTextBoxColumn();
      ((System.ComponentModel.ISupportInitialize)dgvFiles).BeginInit();
      toolStrip1.SuspendLayout();
      SuspendLayout();
      // 
      // ucFoobar2000
      // 
      ucFoobar2000.FolderPath = "";
      ucFoobar2000.Identifier = null;
      ucFoobar2000.Location = new Point(12, 28);
      ucFoobar2000.Name = "ucFoobar2000";
      ucFoobar2000.Size = new Size(618, 76);
      ucFoobar2000.TabIndex = 0;
      ucFoobar2000.TitleText = "タイトルを表示";
      // 
      // ucPhone
      // 
      ucPhone.FolderPath = "";
      ucPhone.Identifier = null;
      ucPhone.Location = new Point(12, 192);
      ucPhone.Name = "ucPhone";
      ucPhone.Size = new Size(618, 76);
      ucPhone.TabIndex = 1;
      ucPhone.TitleText = "タイトルを表示";
      // 
      // ucMedia
      // 
      ucMedia.FolderPath = "";
      ucMedia.Identifier = null;
      ucMedia.Location = new Point(12, 110);
      ucMedia.Name = "ucMedia";
      ucMedia.Size = new Size(618, 76);
      ucMedia.TabIndex = 2;
      ucMedia.TitleText = "タイトルを表示";
      // 
      // btnMakeMedia
      // 
      btnMakeMedia.Font = new Font("メイリオ", 9.75F);
      btnMakeMedia.Location = new Point(636, 91);
      btnMakeMedia.Name = "btnMakeMedia";
      btnMakeMedia.Size = new Size(141, 50);
      btnMakeMedia.TabIndex = 3;
      btnMakeMedia.Text = "メディアサーバーへプレイリストを転送";
      btnMakeMedia.UseVisualStyleBackColor = true;
      // 
      // btnMakePhoneCpy
      // 
      btnMakePhoneCpy.Font = new Font("メイリオ", 9.75F);
      btnMakePhoneCpy.Location = new Point(636, 181);
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
      btnQuit.Location = new Point(636, 531);
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
      dgvFiles.Columns.AddRange(new DataGridViewColumn[] { Selected, FileName, FullPath, Status });
      dgvFiles.Location = new Point(12, 274);
      dgvFiles.Name = "dgvFiles";
      dgvFiles.Size = new Size(765, 251);
      dgvFiles.TabIndex = 6;
      // 
      // toolStrip1
      // 
      toolStrip1.Items.AddRange(new ToolStripItem[] { tslExclude, tscbExclude, tsbAddExclude, tsbRemoveExclude });
      toolStrip1.Location = new Point(0, 0);
      toolStrip1.Name = "toolStrip1";
      toolStrip1.Size = new Size(798, 25);
      toolStrip1.TabIndex = 7;
      toolStrip1.Text = "toolStrip1";
      // 
      // tslExclude
      // 
      tslExclude.Name = "tslExclude";
      tslExclude.Size = new Size(31, 22);
      tslExclude.Text = "除外";
      // 
      // tscbExclude
      // 
      tscbExclude.Name = "tscbExclude";
      tscbExclude.Size = new Size(400, 25);
      // 
      // tsbAddExclude
      // 
      tsbAddExclude.DisplayStyle = ToolStripItemDisplayStyle.Image;
      tsbAddExclude.Image = (Image)resources.GetObject("tsbAddExclude.Image");
      tsbAddExclude.ImageTransparentColor = Color.Magenta;
      tsbAddExclude.Name = "tsbAddExclude";
      tsbAddExclude.Size = new Size(23, 22);
      tsbAddExclude.Text = "＋";
      // 
      // tsbRemoveExclude
      // 
      tsbRemoveExclude.DisplayStyle = ToolStripItemDisplayStyle.Image;
      tsbRemoveExclude.Image = (Image)resources.GetObject("tsbRemoveExclude.Image");
      tsbRemoveExclude.ImageTransparentColor = Color.Magenta;
      tsbRemoveExclude.Name = "tsbRemoveExclude";
      tsbRemoveExclude.Size = new Size(23, 22);
      tsbRemoveExclude.Text = "ー";
      // 
      // Selected
      // 
      Selected.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      Selected.FillWeight = 60.9137039F;
      Selected.HeaderText = "選択";
      Selected.Name = "Selected";
      // 
      // FileName
      // 
      FileName.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      FileName.FillWeight = 119.543152F;
      FileName.HeaderText = "FileName";
      FileName.Name = "FileName";
      FileName.ReadOnly = true;
      // 
      // FullPath
      // 
      FullPath.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      FullPath.HeaderText = "FullPath";
      FullPath.Name = "FullPath";
      FullPath.ReadOnly = true;
      FullPath.Visible = false;
      // 
      // Status
      // 
      Status.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
      Status.FillWeight = 119.543152F;
      Status.HeaderText = "Status";
      Status.Name = "Status";
      Status.ReadOnly = true;
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(798, 587);
      Controls.Add(toolStrip1);
      Controls.Add(dgvFiles);
      Controls.Add(btnQuit);
      Controls.Add(btnMakePhoneCpy);
      Controls.Add(btnMakeMedia);
      Controls.Add(ucMedia);
      Controls.Add(ucPhone);
      Controls.Add(ucFoobar2000);
      Name = "MainForm";
      Text = "Foobar2000用m3uファイルの再設定";
      FormClosing += MainForm_FormClosing;
      Load += MainForm_Load;
      ((System.ComponentModel.ISupportInitialize)dgvFiles).EndInit();
      toolStrip1.ResumeLayout(false);
      toolStrip1.PerformLayout();
      ResumeLayout(false);
      PerformLayout();
    }
    private UserControl_FolderBrowse ucFoobar2000;
    private UserControl_FolderBrowse ucPhone;
    private UserControl_FolderBrowse ucMedia;

    #endregion

    private Button btnMakeMedia;
    private Button btnMakePhoneCpy;
    private Button btnQuit;
    private DataGridView dgvFiles;
    private ToolStrip toolStrip1;
    private ToolStripLabel tslExclude;
    private ToolStripComboBox tscbExclude;
    private ToolStripButton tsbAddExclude;
    private ToolStripButton tsbRemoveExclude;
    private DataGridViewCheckBoxColumn Selected;
    private DataGridViewTextBoxColumn FileName;
    private DataGridViewTextBoxColumn FullPath;
    private DataGridViewTextBoxColumn Status;
  }
}
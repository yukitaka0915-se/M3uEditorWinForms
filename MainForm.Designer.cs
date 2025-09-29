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
      btnQuit = new Button();
      dgvFiles = new DataGridView();
      Selected = new DataGridViewCheckBoxColumn();
      FileName = new DataGridViewTextBoxColumn();
      FullPath = new DataGridViewTextBoxColumn();
      Status = new DataGridViewTextBoxColumn();
      toolStrip1 = new ToolStrip();
      tslExclude = new ToolStripLabel();
      tscbExclude = new ToolStripComboBox();
      tsbAddExclude = new ToolStripButton();
      tsbRemoveExclude = new ToolStripButton();
      groupBox1 = new GroupBox();
      btnMakeMedia = new Button();
      ucMedia = new UserControl_FolderBrowse();
      ucFoobar2000 = new UserControl_FolderBrowse();
      groupBox2 = new GroupBox();
      ucParent = new UserControl_FolderBrowse();
      btnMakePhoneCpy = new Button();
      ucPhone = new UserControl_FolderBrowse();
      ((System.ComponentModel.ISupportInitialize)dgvFiles).BeginInit();
      toolStrip1.SuspendLayout();
      groupBox1.SuspendLayout();
      groupBox2.SuspendLayout();
      SuspendLayout();
      // 
      // btnQuit
      // 
      btnQuit.Font = new Font("メイリオ", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
      btnQuit.ForeColor = Color.Red;
      btnQuit.Location = new Point(642, 560);
      btnQuit.Name = "btnQuit";
      btnQuit.Size = new Size(94, 44);
      btnQuit.TabIndex = 5;
      btnQuit.Text = "終了";
      btnQuit.UseVisualStyleBackColor = true;
      btnQuit.Click += btnQuit_Click;
      // 
      // dgvFiles
      // 
      dgvFiles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      dgvFiles.Columns.AddRange(new DataGridViewColumn[] { Selected, FileName, FullPath, Status });
      dgvFiles.Location = new Point(12, 328);
      dgvFiles.Name = "dgvFiles";
      dgvFiles.Size = new Size(742, 226);
      dgvFiles.TabIndex = 6;
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
      // toolStrip1
      // 
      toolStrip1.Items.AddRange(new ToolStripItem[] { tslExclude, tscbExclude, tsbAddExclude, tsbRemoveExclude });
      toolStrip1.Location = new Point(0, 0);
      toolStrip1.Name = "toolStrip1";
      toolStrip1.Size = new Size(764, 25);
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
      tsbAddExclude.DisplayStyle = ToolStripItemDisplayStyle.Text;
      tsbAddExclude.Image = (Image)resources.GetObject("tsbAddExclude.Image");
      tsbAddExclude.ImageTransparentColor = Color.Magenta;
      tsbAddExclude.Name = "tsbAddExclude";
      tsbAddExclude.Size = new Size(23, 22);
      tsbAddExclude.Text = "＋";
      // 
      // tsbRemoveExclude
      // 
      tsbRemoveExclude.DisplayStyle = ToolStripItemDisplayStyle.Text;
      tsbRemoveExclude.Image = (Image)resources.GetObject("tsbRemoveExclude.Image");
      tsbRemoveExclude.ImageTransparentColor = Color.Magenta;
      tsbRemoveExclude.Name = "tsbRemoveExclude";
      tsbRemoveExclude.Size = new Size(23, 22);
      tsbRemoveExclude.Text = "ー";
      // 
      // groupBox1
      // 
      groupBox1.Controls.Add(btnMakeMedia);
      groupBox1.Controls.Add(ucMedia);
      groupBox1.Controls.Add(ucFoobar2000);
      groupBox1.Location = new Point(12, 28);
      groupBox1.Name = "groupBox1";
      groupBox1.Size = new Size(742, 143);
      groupBox1.TabIndex = 9;
      groupBox1.TabStop = false;
      groupBox1.Text = "メディアサーバー用プレイリストを作成";
      // 
      // btnMakeMedia
      // 
      btnMakeMedia.Font = new Font("メイリオ", 9.75F);
      btnMakeMedia.Location = new Point(630, 63);
      btnMakeMedia.Name = "btnMakeMedia";
      btnMakeMedia.Size = new Size(94, 50);
      btnMakeMedia.TabIndex = 6;
      btnMakeMedia.Text = "開始";
      btnMakeMedia.UseVisualStyleBackColor = true;
      // 
      // ucMedia
      // 
      ucMedia.FolderPath = "";
      ucMedia.Identifier = null;
      ucMedia.Location = new Point(6, 77);
      ucMedia.Name = "ucMedia";
      ucMedia.Size = new Size(618, 60);
      ucMedia.TabIndex = 5;
      ucMedia.TitleText = "タイトルを表示";
      // 
      // ucFoobar2000
      // 
      ucFoobar2000.FolderPath = "";
      ucFoobar2000.Identifier = null;
      ucFoobar2000.Location = new Point(6, 22);
      ucFoobar2000.Name = "ucFoobar2000";
      ucFoobar2000.Size = new Size(618, 60);
      ucFoobar2000.TabIndex = 4;
      ucFoobar2000.TitleText = "タイトルを表示";
      // 
      // groupBox2
      // 
      groupBox2.Controls.Add(ucParent);
      groupBox2.Controls.Add(btnMakePhoneCpy);
      groupBox2.Controls.Add(ucPhone);
      groupBox2.Location = new Point(12, 177);
      groupBox2.Name = "groupBox2";
      groupBox2.Size = new Size(742, 145);
      groupBox2.TabIndex = 10;
      groupBox2.TabStop = false;
      groupBox2.Text = "スマートフォン転送用フォルダへファイルを転送";
      // 
      // ucParent
      // 
      ucParent.FolderPath = "";
      ucParent.Identifier = null;
      ucParent.Location = new Point(6, 22);
      ucParent.Name = "ucParent";
      ucParent.Size = new Size(618, 60);
      ucParent.TabIndex = 11;
      ucParent.TitleText = "タイトルを表示";
      // 
      // btnMakePhoneCpy
      // 
      btnMakePhoneCpy.Font = new Font("メイリオ", 9.75F);
      btnMakePhoneCpy.Location = new Point(630, 59);
      btnMakePhoneCpy.Name = "btnMakePhoneCpy";
      btnMakePhoneCpy.Size = new Size(94, 50);
      btnMakePhoneCpy.TabIndex = 10;
      btnMakePhoneCpy.Text = "開始";
      btnMakePhoneCpy.UseVisualStyleBackColor = true;
      // 
      // ucPhone
      // 
      ucPhone.FolderPath = "";
      ucPhone.Identifier = null;
      ucPhone.Location = new Point(6, 78);
      ucPhone.Name = "ucPhone";
      ucPhone.Size = new Size(618, 60);
      ucPhone.TabIndex = 9;
      ucPhone.TitleText = "タイトルを表示";
      // 
      // MainForm
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      ClientSize = new Size(764, 610);
      Controls.Add(groupBox2);
      Controls.Add(groupBox1);
      Controls.Add(toolStrip1);
      Controls.Add(btnQuit);
      Controls.Add(dgvFiles);
      Name = "MainForm";
      Text = "Foobar2000用m3uファイルの再設定";
      FormClosing += MainForm_FormClosing;
      Load += MainForm_Load;
      ((System.ComponentModel.ISupportInitialize)dgvFiles).EndInit();
      toolStrip1.ResumeLayout(false);
      toolStrip1.PerformLayout();
      groupBox1.ResumeLayout(false);
      groupBox2.ResumeLayout(false);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion
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
    private GroupBox groupBox1;
    private Button btnMakeMedia;
    private UserControl_FolderBrowse ucMedia;
    private UserControl_FolderBrowse ucFoobar2000;
    private GroupBox groupBox2;
    private UserControl_FolderBrowse ucParent;
    private Button btnMakePhoneCpy;
    private UserControl_FolderBrowse ucPhone;
  }
}
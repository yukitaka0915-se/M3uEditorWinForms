namespace M3uEditorWinForms
{
  partial class UserControl_FolderBrowse
  {
    /// <summary> 
    /// 必要なデザイナー変数です。
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// 使用中のリソースをすべてクリーンアップします。
    /// </summary>
    /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region コンポーネント デザイナーで生成されたコード

    /// <summary> 
    /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
    /// コード エディターで変更しないでください。
    /// </summary>
    private void InitializeComponent()
    {
      txt_folderpath = new TextBox();
      btn_folderblows = new Button();
      folderBrowserDialog1 = new FolderBrowserDialog();
      openFileDialog1 = new OpenFileDialog();
      lbl_title = new Label();
      SuspendLayout();
      // 
      // txt_folderpath
      // 
      txt_folderpath.Font = new Font("メイリオ", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 128);
      txt_folderpath.Location = new Point(0, 21);
      txt_folderpath.Multiline = true;
      txt_folderpath.Name = "txt_folderpath";
      txt_folderpath.Size = new Size(547, 49);
      txt_folderpath.TabIndex = 0;
      txt_folderpath.TextChanged += txt_folderpath_TextChanged;
      txt_folderpath.Leave += txt_folderpath_Leave;
      // 
      // btn_folderblows
      // 
      btn_folderblows.Location = new Point(553, 21);
      btn_folderblows.Name = "btn_folderblows";
      btn_folderblows.Size = new Size(56, 49);
      btn_folderblows.TabIndex = 1;
      btn_folderblows.Text = "フォルダ参照";
      btn_folderblows.UseVisualStyleBackColor = true;
      btn_folderblows.Click += btn_folderblows_Click;
      // 
      // openFileDialog1
      // 
      openFileDialog1.FileName = "openFileDialog1";
      // 
      // lbl_title
      // 
      lbl_title.AutoSize = true;
      lbl_title.Font = new Font("メイリオ", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
      lbl_title.Location = new Point(0, 0);
      lbl_title.Name = "lbl_title";
      lbl_title.Size = new Size(92, 18);
      lbl_title.TabIndex = 2;
      lbl_title.Text = "タイトルを表示";
      // 
      // UserControl_FolderBrowse
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(lbl_title);
      Controls.Add(btn_folderblows);
      Controls.Add(txt_folderpath);
      Name = "UserControl_FolderBrowse";
      Size = new Size(618, 76);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button btn_folderblows;
    private FolderBrowserDialog folderBrowserDialog1;
    private OpenFileDialog openFileDialog1;

    public string Identifier { get; set; }

    public TextBox txt_folderpath
    {
      get => txt_folderpath;
      set => txt_folderpath = value;
    }
    public Label lbl_title;
    public Boolean IsFolerExists
    {
      get => (txt_folderpath.BackColor == Color.White);

    }

  }
}

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
      btnBrowse = new Button();
      folderBrowserDialog = new FolderBrowserDialog();
      lbl_title = new Label();
      txtPath = new TextBox();
      SuspendLayout();
      // 
      // btnBrowse
      // 
      btnBrowse.Location = new Point(553, 21);
      btnBrowse.Name = "btnBrowse";
      btnBrowse.Size = new Size(56, 49);
      btnBrowse.TabIndex = 1;
      btnBrowse.Text = "フォルダ参照";
      btnBrowse.UseVisualStyleBackColor = true;
      btnBrowse.Click += btnBrowse_Click;
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
      // txtPath
      // 
      txtPath.Location = new Point(5, 21);
      txtPath.Multiline = true;
      txtPath.Name = "txtPath";
      txtPath.Size = new Size(542, 49);
      txtPath.TabIndex = 3;
      txtPath.Leave += txtPath_Leave;
      // 
      // UserControl_FolderBrowse
      // 
      AutoScaleDimensions = new SizeF(7F, 15F);
      AutoScaleMode = AutoScaleMode.Font;
      Controls.Add(txtPath);
      Controls.Add(lbl_title);
      Controls.Add(btnBrowse);
      Name = "UserControl_FolderBrowse";
      Size = new Size(618, 76);
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private Button btnBrowse;
    private FolderBrowserDialog folderBrowserDialog;

    public string Identifier { get; set; }
    private bool _internalUpdate;

    private Label lbl_title;
    public string TitleText
    {
      get => lbl_title.Text;
      set => lbl_title.Text = value;
    }

    public string FolderPath
    {
      get => txtPath.Text;
      set
      {
        if (!string.Equals(txtPath.Text, value, StringComparison.Ordinal))
          txtPath.Text = value ?? string.Empty;
      }
    }


    public string GetText()
    {
      return txtPath.Text;
    }
    public void SetText(string text)
    {
      txtPath.Text = text;
    }
    private TextBox txtPath;

    public bool IsValid => Directory.Exists(txtPath.Text);

  }
}

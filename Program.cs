using System.Text;

namespace M3uEditorWinForms
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

          // 必須：CP932 等のコードページを利用可能にする
          Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

          // To customize application configuration such as set high DPI settings or default font,
          // see https://aka.ms/applicationconfiguration.
          ApplicationConfiguration.Initialize();
          Application.Run(new MainForm());

           
        }
    }
}
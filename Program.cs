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

          // �K�{�FCP932 ���̃R�[�h�y�[�W�𗘗p�\�ɂ���
          Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

          // To customize application configuration such as set high DPI settings or default font,
          // see https://aka.ms/applicationconfiguration.
          ApplicationConfiguration.Initialize();
          Application.Run(new MainForm());

           
        }
    }
}
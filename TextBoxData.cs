using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3uEditorWinForms
{
  internal class TextBoxData : Dictionary<string, string>
  {
  }

  public sealed class AppState
  {
    public string? Foobar2000 { get; set; }
    public string? MediaServer { get; set; }
    public string? Smartphone { get; set; }
  }

}

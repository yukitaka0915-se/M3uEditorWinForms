using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace M3uEditorWinForms
{

  public class AppState
  {
    public string? Foobar2000 { get; set; }
    public string? MediaServer { get; set; }
    public string? Smartphone { get; set; }
    public List<string>? ExcludeFileNames { get; set; }
  }

}

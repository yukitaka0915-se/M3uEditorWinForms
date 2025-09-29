using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace M3uEditorWinForms
{
    public static class M3uFileHelper
    {
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(false);
        private static readonly Encoding Sjis = Encoding.GetEncoding(932);

        public static async Task<bool> ConvertM3UToUtf8Async(string srcPath, string dstDir, string mediaRootForReplace)
        {
            try
            {
                string text;
                using (var reader = new StreamReader(srcPath, Sjis, false))
                {
                    text = await reader.ReadToEndAsync().ConfigureAwait(false);
                }
                var dstName = "UTF8_" + Path.GetFileName(srcPath);
                var dstPath = Path.Combine(dstDir, dstName);

                using (var writer = new StreamWriter(dstPath, false, Utf8NoBom))
                {
                    writer.NewLine = "\r\n";
                    var rootBS = mediaRootForReplace;
                    var rootSL = mediaRootForReplace.Replace('\\', '/');
                    using var sr = new StringReader(text);
                    string? line;
                    while ((line = await sr.ReadLineAsync().ConfigureAwait(false)) is not null)
                    {
                        string outLine =
                            (line.Length > 0 && line[0] != '#')
                            ? ReplaceRootPrefixWithDot(line, rootBS, rootSL)
                            : line;
                        await writer.WriteLineAsync(outLine).ConfigureAwait(false);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ReplaceRootPrefixWithDot(string line, string rootBackslash, string rootSlash)
        {
            if (StartsWithOrdinalIgnoreCase(line, rootBackslash))
                return "." + line.Substring(rootBackslash.Length);
            if (StartsWithOrdinalIgnoreCase(line, rootSlash))
                return "." + line.Substring(rootSlash.Length);
            return line;
        }

        public static bool StartsWithOrdinalIgnoreCase(string s, string prefix)
        {
            return s.Length >= prefix.Length &&
                   s.AsSpan(0, prefix.Length).Equals(prefix.AsSpan(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
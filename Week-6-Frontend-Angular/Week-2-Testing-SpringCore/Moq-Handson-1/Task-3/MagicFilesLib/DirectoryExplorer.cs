using System.Collections.Generic;
using System.IO;

namespace MagicFilesLib
{
    public class DirectoryExplorer : IDirectoryExplorer
    {
        public ICollection<string> GetFiles(string path)
        {
            // Bypasses direct static calling in higher levels by wrapping it in an implementation
            string[] files = Directory.GetFiles(path);
            return files;
        }
    }
}
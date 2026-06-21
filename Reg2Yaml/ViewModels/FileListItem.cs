using System.IO;

namespace Reg2Yaml.ViewModels
{
    public class FileListItem
    {
        public FileListItem(string fullPath)
        {
            FileInfo = new FileInfo(fullPath);
        }

        public FileInfo FileInfo { get; init; }
    }
}
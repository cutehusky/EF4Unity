using System.IO;

namespace Utils
{
    public static class DirectoryExtensions
    {
        public static void EnsureDatabaseFolderExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }
    }
}
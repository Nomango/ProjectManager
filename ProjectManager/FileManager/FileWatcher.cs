using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.FileManager
{
    class FileWatcher
    {
        private static DirectoryInfo currentDir;

        public static void Init()
        {
            currentDir = new DirectoryInfo(@"\\" + Connection.HostIP);
        }

        public static DirectoryInfo GetDirectory()
        {
            return currentDir;
        }
    }
}

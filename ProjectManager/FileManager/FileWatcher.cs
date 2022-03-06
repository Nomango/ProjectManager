using ProjectManager.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.FileManager
{
    class FileWatcher
    {
        public static ArrayList ShareNames = new ArrayList { };
        private static string currentShareName;
        private static ArrayList filePaths = new ArrayList { };
        private static ArrayList historyPaths = new ArrayList { };

        public static void Init()
        {
            ShareNames.Clear();
            var shares = ShareCollection.GetShares(Connection.HostIP);
            if (shares != null)
            {
                foreach (Share share in shares)
                {
                    if (share.IsFileSystem)
                    {
                        ShareNames.Add(share.NetName);
                    }
                }
            }
            if (ShareNames.Count == 0)
            {
                throw new Exception("在 " + Connection.HostIP + " 地址下找不到共享文件！");
            }
        }

        public static void SelectShare(string shareName)
        {
            currentShareName = shareName;
            filePaths.Clear();
            historyPaths.Clear();
        }

        public static DirectoryInfo GetCurrentDir()
        {
            var path = @"\" + currentShareName;
            if (filePaths.Count > 0)
            {
                path += @"\" + string.Join(@"\", filePaths.ToArray());
            }
            return new DirectoryInfo(@"\\" + Connection.HostIP + path);
        }

        public static void Enter(string folderName)
        {
            filePaths.Add(folderName);
            historyPaths.Clear();
        }

        public static bool Backward()
        {
            if (filePaths.Count == 0)
                return false;
            historyPaths.Add(filePaths[filePaths.Count - 1]);
            filePaths.RemoveAt(filePaths.Count - 1);
            return true;
        }

        public static bool Forward()
        {
            if (historyPaths.Count == 0)
                return false;
            filePaths.Add(historyPaths[historyPaths.Count - 1]);
            historyPaths.RemoveAt(historyPaths.Count - 1);
            return historyPaths.Count > 0;
        }
    }
}

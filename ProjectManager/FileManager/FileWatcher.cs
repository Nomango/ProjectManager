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
        public static ArrayList ShareNames;
        private static DirectoryInfo currentDir;

        public static void Init()
        {
            ShareNames = new ArrayList { };
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
            currentDir = new DirectoryInfo(@"\\" + Connection.HostIP + @"\" + ShareNames[0]);
        }

        public static DirectoryInfo GetDirectory()
        {
            return currentDir;
        }
    }
}

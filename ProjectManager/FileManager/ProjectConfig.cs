using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.FileManager
{
    public static class ProjectConfig
    {
        public static string[] subDirs = { "素材", "落地页", "杂项" };
        public static string[] SubDirs
        {
            get
            {
                return subDirs;
            }
        }

        public static string CreateProjectFolders(string projectName)
        {
            var relativePath = FileWatcher.PathSpliter + FormatProjectName(projectName);
            var folderPath = FileWatcher.Instance.GetFullPath(relativePath);
            if (Directory.Exists(folderPath))
            {
                Util.ShowErrorMessage("项目已存在");
                return null;
            }
            _ = Directory.CreateDirectory(folderPath);
            foreach (var subDir in SubDirs)
            {
                var subFolderPath = folderPath + FileWatcher.PathSpliter + subDir;
                _ = Directory.CreateDirectory(subFolderPath);
            }
            return relativePath;
        }

        public static string FormatProjectName(string projectName)
        {
            return string.Format("{0:yyyy-MM-dd} {1}", DateTime.Now, projectName);
        }

        public static bool CheckProjectName(string projectName)
        {
            var relativePath = FileWatcher.PathSpliter + FormatProjectName(projectName);
            var folderPath = FileWatcher.Instance.GetFullPath(relativePath);
            return !Directory.Exists(folderPath);
        }
    }
}
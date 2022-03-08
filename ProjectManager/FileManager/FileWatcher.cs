using FirstFloor.ModernUI.Presentation;
using ProjectManager.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace ProjectManager.FileManager
{
    public class FileItem
    {
        public string Name { get; set; }
        public string FullPath { get; set; }
        public bool IsFolder { get; set; }

        private ImageSource icon;
        public ImageSource Icon
        {
            get
            {
                if (icon == null)
                {
                    if (IsFolder)
                    {
                        icon = Util.GetDefaultFolderIcon();
                    }
                    else
                    {
                        var extension = System.IO.Path.GetExtension(Name);
                        if (extension.Equals(".exe"))
                        {
                            icon = IconUtil.IconToImageSource(IconUtil.GetFileIcon(FullPath, false));
                        }
                        else
                        {
                            icon = IconUtil.IconToImageSource(IconUtil.GetFileIcon(Name, false));
                        }
                    }
                }
                return icon;
            }
        }
    }

    public class FileWatcher : NotifyPropertyChanged
    {
        private static FileWatcher instance = new FileWatcher();
        public static FileWatcher Instance
        {
            get
            {
                return instance;
            }
        }

        private List<string> shareNames;
        public List<string> ShareNames
        {
            get { return shareNames; }
            set
            {
                if (shareNames != value)
                {
                    shareNames = value;
                    OnPropertyChanged("ShareNames");
                    this.CurrentShareName = shareNames.FirstOrDefault();
                    this.CurrentPath = "";
                }
            }
        }

        private string currentShareName;
        public string CurrentShareName
        {
            get { return currentShareName; }
            set
            {
                if (currentShareName != value)
                {
                    currentShareName = value;
                    OnPropertyChanged("CurrentShareName");
                }
            }
        }

        private List<FileItem> currentFileItems;
        public List<FileItem> CurrentFileItems
        {
            get { return currentFileItems; }
            set
            {
                if (currentFileItems != value)
                {
                    currentFileItems = value;
                    OnPropertyChanged("CurrentFileItems");
                }
            }
        }

        private List<FileItem> selectedFileItems;
        public List<FileItem> SelectedFileItems
        {
            get { return selectedFileItems; }
            set
            {
                if (selectedFileItems != value)
                {
                    selectedFileItems = value;
                    OnPropertyChanged("SelectedFileItems");
                }
            }
        }

        private List<string> filePaths = new List<string>();
        private List<string> historyPaths = new List<string>();
        private string actualCurrentPath
        {
            get
            {
                return @"\" + string.Join(@"\", filePaths.ToArray());
            }
        }
        public bool CanBackward
        {
            get
            {
                return filePaths.Count > 0;
            }
        }
        public bool CanForward
        {
            get
            {
                return historyPaths.Count > 0;
            }
        }

        private string currentPath;
        public string CurrentPath
        {
            get
            {
                return currentPath;
            }
            set
            {
                if (currentPath != value)
                {
                    var path = value.Replace("/", @"\");
                    ApplyPath(path, true);

                    currentPath = path;
                    OnPropertyChanged("CurrentPath");
                    OnPropertyChanged("CanBackward");
                    OnPropertyChanged("CanForward");
                }
            }
        }

        private void ApplyPath(string path, bool clearHistory)
        {
            var dir = GetDir(path);
            this.CurrentFileItems = GetDirItems(dir);

            filePaths = new List<string>(from s in path.Split('/') where s != "" select s);
            if (clearHistory)
            {
                historyPaths.Clear();
            }
        }

        public void Init()
        {
            var shares = ShareCollection.GetShares(Connection.HostIP);
            if (shares != null)
            {
                this.ShareNames = new List<string>(
                    from Share share in shares.ToArray()
                    where share.IsFileSystem
                    select share.NetName);
            }
            if (ShareNames.Count == 0)
            {
                throw new Exception("在 " + Connection.HostIP + " 地址下找不到共享文件");
            }
        }

        public DirectoryInfo GetDir(string path)
        {
            var dir = new DirectoryInfo(@"\\" + Connection.HostIP + @"\" + CurrentShareName + path);
            if (!dir.Exists)
            {
                throw new Exception("文件夹 " + path + " 不存在");
            }
            return dir;
        }

        public List<FileItem> GetDirItems(DirectoryInfo dir)
        {
            var items = new List<FileItem> { };
            foreach (var subDir in dir.GetDirectories())
            {
                items.Add(new FileItem
                {
                    Name = subDir.Name,
                    FullPath = subDir.FullName,
                    IsFolder = true,
                });
            }
            foreach (var file in dir.GetFiles())
            {
                items.Add(new FileItem
                {
                    Name = file.Name,
                    FullPath = file.FullName,
                });
            }
            return items;
        }

        public void Flush()
        {
            var dir = GetDir(this.actualCurrentPath);
            this.CurrentFileItems = GetDirItems(dir);
        }

        public void Open(FileItem item)
        {
            if (item.IsFolder)
            {
                this.CurrentPath = this.actualCurrentPath + @"\" + item.Name;
            }
            else
            {
                Process.Start(item.FullPath);
            }
        }

        public void Backward()
        {
            if (filePaths.Count == 0)
                return;
            historyPaths.Add(filePaths[filePaths.Count - 1]);
            filePaths.RemoveAt(filePaths.Count - 1);
            this.CurrentPath = this.actualCurrentPath;
        }

        public void Forward()
        {
            if (historyPaths.Count == 0)
                return;
            filePaths.Add(historyPaths[historyPaths.Count - 1]);
            historyPaths.RemoveAt(historyPaths.Count - 1);
            this.CurrentPath = this.actualCurrentPath;
        }
    }
}

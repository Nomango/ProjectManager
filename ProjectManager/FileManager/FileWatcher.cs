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
                        var extension = Path.GetExtension(Name);
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
        public static char PathSpliter = '\\';
        public static string Root = @"\";

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
                    Flush();
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

        private List<string> visitHistory = new List<string>();
        private int visitIndex = 0;
        public bool CanBack
        {
            get
            {
                return visitHistory.Count > 0 && visitIndex > 0;
            }
        }
        public bool CanForward
        {
            get
            {
                return visitHistory.Count > 0 && (visitIndex + 1) < visitHistory.Count;
            }
        }
        public bool CanUp
        {
            get
            {
                return visitHistory.Count > 0 && visitHistory[visitIndex] != Root;
            }
        }

        public string CurrentPath
        {
            get
            {
                if (visitHistory.Count > 0)
                {
                    return visitHistory[visitIndex];
                }
                return Root;
            }
            set
            {
                if (CurrentPath != value && value != null)
                {
                    var path = value.Replace('/', PathSpliter);
                    path = Root + Path.Combine(path.Split(PathSpliter).Where(s => s != "").ToArray());
                    Visit(path);
                }
            }
        }

        public string CurrentFullPath
        {
            get
            {
                return GetFullPath(CurrentPath);
            }
        }

        public void NotifyPathChanged()
        {
            OnPropertyChanged("CurrentPath");
            OnPropertyChanged("CanBack");
            OnPropertyChanged("CanForward");
            OnPropertyChanged("CanUp");
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

        public string GetFullPath(string path)
        {
            return @"\\" + Connection.HostIP + PathSpliter + CurrentShareName + path;
        }

        public DirectoryInfo GetDir(string path)
        {
            var dir = new DirectoryInfo(GetFullPath(path));
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
            var dir = GetDir(this.CurrentPath);
            this.CurrentFileItems = GetDirItems(dir);
            NotifyPathChanged();
        }

        public void Open(FileItem item)
        {
            if (item.IsFolder)
            {
                this.CurrentPath = this.CurrentPath + PathSpliter + item.Name;
            }
            else
            {
                Process.Start(item.FullPath);
            }
        }

        public void Back()
        {
            if (visitHistory.Count == 0)
            {
                Util.ShowErrorMessage("访问记录为空，无法后退");
                return;
            }
            if (visitIndex == 0)
            {
                Util.ShowErrorMessage("已达到最早访问位置，无法后退");
                return;
            }
            visitIndex--;
            Flush();
        }

        public void Forward()
        {
            if (visitHistory.Count == 0)
            {
                Util.ShowErrorMessage("访问记录为空，无法前进");
                return;
            }
            if (visitIndex >= visitHistory.Count - 1)
            {
                Util.ShowErrorMessage("已达到最晚访问位置，无法前进");
                return;
            }
            visitIndex++;
            Flush();
        }
        public void Up()
        {
            var folders = this.CurrentPath.Split('\\');
            if (folders.Count() == 0)
            {
                Util.ShowErrorMessage("已达到根目录，无法向上");
                return;
            }
            var parentFolders = folders.Where((s, idx) => s != "" && idx != folders.Count() - 1).ToArray();
            Visit(Root + Path.Combine(string.Join(@"\", parentFolders)));
        }

        private void Visit(string path)
        {
            try
            {
                _ = GetDir(path);
            }
            catch (Exception ex)
            {
                // 路径错误，需要将UI的值修正
                NotifyPathChanged();
                Util.ShowErrorMessage(ex.Message);
                return;
            }
            if (visitHistory.Count > 0 && visitIndex != visitHistory.Count - 1)
            {
                visitHistory.RemoveRange(visitIndex + 1, visitHistory.Count - visitIndex - 1);
            }
            visitHistory.Add(path);
            visitIndex = visitHistory.Count - 1;
            Flush();
        }
    }
}

using ProjectManager.Controls;
using ProjectManager.FileManager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManager.Pages
{
    /// <summary>
    /// Interaction logic for ListPage1.xaml
    /// </summary>
    public partial class FileList : UserControl
    {
        public FileList()
        {
            InitializeComponent();

            FlushShares();
        }

        private void FlushShares()
        {
            this.BackwardBtn.IsEnabled = false;
            this.ForwardBtn.IsEnabled = false;
            this.FilePanel.Children.Clear();
            foreach (string shareName in FileWatcher.ShareNames)
            {
                var folder = new FolderBlock { FolderName = shareName, IsShared = true };
                folder.MouseDoubleClick += ShareFolder_MouseDoubleClick;
                this.FilePanel.Children.Add(folder);
            }
        }

        private async void ShareFolder_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is FolderBlock)
            {
                var folder = sender as FolderBlock;
                var shareName = folder.FolderName;
                await this.Flush(() =>
                {
                    FileWatcher.SelectShare(shareName);
                });
                this.BackwardBtn.IsEnabled = true;
            }
        }

        public void ApplyDir(DirectoryInfo dir, UIElementCollection items)
        {
            foreach (var subDir in dir.GetDirectories())
            {
                var folderBlock = new FolderBlock { FolderName = subDir.Name };
                folderBlock.MouseDoubleClick += async (s, e) =>
                {
                    var folder = s as FolderBlock;
                    var folderName = folder.FolderName;
                    await this.Flush(() =>
                    {
                        FileWatcher.Enter(folderName);
                    });
                    this.ForwardBtn.IsEnabled = false;
                };
                items.Add(folderBlock);
            }
            foreach (var file in dir.GetFiles())
            {
                var fileBlock = new FileBlock { FileName = file.Name, FullPath = file.FullName };
                //fileBlock.MouseDoubleClick += TODO;
                items.Add(fileBlock);
            }
        }

        public async Task Flush(Action action)
        {
            this.Progress.IsIndeterminate = true;
            this.Progress.Visibility = Visibility.Visible;

            // 选择了其他share name，重建文件树
            this.FilePanel.Children.Clear();

            var dir = await Task.Run(() =>
            {
                action?.Invoke();
                return FileWatcher.GetCurrentDir();
            });
            ApplyDir(dir, this.FilePanel.Children);

            this.Progress.Visibility = Visibility.Hidden;
            this.Progress.IsIndeterminate = false;
        }

        private async void ButtonBackward_Click(object sender, RoutedEventArgs e)
        {
            if (FileWatcher.Backward())
            {
                await this.Flush(null);
                this.ForwardBtn.IsEnabled = true;
            }
            else
            {
                FlushShares();
            }
        }

        private async void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            if (!FileWatcher.Forward())
            {
                this.ForwardBtn.IsEnabled = false;
            }
            await this.Flush(null);
        }
    }
}

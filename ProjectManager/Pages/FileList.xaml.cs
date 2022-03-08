using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.Controls;
using ProjectManager.FileManager;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
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

            this.DataContext = FileWatcher.Instance;
        }

        private async void ButtonBackward_Click(object sender, RoutedEventArgs e)
        {
            await Do(() => FileWatcher.Instance.Back());
        }

        private async void ButtonForward_Click(object sender, RoutedEventArgs e)
        {
            await Do(() => FileWatcher.Instance.Forward());
        }

        private async void ButtonUp_Click(object sender, RoutedEventArgs e)
        {
            await Do(() => FileWatcher.Instance.Up());
        }

        private async void ButtonFlush_Click(object sender, RoutedEventArgs e)
        {
            await Do(() => FileWatcher.Instance.Flush());
        }

        private async void FileItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is UserControl)
            {
                var item = (FileItem)(sender as UserControl).DataContext;
                await Do(() => FileWatcher.Instance.Open(item));
            }
        }

        private async void RefreshMenu_Click(object sender, RoutedEventArgs e)
        {
            await Do(() => FileWatcher.Instance.Flush());
        }

        private async void ParseMenu_Click(object sender, RoutedEventArgs e)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (!dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            var fileDropList = (string[])dataObject.GetData(DataFormats.FileDrop);

            // 判断是复制还是剪切
            MemoryStream memoryStream = (MemoryStream)dataObject.GetData("Preferred DropEffect", true);
            DragDropEffects dragDropEffects = (DragDropEffects)memoryStream.ReadByte();
            if ((dragDropEffects & DragDropEffects.Move) == DragDropEffects.Move)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Move " + string.Join(", ", fileDropList));
                    var result = FileUtil.Move(fileDropList, FileWatcher.Instance.CurrentFullPath);
                    if (result != 0)
                    {
                        Util.ShowErrorMessage(Util.GetErrorMessage(result));
                        return;
                    }
                    FileWatcher.Instance.Flush();
                });
            }
            else if ((dragDropEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Copy " + string.Join(", ", fileDropList));
                    var result = FileUtil.Copy(fileDropList, FileWatcher.Instance.CurrentFullPath);
                    if (result != 0)
                    {
                        Util.ShowErrorMessage(Util.GetErrorMessage(result));
                        return;
                    }
                    FileWatcher.Instance.Flush();
                });
            }
        }

        private void ContextMenu_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is Control)
            {
                var elem = sender as Control;
                var menu = elem.ContextMenu;
                foreach (MenuItem item in menu.Items)
                {
                    if (item.Header == this.ParseMenuItem.Header)
                    {
                        // 判断是否可以粘贴
                        IDataObject dataObject = Clipboard.GetDataObject();
                        item.IsEnabled = dataObject.GetDataPresent(DataFormats.FileDrop);
                    }
                }
            }
        }

        private void FileItem_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            if (sender is UserControl)
            {
                var elem = sender as UserControl;
                if (this.FilePanel.SelectedItems.Count == 0)
                    return;

                var menu = new ContextMenu { };

                if (this.FilePanel.SelectedItems.Count == 1)
                {
                    var openMenu = new MenuItem { Header = "Open" };
                    openMenu.Click += (s, ea) => FileWatcher.Instance.Open((FileItem)this.FilePanel.SelectedItem);
                    menu.Items.Add(openMenu);
                    menu.Items.Add(new Separator());
                }

                var cutMenu = new MenuItem { Header = "Cut" };
                cutMenu.Click += (s, ea) => CutAllSelectedFileItems();
                menu.Items.Add(cutMenu);

                var copyMenu = new MenuItem { Header = "Copy" };
                copyMenu.Click += (s, ea) => CopyAllSelectedFileItems();
                menu.Items.Add(copyMenu);

                menu.Items.Add(new Separator());

                var deleteMenu = new MenuItem { Header = "Delete" };
                deleteMenu.Click += (s, ea) => DeleteAllSelectedFileItems();
                menu.Items.Add(deleteMenu);
                elem.ContextMenu = menu;

                if (this.FilePanel.SelectedItems.Count == 1)
                {
                    var renameMenu = new MenuItem { Header = "Rename" };
                    renameMenu.Click += (s, ea) => RenameSelectedFileItem();
                    menu.Items.Add(renameMenu);
                }
            }
        }

        private async void CopyAllSelectedFileItems()
        {
            if (this.FilePanel.SelectedItems.Count == 0)
                return;
            var fileItems = this.FilePanel.SelectedItems.Cast<FileItem>();
            var filePaths = from item in fileItems select item.FullPath;
            await Task.Run(() =>
            {
                FileUtil.SetDropFilesIntoClipboard(filePaths.ToArray(), false);
            });
        }

        private async void CutAllSelectedFileItems()
        {
            if (this.FilePanel.SelectedItems.Count == 0)
                return;
            var fileItems = this.FilePanel.SelectedItems.Cast<FileItem>();
            var filePaths = from item in fileItems select item.FullPath;
            await Task.Run(() =>
            {
                FileUtil.SetDropFilesIntoClipboard(filePaths.ToArray(), true);
            });
        }

        private async void DeleteAllSelectedFileItems()
        {
            if (this.FilePanel.SelectedItems.Count == 0)
                return;
            var fileItems = this.FilePanel.SelectedItems.Cast<FileItem>();
            var filePaths = from item in fileItems select item.FullPath;
            await Task.Run(() =>
            {
                Console.WriteLine("Delete " + string.Join(", ", filePaths));
                var result = FileUtil.Delete(filePaths.ToArray());
                if (result != 0)
                {
                    Util.ShowErrorMessage(Util.GetErrorMessage(result));
                    return;
                }
                FileWatcher.Instance.Flush();
            });
        }

        private void RenameSelectedFileItem()
        {
            if (this.FilePanel.SelectedItem == null)
                return;

            var dlg = new RenameFileDialog();
            dlg.ShowDialog();
            if (dlg.DialogResult ?? false)
            {
                var fileItem = (FileItem)this.FilePanel.SelectedItem;
                var result = FileUtil.Rename(fileItem.FullPath, dlg.NewName.Text);
                if (result != 0)
                {
                    Util.ShowErrorMessage(Util.GetErrorMessage(result));
                    return;
                }
                FileWatcher.Instance.Flush();
            }
        }

        private async Task Do(Action action)
        {
            if (!this.FilePanel.IsEnabled)
            {
                // 有其他任务在执行
                return;
            }

            this.FilePanel.IsEnabled = false;
            await Task.Run(action);
            this.FilePanel.IsEnabled = true;
        }
    }
}

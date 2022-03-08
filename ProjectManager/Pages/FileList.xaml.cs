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

            this.PanelBackground.ContextMenu = new ContextMenu { };
            var parseMenu = new MenuItem { Header = "Parse" };
            parseMenu.Click += async (s, e) =>
            {
                await CopyOrMove();
            };
            this.PanelBackground.ContextMenu.Items.Add(parseMenu);
        }

        public async Task CopyOrMove()
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            if (!dataObject.GetDataPresent(DataFormats.FileDrop))
            {
                return;
            }
            var fileDropList = (string[])dataObject.GetData(DataFormats.FileDrop);

            MemoryStream memoryStream = (MemoryStream)dataObject.GetData("Preferred DropEffect", true);
            DragDropEffects dragDropEffects = (DragDropEffects)memoryStream.ReadByte();
            if ((dragDropEffects & DragDropEffects.Move) == DragDropEffects.Move)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Move " + string.Join(", ", fileDropList));
                });
            }
            else if ((dragDropEffects & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                await Task.Run(() =>
                {
                    Console.WriteLine("Copy " + string.Join(", ", fileDropList));
                });
            }
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

        private async void FileItem_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is UserControl)
            {
                var item = (FileItem)(sender as UserControl).DataContext;
                await Do(() => FileWatcher.Instance.Open(item));
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

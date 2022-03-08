using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ProjectManager.Converters
{
    class FileTypeToContextMenuConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var item = (FileItem)value;
            if (item.IsFolder)
            {
                return CreateFolderMenu(item);
            }
            return CreateFileMenu(item);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        private ContextMenu CreateFolderMenu(FileItem item)
        {
            var menu = new ContextMenu { };
            var openMenu = new MenuItem { Header = "Open" };
            openMenu.Click += (s, e) =>
            {
                FileWatcher.Instance.Open(item);
            };
            menu.Items.Add(openMenu);
            return menu;
        }

        private ContextMenu CreateFileMenu(FileItem item)
        {
            var menu = new ContextMenu { };
            var openMenu = new MenuItem { Header = "Open" };
            openMenu.Click += (s, e) =>
            {
                FileWatcher.Instance.Open(item);
            };
            menu.Items.Add(openMenu);

            var copyMenu = new MenuItem { Header = "Copy" };
            copyMenu.Click += (s, e) =>
            {
                Clipboard.SetData(DataFormats.FileDrop, new string[] { item.FullPath });
            };
            menu.Items.Add(copyMenu);

            var deleteMenu = new MenuItem { Header = "Delete" };
            deleteMenu.Click += (s, e) =>
            {
                var dlg = new ModernDialog
                {
                    Title = "Delete",
                    Content = string.Format("Are you sure you want to delete {0}?", item.Name),
                };
                dlg.Buttons = new Button[] { dlg.YesButton, dlg.CancelButton };
                dlg.ShowDialog();
                if (dlg.DialogResult ?? false)
                {
                    File.Delete(item.FullPath);
                }
                FileWatcher.Instance.Flush();
            };
            menu.Items.Add(deleteMenu);
            return menu;
        }
    }
}

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
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
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();

            //this.ShareList.SelectedSourceChanged += ShareList_SelectedSourceChanged;
            //foreach (string shareName in FileWatcher.ShareNames)
            //{
            //    //var group = new LinkGroup
            //    //{
            //    //    DisplayName = shareName,
            //    //};
            //    //this.ShareList.LinkGroups.Add(group);

            //    this.ShareList.Links.Add(new Link
            //    {
            //        DisplayName = shareName,
            //        Source = new Uri(""),
            //    });
            //}
        }

        //private async void ShareList_SelectedSourceChanged(object sender, SourceEventArgs e)
        //{
        //    this.Progress.IsIndeterminate = true;
        //    this.Progress.Visibility = Visibility.Visible;

        //    // 选择了其他share name，重建文件树
        //    this.DirTree.Items.Clear();
        //    var dir = await Task.Run(() =>
        //    {
        //        return FileWatcher.GetRootDir();
        //    });
        //    GenerateTree(dir, this.DirTree);

        //    this.Progress.Visibility = Visibility.Hidden;
        //    this.Progress.IsIndeterminate = false;
        //}

        //public void GenerateTree(DirectoryInfo dir, ItemsControl items)
        //{
        //    foreach (var subDir in dir.GetDirectories())
        //    {
        //        var item = new TreeViewItem { Header = subDir.Name };
        //        GenerateTree(subDir, item);
        //        items.Items.Add(item);
        //    }
        //    foreach (var file in dir.GetFiles())
        //    {
        //        items.Items.Add(new TreeViewItem { Header = file.Name });
        //    }
        //}
    }
}

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

            Setup();
        }

        public async void Setup()
        {
            var dir = await Task.Run(() => {
                FileWatcher.Init();
                return FileWatcher.GetDirectory();
            });
            GenerateTree(dir, this.DirTree);
        }

        public void GenerateTree(DirectoryInfo dir, ItemsControl items)
        {
            foreach (var subDir in dir.GetDirectories())
            {
                var item = new TreeViewItem { Header = subDir.Name };
                GenerateTree(subDir, item);
                items.Items.Add(item);
            }
            foreach (var file in dir.GetFiles())
            {
                items.Items.Add(new TreeViewItem { Header = file.Name });
            }
        }
    }
}

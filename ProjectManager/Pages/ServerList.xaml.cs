using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using System;
using System.Collections.Generic;
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
            var dlg = new Login.Login
            {
                Width = 480,
                Height = 320
            };
            dlg.ShowDialog();

            if (dlg.DialogResult ?? false)
            {
                InitializeComponent();
                Setup();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        public void Setup()
        {
            FileWatcher.Init();
            var dir = FileWatcher.GetDirectory();
            foreach (var subDir in dir.GetDirectories())
            {
                this.DirTree.Items.Add(new TreeViewItem { Header = subDir.FullName });
            }
            foreach (var file in dir.GetFiles())
            {
                this.DirTree.Items.Add(new TreeViewItem { Header = file.FullName });
            }
        }
    }
}

using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using ProjectManager.Pages.Login;
using ProjectManager.Utils;
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

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ModernWindow
    {
        public MainWindow()
        {
            var dlg = new HostWindow
            {
                Width = 360,
                Height = 200
            };
            dlg.ShowDialog();
            if (dlg.DialogResult ?? false)
            {
                if (dlg.AlreadyLogin)
                {
                    InitializeComponent();
                    return;
                }
                var login = new Login
                {
                    Width = 480,
                    Height = 320,
                    HostIP = dlg.TextHostIP.Text,
                    LastLoginUser = dlg.LastLoginUser,
                };
                login.ShowDialog();
                if (login.DialogResult ?? false)
                {
                    InitializeComponent();
                    return;
                }
            }
            Application.Current.Shutdown();
        }
    }
}

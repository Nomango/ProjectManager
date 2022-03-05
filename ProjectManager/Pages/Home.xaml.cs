using FirstFloor.ModernUI.Windows.Controls;
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
            }
            else
            {
                Application.Current.Shutdown();
            }
        }
    }
}

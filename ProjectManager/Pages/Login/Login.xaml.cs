using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ProjectManager.Pages.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : ModernWindow
    {
        public Login()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // select first control on the form
            Keyboard.Focus(this.TextHostIP);
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorMessage.BBCode = "";

            var form = (LoginForm)this.Form.DataContext;
            if (form.Error != null)
            {
                return;
            }
            this.Progress.IsIndeterminate = true;
            this.Progress.Visibility = Visibility.Visible;
            this.Submit.IsEnabled = false;

            string hostIP = this.TextHostIP.Text;
            string userName = this.TextUserName.Text;
            var password = this.TextPassword.SecurePassword;
            var msg = await Task.Run(() =>
            {
                try
                {
                    Connection.Connect(hostIP, userName, password);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
                return null;
            });

            this.Submit.IsEnabled = true;
            this.Progress.Visibility = Visibility.Hidden;
            this.Progress.IsIndeterminate = false;

            if (msg == null)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                msg = string.Join("", msg.Split('\r', '\n'));
                this.ErrorMessage.BBCode = string.Format("[color=#ff4500]{0}[/color]", msg);
            }
        }
    }
}

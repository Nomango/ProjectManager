using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public string HostIP;
        public string LastLoginUser;

        public Login()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
            if (LastLoginUser != null)
            {
                this.UseLastLogin.IsEnabled = true;
                this.UseLastLogin.IsChecked = true;
                this.TextUserName.Text = LastLoginUser;
            }
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // select first control on the form
            Keyboard.Focus(this.TextUserName);
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            this.ErrorMessage.BBCode = "";

            var form = (IDataErrorInfo)this.Form.DataContext;
            if (form.Error != null)
            {
                return;
            }
            this.Progress.IsIndeterminate = true;
            this.Progress.Visibility = Visibility.Visible;
            this.Submit.IsEnabled = false;

            await TryConnect();

            this.Submit.IsEnabled = true;
            this.Progress.Visibility = Visibility.Hidden;
            this.Progress.IsIndeterminate = false;
        }

        private async Task TryConnect()
        {
            string hostIP = this.HostIP;
            string userName = this.TextUserName.Text;
            var password = this.TextPassword.SecurePassword;
            var result = await Task.Run(() =>
            {
                return Connection.Connect(hostIP, userName, password);
            });

            // 连接成功，尝试获取资源列表
            if (result == 0)
            {
                var errMsg = await Task.Run(() =>
                {
                    try
                    {
                        FileWatcher.Init();
                    }
                    catch (Exception ex)
                    {
                        return ex.Message;
                    }
                    return null;
                });
                if (errMsg != null)
                {
                    //Util.ShowErrorMessage(errMsg);
                    this.ErrorMessage.BBCode = string.Format("[color=#ff4500]{0}[/color]", errMsg);
                    return;
                }
                // 获取资源列表成功
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                var msg = string.Format("({0}){1}", result, Util.GetErrorMessage(result));
                msg = string.Join("", msg.Split('\r', '\n'));
                this.ErrorMessage.BBCode = string.Format("[color=#ff4500]{0}[/color]", msg);
            }
        }
    }
}

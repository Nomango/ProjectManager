using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for ServerList.xaml
    /// </summary>
    public partial class HostWindow : ModernWindow
    {
        public bool AlreadyLogin;
        public string LastLoginUser;

        public HostWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            // select first control on the form
            Keyboard.Focus(this.TextHostIP);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var form = (IDataErrorInfo)this.Form.DataContext;
            if (form.Error != null)
            {
                return;
            }
            this.Progress.IsIndeterminate = true;
            this.Progress.Visibility = Visibility.Visible;
            this.Submit.IsEnabled = false;

            // 尝试连接一下ip
            string hostIP = this.TextHostIP.Text;
            var result = await Task.Run(() =>
            {
                // 不知道为什么这段代码获取不到已连接的用户名
                StringBuilder buffer = new StringBuilder(260);
                int length = 260;
                int result2 = Net.WNetGetUser(@"\\" + hostIP, buffer, length);
                if (result2 == 0)
                {
                    this.LastLoginUser = buffer.ToString();
                }
                else
                {
                    var msg = string.Format("({0}){1}", result2, Util.GetErrorMessage(result2));
                    Console.WriteLine(msg);
                }
                return Connection.Connect(hostIP, this.LastLoginUser, null);
            });

            this.Submit.IsEnabled = true;
            this.Progress.Visibility = Visibility.Hidden;
            this.Progress.IsIndeterminate = false;

            if (result == 0)
            {
                this.AlreadyLogin = true;
                this.DialogResult = true;
                this.Close();
            }
            else if (result == 1326 /* username or password is incorrect */)
            {
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

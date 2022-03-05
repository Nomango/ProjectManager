using FirstFloor.ModernUI.Windows.Controls;
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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            var form = (LoginForm)this.Form.DataContext;
            if (form.Error != null)
            {
                return;
            }
            this.ProgressRing.IsActive = true;
            this.Submit.IsEnabled = false;
            Console.WriteLine(this.TextHostIP.Text);
            Console.WriteLine(this.TextUserName.Text);
            Console.WriteLine(this.TextPassword.Password);

            this.DialogResult = true;
            this.Close();
        }
    }
}

using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        App()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                Util.ShowErrorMessage(this.Dispatcher, e.ExceptionObject.ToString());
            };

            Application.Current.DispatcherUnhandledException += (sender, e) =>
            {
                Util.ShowErrorMessage(this.Dispatcher, e.Exception.Message);
                e.Handled = true;
            };
        }

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            Util.ShowErrorMessage(this.Dispatcher, e.Exception.Message);
            e.Handled = true;
        }
    }
}

using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Pages.Login
{
    public class LoginForm
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private string hostIP;
        private string userName;
        private string password;

        public string HostIP
        {
            get { return this.hostIP; }
            set
            {
                if (this.hostIP != value)
                {
                    this.hostIP = value;
                    OnPropertyChanged("HostIP");
                }
            }
        }

        public string UserName
        {
            get { return this.userName; }
            set
            {
                if (this.userName != value)
                {
                    this.userName = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        public string Password
        {
            get { return this.password; }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public string Error
        {
            get { return null; }
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "HostIP")
                {
                    return string.IsNullOrEmpty(this.HostIP) ? "Required value" : null;
                }
                if (columnName == "UserName")
                {
                    return string.IsNullOrEmpty(this.UserName) ? "Required value" : null;
                }
                if (columnName == "Password")
                {
                    return string.IsNullOrEmpty(this.Password) ? "Required value" : null;
                }
                return null;
            }
        }
    }
}

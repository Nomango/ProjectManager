using FirstFloor.ModernUI.Presentation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Pages.Login
{
    public class LoginForm
        : NotifyPropertyChanged, IDataErrorInfo
    {
        private string hostIP = "192.168.18.128";
        private string userName = "";

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

        public string Error
        {
            get;
        }

        public string this[string columnName]
        {
            get
            {
                if (columnName == "HostIP")
                {
                    if (string.IsNullOrEmpty(this.HostIP))
                    {
                        return "Required value";
                    }
                    if (!this.HostIP.Contains("."))
                    {
                        return "Invalid";
                    }
                    if (!IPAddress.TryParse(this.HostIP, out _))
                    {
                        return "Invalid";
                    }
                    return null;
                }
                if (columnName == "UserName")
                {
                    return string.IsNullOrEmpty(this.UserName) ? "Required value" : null;
                }
                return null;
            }
        }
    }
}

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
        private Dictionary<string, string> errors = new Dictionary<string, string>();

        private string hostIP = "192.168.18.130";
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
            get
            {
                return errors.Count > 0 ? "has error" : null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var err = GetError(columnName);
                if (err != null)
                {
                    errors.Add(columnName, err);
                    return err;
                }
                else
                {
                    errors.Remove(columnName);
                    return null;
                }
            }
        }

        private string GetError(string columnName)
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

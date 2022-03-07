using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.FileManager
{
    class Connection
    {
        public static string HostIP;
        public static string UserName;
        public static SecureString Password;

        public static int Connect(string hostIP, string userName, SecureString password)
        {
            var myResource = new Net.NETRESOURCE();
            myResource.dwScope = (int)Net.RESOURCE_SCOPE.RESOURCE_GLOBALNET;
            myResource.dwType = (int)Net.RESOURCE_TYPE.RESOURCETYPE_DISK; //RESOURCETYPE_ANY
            myResource.dwDisplayType = 0;
            myResource.LocalName = "";
            myResource.RemoteName = @"\\" + hostIP;
            myResource.dwUsage = 0;
            myResource.Comment = "";
            myResource.Provider = "";
            var rawPassword = Util.GetSecureStringContent(password);
            int result = Net.WNetAddConnection2(myResource, rawPassword, userName, 0);
            if (result != 0)
            {
                return result;
            }
            HostIP = hostIP;
            UserName = userName;
            if (password != null)
            {
                Password = password.Copy();
                Password.MakeReadOnly();
            }
            return 0;
        }

        public static int Disconnect()
        {
            if (Connection.HostIP == null)
            {
                return 0;
            }
            int result = Net.WNetCancelConnection2(@"\\" + HostIP, 0, false);
            if (result != 0)
            {
                return result;
            }
            Connection.HostIP = "";
            Connection.UserName = "";
            Connection.Password = new SecureString();
            return result;
        }

        public static void Connect2(string hostIP, string userName, string password)
        {
            hostIP = @"\\" + hostIP;
            string cmd = "net use " + hostIP + " " + password + " /user:" + userName;
            Cmd.Exec(cmd);
            Connection.HostIP = hostIP;
        }

        public static void Disconnect2()
        {
            if (Connection.HostIP == null)
            {
                return;
            }
            string cmd = "net use " + Connection.HostIP + " /delete";
            Cmd.Exec(cmd);
            Connection.HostIP = "";
        }
    }
}

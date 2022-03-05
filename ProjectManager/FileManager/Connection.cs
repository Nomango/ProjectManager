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

        public static void Connect(string hostIP, string userName, SecureString password)
        {
            var myResource = new Util.NETRESOURCE();
            myResource.dwScope = 0;
            myResource.dwType = 0; //RESOURCETYPE_ANY
            myResource.dwDisplayType = 0;
            myResource.LocalName = "";
            myResource.RemoteName = @"\\" + hostIP;
            myResource.dwUsage = 0;
            myResource.Comment = "";
            myResource.Provider = "";
            int result = Util.WNetAddConnection2(myResource,
                                                 Util.GetSecureStringContent(Password),
                                                 userName,
                                                 0);
            if (result != 0)
            {
                throw new Exception(string.Format("({0}){1}", result, Util.GetErrorMessage(result)));
            }
            HostIP = hostIP;
            UserName = userName;
            Password = password.Copy();
            Password.MakeReadOnly();
        }

        public static void Disconnect()
        {
            if (Connection.HostIP == null)
            {
                return;
            }
            int result = Util.WNetCancelConnection2(@"\\" + HostIP, 0, false);
            if (result != 0)
            {
                throw new Exception(string.Format("({0}){1}", result, Util.GetErrorMessage(result)));
            }
            Connection.HostIP = "";
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

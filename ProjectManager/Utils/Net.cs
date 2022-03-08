using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Utils
{
    class Net
    {
        public enum RESOURCE_SCOPE
        {
            RESOURCE_CONNECTED = 1,
            RESOURCE_GLOBALNET = 2,
            RESOURCE_REMEMBERED = 3,
            RESOURCE_RECENT = 4,
            RESOURCE_CONTEXT = 5
        }

        public enum RESOURCE_TYPE
        {
            RESOURCETYPE_ANY = 0,
            RESOURCETYPE_DISK = 1,
            RESOURCETYPE_PRINT = 2,
            RESOURCETYPE_RESERVED = 8,
        }

        public enum RESOURCE_USAGE
        {
            RESOURCEUSAGE_CONNECTABLE = 1,
            RESOURCEUSAGE_CONTAINER = 2,
            RESOURCEUSAGE_NOLOCALDEVICE = 4,
            RESOURCEUSAGE_SIBLING = 8,
            RESOURCEUSAGE_ATTACHED = 16,
            RESOURCEUSAGE_ALL = (RESOURCEUSAGE_CONNECTABLE | RESOURCEUSAGE_CONTAINER | RESOURCEUSAGE_ATTACHED),
        }

        public enum RESOURCE_DISPLAYTYPE
        {
            RESOURCEDISPLAYTYPE_GENERIC = 0,
            RESOURCEDISPLAYTYPE_DOMAIN = 1,
            RESOURCEDISPLAYTYPE_SERVER = 2,
            RESOURCEDISPLAYTYPE_SHARE = 3,
            RESOURCEDISPLAYTYPE_FILE = 4,
            RESOURCEDISPLAYTYPE_GROUP = 5,
            RESOURCEDISPLAYTYPE_NETWORK = 6,
            RESOURCEDISPLAYTYPE_ROOT = 7,
            RESOURCEDISPLAYTYPE_SHAREADMIN = 8,
            RESOURCEDISPLAYTYPE_DIRECTORY = 9,
            RESOURCEDISPLAYTYPE_TREE = 10,
            RESOURCEDISPLAYTYPE_NDSCONTAINER = 11
        }

        [StructLayout(LayoutKind.Sequential)]
        public class NETRESOURCE
        {
            public int dwScope;
            public int dwType;
            public int dwDisplayType;
            public int dwUsage;
            public string LocalName;
            public string RemoteName;
            public string Comment;
            public string Provider;
        }

        [DllImport("mpr.dll")]
        public static extern int WNetAddConnection2(NETRESOURCE netResource, string password, string username, int flags);

        [DllImport("mpr.dll")]
        public static extern int WNetCancelConnection2(string server, int flags, bool force);

        [DllImport("mpr.dll")]
        public static extern int WNetGetUser(string server, [Out] StringBuilder userName, [In, Out] int length);

        public static bool TryConnect(string server)
        {
            return true;
            var client = new TcpClient();
            try
            {
                return client.ConnectAsync(server, 5000 /* 群晖的Web端口 */).Wait(500);
            }
            catch (Exception ex)
            {
                Console.WriteLine("尝试连接服务器失败：" + ex.Message);
                return false;
            }
            finally
            {
                client.Close();
            }
        }
    }
}

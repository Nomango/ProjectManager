using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Utils
{
    class Util
    {
        public static string GetSecureStringContent(SecureString s)
        {
            if (s == null)
            {
                return null;
            }
            var p = Marshal.SecureStringToBSTR(s);
            string content = Marshal.PtrToStringBSTR(p);
            return content;
        }

        const int FORMAT_MESSAGE_FROM_SYSTEM = 0x1000;
        const int FORMAT_MESSAGE_IGNORE_INSERTS = 0x200;

        [DllImport("Kernel32.dll")]
        private static extern int FormatMessage(uint dwFlags, IntPtr lpSource, uint dwMessageId, uint dwLanguageId, [Out] StringBuilder lpBuffer, uint nSize, IntPtr arguments);

        public static string GetErrorMessage(int code)
        {
            uint dwFlags = FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS;
            StringBuilder lpBuffer = new StringBuilder(260);
            _ = FormatMessage(dwFlags, IntPtr.Zero, (uint)code, 0, lpBuffer, 260, IntPtr.Zero);
            return lpBuffer.ToString();
        }
    }
}

using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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

            var msg = string.Format("({0}){1}", code, lpBuffer.ToString());
            msg = string.Join("", msg.Split('\r', '\n'));
            return msg;
        }

        public static string GetColoredBBCode(string s, string color)
        {
            return string.Format("[color={0}]{1}[/color]", color, s.Replace("[", "【").Replace("]", "】"));
        }

        public static void ShowErrorMessage(string message)
        {
            //var dlg = new ModernDialog
            //{
            //    Width = 120,
            //    Height = 75,
            //    Title = "Error",
            //    //Content = message,
            //};
            //dlg.Buttons = new Button[] { dlg.OkButton };
            //dlg.ShowDialog();

            ModernDialog.ShowMessage(message, "Error", System.Windows.MessageBoxButton.OK);
        }

        public static BitmapImage GetDefaultFolderIcon()
        {
            return GetIcon("folder.png");
        }

        public static BitmapImage GetDefaultSharedFolderIcon()
        {
            return GetIcon("share_folder.png");
        }

        public static BitmapImage GetIcon(string imageName)
        {
            try
            {
                string source = "/ProjectManager;component/Assets/Images/" + imageName;
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.UriSource = new Uri(source, UriKind.RelativeOrAbsolute);
                bitmap.EndInit();
                return bitmap;
            }
            catch (Exception ex)
            {
                ;
            }
            return null;
        }
    }
}

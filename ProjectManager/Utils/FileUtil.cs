using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjectManager.Utils
{
    public static class FileUtil
    {
        [DllImport("shell32.dll")]
        private static extern int SHFileOperation(ref SHFILEOPSTRUCT lpFileOp);

        public static int Copy(string[] src, string dest)
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = wFunc.FO_COPY;
            lpFileOp.pFrom = string.Join("\0", src) + '\0';
            lpFileOp.pTo = dest + '\0';
            lpFileOp.fFlags = FILEOP_FLAGS.FOF_RENAMEONCOLLISION;
            lpFileOp.fFlags &= ~FILEOP_FLAGS.FOF_ALLOWUNDO;
            lpFileOp.fAnyOperationsAborted = false;
            var result = SHFileOperation(ref lpFileOp);
            return result == 1223 ? 0 : result;
        }

        public static int Move(string[] src, string dest)
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = wFunc.FO_MOVE;
            lpFileOp.pFrom = string.Join("\0", src) + '\0';
            lpFileOp.pTo = dest + '\0';
            lpFileOp.fFlags = FILEOP_FLAGS.FOF_RENAMEONCOLLISION;
            lpFileOp.fFlags &= ~FILEOP_FLAGS.FOF_ALLOWUNDO;
            lpFileOp.fAnyOperationsAborted = false;
            var result = SHFileOperation(ref lpFileOp);
            return result == 1223 ? 0 : result;
        }

        public static int Delete(string[] src)
        {
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = wFunc.FO_DELETE;
            lpFileOp.pFrom = string.Join("\0", src) + '\0';
            lpFileOp.pTo = "\0";
            lpFileOp.fFlags = FILEOP_FLAGS.FOF_WANTNUKEWARNING;
            lpFileOp.fFlags &= ~FILEOP_FLAGS.FOF_ALLOWUNDO;
            lpFileOp.fAnyOperationsAborted = false;
            var result = SHFileOperation(ref lpFileOp);
            return result == 1223 ? 0 : result;
        }

        public static int Rename(string path, string newName)
        {
            newName = Path.Combine(Path.GetDirectoryName(path), newName);
            SHFILEOPSTRUCT lpFileOp = new SHFILEOPSTRUCT();
            lpFileOp.wFunc = wFunc.FO_RENAME;
            lpFileOp.pFrom = path + '\0';
            lpFileOp.pTo = newName + "\0";
            lpFileOp.fFlags = FILEOP_FLAGS.FOF_RENAMEONCOLLISION;
            lpFileOp.fFlags &= ~FILEOP_FLAGS.FOF_ALLOWUNDO;
            lpFileOp.fAnyOperationsAborted = false;
            var result = SHFileOperation(ref lpFileOp);
            return result == 1223 ? 0 : result;
        }

        public static void SetDropFilesIntoClipboard(string[] fileList, bool isCut)
        {
            Application.Current.Dispatcher.BeginInvoke((Action)(() =>
            {
                var data = new DataObject();
                var collection = new StringCollection();
                collection.AddRange(fileList);
                data.SetFileDropList(collection);

                MemoryStream dropEffect = new MemoryStream(!isCut ? new byte[] { 5, 0, 0, 0 } : new byte[] { 2, 0, 0, 0 });
                data.SetData("Preferred DropEffect", dropEffect);
                Clipboard.SetDataObject(data, true);
            }));
        }

        private struct SHFILEOPSTRUCT
        {
            public IntPtr hwnd;
            public wFunc wFunc;
            public string pFrom;
            public string pTo;
            public FILEOP_FLAGS fFlags;
            public bool fAnyOperationsAborted;
            public IntPtr hNameMappings;
            public string lpszProgressTitle;
        }

        private enum wFunc
        {
            FO_MOVE = 0x0001,
            FO_COPY = 0x0002,
            FO_DELETE = 0x0003,
            FO_RENAME = 0x0004
        }

        private enum FILEOP_FLAGS
        {
            FOF_MULTIDESTFILES = 0x0001, //pTo 指定了多个目标文件，而不是单个目录
            FOF_CONFIRMMOUSE = 0x0002,
            FOF_SILENT = 0x0044, // 不显示一个进度对话框
            FOF_RENAMEONCOLLISION = 0x0008, // 碰到有抵触的名字时，自动分配前缀
            FOF_NOCONFIRMATION = 0x10, // 不对用户显示提示
            FOF_WANTMAPPINGHANDLE = 0x0020, // 填充 hNameMappings 字段，必须使用 SHFreeNameMappings 释放
            FOF_ALLOWUNDO = 0x40, // 允许撤销
            FOF_FILESONLY = 0x0080, // 使用 *.* 时, 只对文件操作
            FOF_SIMPLEPROGRESS = 0x0100, // 简单进度条，意味者不显示文件名。
            FOF_NOCONFIRMMKDIR = 0x0200, // 建新目录时不需要用户确定
            FOF_NOERRORUI = 0x0400, // 不显示出错用户界面
            FOF_NOCOPYSECURITYATTRIBS = 0x0800, // 不复制 NT 文件的安全属性
            FOF_NORECURSION = 0x1000, // 不递归目录
            FOF_WANTNUKEWARNING = 0x4000, // 永久删除时弹出警告
        }
    }
}

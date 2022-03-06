using ProjectManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjectManager.Controls
{
    /// <summary>
    /// Interaction logic for FileBlock.xaml
    /// </summary>
    public partial class FileBlock : UserControl
    {
        public string FileName;
        public string FullPath;

        public FileBlock()
        {
            InitializeComponent();
            this.Loaded += FileBlock_Loaded;
        }

        private void FileBlock_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextName.Text = FileName;

            var extension = System.IO.Path.GetExtension(this.FileName);
            if (extension.Equals(".exe"))
            {
                var icon = IconUtil.GetFileIcon(FullPath, false);
                this.Icon.Source = IconUtil.ConvertFromIcon(icon);
            }
            else
            {
                var icon = IconUtil.GetFileIcon(FileName, false);
                this.Icon.Source = IconUtil.ConvertFromIcon(icon);
            }
        }
    }
}

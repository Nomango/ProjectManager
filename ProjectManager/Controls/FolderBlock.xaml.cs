using ProjectManager.Utils;
using System;
using System.Collections.Generic;
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
    public partial class FolderBlock : UserControl
    {
        public string FolderName;
        public bool IsShared;

        public FolderBlock()
        {
            InitializeComponent();
            this.Loaded += FolderBlock_Loaded;
        }

        private void FolderBlock_Loaded(object sender, RoutedEventArgs e)
        {
            this.TextName.Text = FolderName;
            if (IsShared)
            {
                this.Icon.Source = Util.GetDefaultSharedFolderIcon();
            }
            else
            {
                this.Icon.Source = Util.GetDefaultFolderIcon();
            }
        }
    }
}

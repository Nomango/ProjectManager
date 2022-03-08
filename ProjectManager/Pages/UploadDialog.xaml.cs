using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
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

namespace ProjectManager.Pages
{
    public class UploadModel
    {
        public string[] SubDirs
        {
            get
            {
                return ProjectConfig.SubDirs;
            }
        }

        public string selectedDir;
        public string SelectedDir
        {
            get
            {
                if (selectedDir == null)
                {
                    selectedDir = SubDirs.FirstOrDefault();
                }
                return selectedDir;
            }
            set
            {
                selectedDir = value;
            }
        }
    }

    /// <summary>
    /// Interaction logic for ModernDialog1.xaml
    /// </summary>
    public partial class UploadDialog : ModernDialog
    {
        public string ProjectName;

        public UploadDialog(string projectName)
        {
            InitializeComponent();

            // define the dialog buttons
            //this.Buttons = new Button[] { this.OkButton, this.CancelButton };

            this.ProjectName = projectName;
            this.Title = string.Format("Upload to {0}", projectName);
        }

        private void OnDragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effects = DragDropEffects.Link;
            }
            else
            {
                e.Effects = DragDropEffects.None;
            }
        }

        private async void OnDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                return;

            try
            {
                var fileList = (string[])e.Data.GetData(DataFormats.FileDrop);
                var subDir = (string)this.ListSubDir.SelectedItem;
                await Task.Run(() =>
                {
                    var dest = FileWatcher.PathSpliter + ProjectName + FileWatcher.PathSpliter + subDir;
                    var result = FileUtil.Copy(fileList, FileWatcher.Instance.GetFullPath(dest));
                    if (result != 0)
                    {
                        Util.ShowErrorMessage(Util.GetErrorMessage(result));
                    }
                });
            }
            catch (Exception ex)
            {
                Util.ShowErrorMessage(ex.Message);
            }
        }
    }
}

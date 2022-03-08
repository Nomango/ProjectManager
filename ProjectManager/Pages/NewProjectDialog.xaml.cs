using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using ProjectManager.FileManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
    public class CreateProjectModel : NotifyPropertyChanged, IDataErrorInfo
    {
        private string projectName;
        public string ProjectName
        {
            get
            {
                return projectName;
            }
            set
            {
                if (projectName != value)
                {
                    projectName = value;
                    OnPropertyChanged("ProjectName");
                }
            }
        }

        private Dictionary<string, string> errors = new Dictionary<string, string>();
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

        public string GetError(string columnName)
        {
            if (columnName == "ProjectName")
            {
                if (string.IsNullOrEmpty(ProjectName))
                {
                    return "Required value";
                }
                if (!ProjectConfig.CheckProjectName(ProjectName))
                {
                    return "Project exists";
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Interaction logic for ModernDialog1.xaml
    /// </summary>
    public partial class NewProjectDialog : ModernDialog
    {
        public NewProjectDialog()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            this.OkButton.Command = null;
            this.OkButton.Click += (s, e) =>
            {
                var form = (IDataErrorInfo)this.Form.DataContext;
                if (!string.IsNullOrEmpty(form.Error))
                {
                    return;
                }
                this.DialogResult = true;
                this.Close();
            };
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.TextNewName);
        }
    }
}

using FirstFloor.ModernUI.Presentation;
using FirstFloor.ModernUI.Windows.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class NewNameModel : NotifyPropertyChanged, IDataErrorInfo
    {
        private string newName;
        public string NewName
        {
            get
            {
                return newName;
            }
            set
            {
                if (newName != value)
                {
                    newName = value;
                    OnPropertyChanged("NewName");
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
            if (columnName == "NewName")
            {
                return string.IsNullOrEmpty(NewName) ? "Required value" : null;
            }
            return null;
        }
    }

    /// <summary>
    /// Interaction logic for ModernDialog1.xaml
    /// </summary>
    public partial class RenameFileDialog : ModernDialog
    {
        public RenameFileDialog()
        {
            InitializeComponent();

            // define the dialog buttons
            this.Buttons = new Button[] { this.OkButton, this.CancelButton };
            this.Loaded += RenameFileDialog_Loaded;
        }

        private void RenameFileDialog_Loaded(object sender, RoutedEventArgs e)
        {
            Keyboard.Focus(this.TextNewName);
        }
    }
}

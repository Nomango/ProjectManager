using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ProjectManager.Events
{
    public class MouseLeftButtonUpClick
    {
        public static DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
            typeof(ICommand),
            typeof(MouseLeftButtonUpClick),
            new UIPropertyMetadata(CommandChanged));

        public static void SetCommand(DependencyObject target, ICommand value)
        {
            target.SetValue(CommandProperty, value);
        }

        private static void CommandChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            Control control = target as Control;
            if (control != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    control.MouseLeftButtonUp += OnMouseLeftButtonUpClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    control.MouseLeftButtonUp -= OnMouseLeftButtonUpClick;
                }
            }
        }

        private static void OnMouseLeftButtonUpClick(object sender, MouseButtonEventArgs e)
        {
            DataGrid datagrid = sender as DataGrid;
            Point point = e.GetPosition(datagrid);
            IInputElement obj = datagrid.InputHitTest(point);
            DependencyObject target = obj as DependencyObject;

            while (target != null)
            {
                if (target is DataGridRow)
                {
                    ICommand command = (ICommand)datagrid.GetValue(CommandProperty);
                    object commandParameter = datagrid.SelectedItems;
                    if (null != commandParameter)
                    {
                        command.Execute(commandParameter);
                    }
                    break;
                }
                target = VisualTreeHelper.GetParent(target);
            }
        }
    }

}

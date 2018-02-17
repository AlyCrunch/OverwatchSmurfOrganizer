using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace OWOrganizerApp.Views
{
    /// <summary>
    /// Logique d'interaction pour Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main() => InitializeComponent();

        private void OnlyDigit(object sender, KeyEventArgs e)
        {
            var inputChar = Helpers.Converters.KeysConverter.GetCharFromKey(e.Key);
            e.Handled = !char.IsDigit(inputChar);
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ItemContainerGenerator generator = AccountsListView.ItemContainerGenerator;
            ListBoxItem selectedItem = (ListBoxItem)generator.ContainerFromIndex(AccountsListView.SelectedIndex);
            Button openButton = GetDescendantByType(selectedItem, typeof(Button), "openAccount") as Button;
            openButton.Command?.Execute(openButton.CommandParameter);
        }

        private void AccountDialog_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if ((e.OriginalSource is Grid) && ((Grid)e.OriginalSource).Name == "AccountDialog")
                CloseDialogueHyperlink.Command?.Execute(CloseDialogueHyperlink.CommandParameter);
        }

        public static Visual GetDescendantByType(Visual element, Type type, string name)
        {
            if (element == null) return null;
            if (element.GetType() == type)
            {
                FrameworkElement fe = element as FrameworkElement;
                if (fe != null)
                {
                    if (fe.Name == name)
                    {
                        return fe;
                    }
                }
            }
            Visual foundElement = null;
            if (element is FrameworkElement)
                (element as FrameworkElement).ApplyTemplate();
            for (int i = 0;
                i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                Visual visual = VisualTreeHelper.GetChild(element, i) as Visual;
                foundElement = GetDescendantByType(visual, type, name);
                if (foundElement != null)
                    break;
            }
            return foundElement;
        }
    }
}

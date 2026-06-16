using System.Windows;
using wpfBudgetSys.Helpers;

namespace wpfBudgetSys.View
{
    public partial class AppDialogWindow : Window
    {
        public AppDialogWindow(string title, string message, AppDialogIcon icon, bool showCancel = false)
        {
            InitializeComponent();
            TitleText.Text = title;
            MessageText.Text = message;

            CancelButton.Visibility = showCancel ? Visibility.Visible : Visibility.Collapsed;

            (IconText.Text, IconBadge.Background) = icon switch
            {
                AppDialogIcon.Success => ("✓", new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DDEC00")!)),
                AppDialogIcon.Warning => ("!", new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F59E0B")!)),
                AppDialogIcon.Error => ("×", new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#FCA5A5")!)),
                _ => ("i", new System.Windows.Media.SolidColorBrush(
                    (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#DDEC00")!))
            };
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

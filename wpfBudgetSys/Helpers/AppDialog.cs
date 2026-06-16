using System.Windows;
using wpfBudgetSys.View;

namespace wpfBudgetSys.Helpers
{
    public enum AppDialogIcon
    {
        Info,
        Success,
        Warning,
        Error
    }

    public static class AppDialog
    {
        public static void Show(string message, string title = "Bawal Gastos", AppDialogIcon icon = AppDialogIcon.Info)
        {
            var dialog = new AppDialogWindow(title, message, icon);
            dialog.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                           ?? Application.Current.MainWindow;
            dialog.ShowDialog();
        }

        public static bool Confirm(string message, string title = "Confirm")
        {
            var dialog = new AppDialogWindow(title, message, AppDialogIcon.Warning, showCancel: true);
            dialog.Owner = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.IsActive)
                           ?? Application.Current.MainWindow;
            return dialog.ShowDialog() == true;
        }
    }
}

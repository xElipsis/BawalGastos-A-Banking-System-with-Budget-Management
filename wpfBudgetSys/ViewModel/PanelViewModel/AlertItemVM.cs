using System.Windows.Media;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class AlertItemVM : ViewModelBase
    {
        public AlertItemVM(Alert alert)
        {
            Alert = alert;
            var colors = AlertThresholdHelper.GetColors(alert.AlertType);
            BorderBrush = colors.Border;
            BackgroundBrush = colors.Background;
            BadgeForeground = colors.BadgeForeground;
        }

        public Alert Alert { get; }

        public int AlertId => Alert.AlertId;
        public string AlertType => Alert.AlertType;
        public string? CategoryName => Alert.CategoryName;
        public string Message => Alert.Message;
        public DateTime TriggeredAt => Alert.TriggeredAt;

        public bool IsRead
        {
            get => Alert.IsRead;
            set
            {
                if (Alert.IsRead == value) return;
                Alert.IsRead = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowMarkReadButton));
            }
        }

        public bool ShowMarkReadButton => !IsRead;

        public Brush BorderBrush { get; }
        public Brush BackgroundBrush { get; }
        public Brush BadgeForeground { get; }
    }
}

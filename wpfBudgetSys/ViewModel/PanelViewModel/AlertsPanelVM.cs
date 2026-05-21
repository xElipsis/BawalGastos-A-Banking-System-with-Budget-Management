using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class AlertsPanelVM : ViewModelBase
    {
        private readonly AlertService alertService = new();

        public ObservableCollection<AlertItemVM> Alerts { get; }

        public bool HasAlerts => Alerts.Count > 0;

        public string EmptyMessage =>
            "No budget alerts yet. Alerts appear when payments reach 50%, 75%, 90%, or 100% of a category's monthly limit.";

        public ICommand MarkReadCommand { get; }
        public ICommand MarkAllAsReadCommand { get; }

        public AlertsPanelVM()
        {
            Alerts = new ObservableCollection<AlertItemVM>();
            LoadAlerts();

            MarkReadCommand = new RelayCommand(o =>
            {
                if (o is not AlertItemVM item)
                    return;

                alertService.MarkAsRead(item.AlertId);
                item.IsRead = true;
            });

            MarkAllAsReadCommand = new RelayCommand(_ =>
            {
                alertService.MarkAllAsRead();
                foreach (var item in Alerts)
                    item.IsRead = true;
            });
        }

        private void LoadAlerts()
        {
            Alerts.Clear();
            foreach (var alert in alertService.GetAlertsForCurrentUser())
                Alerts.Add(new AlertItemVM(alert));

            OnPropertyChanged(nameof(HasAlerts));
        }
    }
}

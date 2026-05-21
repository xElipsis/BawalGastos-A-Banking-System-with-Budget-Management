using System.Windows.Input;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class SettingsPanelVM : ViewModelBase
    {
        private object? subView;
        public object? SubView
        {
            get => subView;
            set
            {
                subView = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(IsHub));
            }
        }

        public bool IsHub => SubView == null;

        public ICommand ShowAccountSettingsCommand { get; }
        public ICommand ShowAppSettingsCommand { get; }
        public ICommand BackToHubCommand { get; }

        public SettingsPanelVM()
        {
            ShowAccountSettingsCommand = new RelayCommand(_ =>
                SubView = new AccountSettingsPanelVM(BackToHubCommand));

            ShowAppSettingsCommand = new RelayCommand(_ =>
                SubView = new AppSettingsPanelVM(BackToHubCommand));

            BackToHubCommand = new RelayCommand(_ => SubView = null);
        }
    }
}

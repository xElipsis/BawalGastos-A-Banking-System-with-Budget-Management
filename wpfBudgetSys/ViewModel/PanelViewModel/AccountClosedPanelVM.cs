using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class AccountClosedPanelVM : ViewModelBase
    {
        public string AccountNumber { get; }
        public string Message { get; }

        public AccountClosedPanelVM()
        {
            var account = new AccountServices().GetAccountForCurrentUser();
            AccountNumber = account?.AccountNumber ?? "—";
            Message =
                "This bank account has been closed by an administrator. Your login still works so you can read notifications and account messages, but you cannot deposit, withdraw, transfer, or pay.\n\n" +
                "You do not need to register a new user account. Contact support or wait for an administrator to reactivate this account.";
        }
    }
}

using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class AccountServices
    {
        private readonly AccountRepository accountRepository = new();
        private readonly SpendingLimitsRepository spendingLimitsRepository = new();
        private readonly UserRepository userRepository = new();

        public Account? GetAccountForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return null;

            return accountRepository.GetByUserId(SessionManager.CurrentUser.UserId);
        }

        public bool CurrentUserHasAccount() => GetAccountForCurrentUser() != null;

        public bool CurrentUserHasActiveAccount()
        {
            var account = GetAccountForCurrentUser();
            return account != null && account.Status == "Active";
        }

        public bool CurrentUserAccountIsClosed() =>
            GetAccountForCurrentUser()?.Status == "Closed";

        public Account? GetAccountByAccountNumber(string accountNumber)
        {

            if (SessionManager.CurrentUser == null)
                return null;

            return accountRepository.GetByAccountNumber(accountNumber);
        }

        public int GetBudgetCategoryCountForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return 0;

            return spendingLimitsRepository.CountByUserId(SessionManager.CurrentUser.UserId);
        }

        public string GetEmailByUserId(int userId)
        {
            return accountRepository.GetEmailByUserId(userId);
        }
    }
}

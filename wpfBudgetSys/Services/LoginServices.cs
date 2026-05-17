using wpfBudgetSys.Model;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class LoginServices
    {
        private readonly LoginRepository loginRepository = new LoginRepository();

        public void RegisterLogin(int user_id, string username, string password)
        {
            Login login = new()
            {
                Username = username,
            };

            loginRepository.InsertLogin(user_id, login, password);
        }
    }
}

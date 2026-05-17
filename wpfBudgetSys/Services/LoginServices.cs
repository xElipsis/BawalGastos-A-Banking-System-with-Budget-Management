using System.Diagnostics;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class LoginServices
    {
        private readonly LoginRepository loginRepository = new LoginRepository();

        public void RegisterLogin(int user_id, string username, string password)
        {
            string hashedPassword = PasswordHelper.Hash(password);
            Debug.Write($"Hashed Password: {hashedPassword}");

            Login login = new()
            {
                UserId = user_id,
                Username = username,
            };

            loginRepository.InsertLogin(login, hashedPassword);
        }
    }
}

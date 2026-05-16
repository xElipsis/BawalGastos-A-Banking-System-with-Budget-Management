using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Model;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class LoginServices
    {
        private readonly LoginRepository loginRepository = new LoginRepository();

        public void RegisterLogin(int user_id, string username, string password)
        {
            Login login = new Login(
                username);
            loginRepository.InsertLogin(user_id, login, password);
        }
    }
}

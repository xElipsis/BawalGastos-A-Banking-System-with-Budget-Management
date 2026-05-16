using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Services
{
    internal class AuthServices
    {
        private readonly UserServices userServices = new UserServices();
        private readonly LoginServices loginServices = new LoginServices();

        public void Register(string fullName, string email, string phone, string username, string password)
        {
            int newUserId = userServices.RegisterUser(fullName, email, phone, "Active");
            loginServices.RegisterLogin(newUserId, username, password);
        }
    }
}

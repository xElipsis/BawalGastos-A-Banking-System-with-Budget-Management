using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Enums;
using wpfBudgetSys.Model;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    class UserServices
    {
        private readonly UserRepository userRepo = new UserRepository();

        public int RegisterUser (string fullname, string email, string phone, string status)
        {
            User user = new User(
                AppEnums.UserRole.User,
                fullname,
                email,
                phone,
                status);
            return userRepo.InsertUser(user);
            
        }
    }
}

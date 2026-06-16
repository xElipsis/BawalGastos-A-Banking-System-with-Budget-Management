using wpfBudgetSys.Enums;
using wpfBudgetSys.Model;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class UserServices
    {
        private readonly UserRepository userRepo = new UserRepository();

        public int RegisterUser (string fullname, string email, string phone, string status)
        {
            User user = new()
            {
                Role = AppEnums.UserRole.User,
                Fullname = fullname,
                Email = email,
                Phone = phone,
                Status = status
            };
                
            return userRepo.InsertUser(user);
        }
    }
}

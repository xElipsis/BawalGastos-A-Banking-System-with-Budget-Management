using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.MVVM
{
    public static class SessionManager
    {
        public static User? CurrentUser { get; set; }
        public static Login? CurrentLogin { get; set; }

        public static bool IsLoggedIn => CurrentUser != null;

        public static void Login(User user, Login login)
        {
            CurrentUser = user;
            CurrentLogin = login;
        }

        public static void Logout()
        {
            CurrentUser = null;
            CurrentLogin = null;
        }
    }
}

using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Enums;

namespace wpfBudgetSys.Model
{

    class User
    {
        private AppEnums.UserRole role;
        private string fullname;
        private string email;
        private string phone;
        private string status;

        public User(AppEnums.UserRole role, string fullname, string email, string phone, string status)
        {
            Role = role;
            Fullname = fullname;
            Email = email;
            Phone = phone;
            Status = status;
        }

        public AppEnums.UserRole Role { get => role; set => role = value; }
        public string Fullname { get => fullname; set => fullname = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        public string Status { get => status; set => status = value; }
    }
}

using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Enums;

namespace wpfBudgetSys.Model
{
    public class User
    {
        public int UserId { get; set; }
        public AppEnums.UserRole Role { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Status { get; set; }
    }
}

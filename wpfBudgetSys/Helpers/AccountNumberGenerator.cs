using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Helpers
{
    public static class AccountNumberGenerator
    {
        public static string Generate()
        {
            // Format: ACC-YYYYMMDD-XXXXX (random 5 digit number)
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(10000, 99999).ToString();
            return $"ACC-{date}-{random}";
        }
    }
}

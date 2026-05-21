using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Helpers
{
    public static class OtpGenerator
    {
        public static string Generate()
        {
            // Generates a random 6 digit code
            return new Random().Next(100000, 999999).ToString();
        }
    }
}

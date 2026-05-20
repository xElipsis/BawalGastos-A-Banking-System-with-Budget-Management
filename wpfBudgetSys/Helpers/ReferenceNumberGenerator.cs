using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Helpers
{
    public static class ReferenceNumberGenerator
    {
        public static string Generate()
        {
            string date = DateTime.Now.ToString("yyyyMMdd");
            string random = new Random().Next(100000, 999999).ToString();
            return $"TXN-{date}-{random}";
        }
    }
}

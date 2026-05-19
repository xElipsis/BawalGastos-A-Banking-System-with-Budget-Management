using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfBudgetSys.Model
{
    public class SpendingLimits
    {
        public int LimitId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public double MonthlyLimit { get; set; }
        public double DailyLimit { get; set; }
    }
}

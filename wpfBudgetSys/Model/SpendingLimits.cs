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
        public decimal MonthlyLimit { get; set; }
        public decimal DailyLimit { get; set; }
    }
}

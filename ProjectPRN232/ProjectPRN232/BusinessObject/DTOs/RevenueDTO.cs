using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTOs
{
    public class RevenueDTO
    {
        public class MonthlyRevenueItemDTO
        {
            public int Month { get; set; }
            public int? TotalMonthlyRevenue { get; set; }
        }
        public class YearlyRevenueDTO
        {
            public int Year { get; set; }
            public int? TotalYearlyRevenue { get; set; }
            public List<MonthlyRevenueItemDTO> MonthlyBreakdown { get; set; } = new List<MonthlyRevenueItemDTO>();
        }
    }
}

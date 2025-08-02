namespace FE.Models
{
    public class RevenueViewModel
    {
        // Dùng để hứng dữ liệu cho từng tháng
        public class MonthlyRevenueItemDTO
        {
            public int Month { get; set; }
            public int? TotalMonthlyRevenue { get; set; }
        }

        // Dùng để hứng dữ liệu cho cả năm
        public class YearlyRevenueDTO
        {
            public int Year { get; set; }
            public int? TotalYearlyRevenue { get; set; }
            public List<MonthlyRevenueItemDTO> MonthlyBreakdown { get; set; } = new();
        }
    }
}

using SalonTrackApi.Entities;

namespace SalonTrackApi.Results
{
    public class PaginatedIncomeResult
    {
        public List<Income> Incomes { get; set; } = new();
        public decimal Total { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal TodayTotal { get; set; }
        public decimal ThisWeekTotal { get; set; }
        public decimal ThisMonthTotal { get; set; }
        public decimal ThisYearTotal { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public List<User> AllUsers { get; set; } = new();

    }
}

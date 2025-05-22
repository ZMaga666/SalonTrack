using SalonTrack.Models;

namespace SalonTrack.ViewModels
{
    public class IncomeListViewModel
    {
        public List<Income> Incomes { get; set; }
        public decimal Total { get; set; }

        public decimal TodayTotal { get; set; }
        public decimal ThisWeekTotal { get; set; }
        public decimal ThisMonthTotal { get; set; }
        public decimal ThisYearTotal { get; set; }

        public decimal TotalExpense { get; set; }      // ✅ Əlavə edildi
        public decimal NetTotal => Total - TotalExpense; // ✅ Yekun gəlir
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? SelectedUsername { get; set; }
        public List<string>? AllUsernames { get; set; }
    }
}

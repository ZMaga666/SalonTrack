using System;
using System.Collections.Generic;
using SalonTrack.Models;

namespace SalonTrack.ViewModels
{
    public class IncomeListViewModel
    {
        public List<Income> Incomes { get; set; }
        public decimal Total { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal NetTotal => Total - TotalExpense;

        public decimal TodayTotal { get; set; }
        public decimal ThisWeekTotal { get; set; }
        public decimal ThisMonthTotal { get; set; }
        public decimal ThisYearTotal { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? SelectedUserId { get; set; }

        public List<ApplicationUser> AllUsers { get; set; }  // ← buraya diqqət!
    }
}

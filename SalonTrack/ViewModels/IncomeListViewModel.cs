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

        public List<ApplicationUser> AllUsers { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; }
        
        public bool ShowDeactivated { get; set; }
    }
}

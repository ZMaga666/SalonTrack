namespace SalonTrack.Models;

public class ServiceTask
{
    public int Id { get; set; }
    //public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    //public decimal Price { get; set; }
    public bool IsCredit { get; set; }

    public int? ServiceId { get; set; }
    public Service? Service { get; set; } // Naviqasiya
    public Income? Income { get; set; }
    public int? IncomeId { get; set; }
}

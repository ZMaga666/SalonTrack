namespace SalonTrack.Models
{
    public class Income
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string? Username { get; set; }
    }
}

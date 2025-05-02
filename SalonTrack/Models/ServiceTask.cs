namespace SalonTrack.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsCredit { get; set; }  // true = nisyə, false = nağd
    }
}

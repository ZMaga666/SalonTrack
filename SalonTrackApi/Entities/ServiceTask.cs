using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SalonTrackApi.Entities
{
    public class ServiceTask
    {
        public int Id { get; set; }
        [Required]
        public decimal Price { get; set; }

        public bool IsCredit { get; set; }

        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        [Required]
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? IncomeId { get; set; }
        public Income? Income { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; } = string.Empty;
    }
}

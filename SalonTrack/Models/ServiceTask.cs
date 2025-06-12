using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonTrack.Models
{
    public class ServiceTask
    {
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; } = DateTime.Now;

        
        public string Description { get; set; } = string.Empty;

        [Required]
        public decimal Price { get; set; }

        public bool IsCredit { get; set; }

        // 👤 İstifadəçi ilə əlaqə
        [Required]
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

        // 🔧 Service ilə əlaqə
        [Required]
        public int? ServiceId { get; set; }
        public Service? Service { get; set; }

        // 💰 Income ilə əlaqə (bir istiqamətli)
        public int? IncomeId { get; set; }
        public Income? Income { get; set; }
    }
}

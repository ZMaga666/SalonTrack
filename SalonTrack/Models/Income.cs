using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SalonTrack.Models
{
    public class Income
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Məbləğ daxil edilməlidir.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Məbləğ 0-dan böyük olmalıdır.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Tarix tələb olunur.")]
        public DateTime Date { get; set; }

        public string? Username { get; set; }

        // 👇 Əlavə olunur
        public string? UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }
    }
}

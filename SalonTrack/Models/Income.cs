using System.ComponentModel.DataAnnotations;

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

      //  [Required(ErrorMessage = "İstifadəçi adı tələb olunur.")]
        public string? Username { get; set; }
    }

}

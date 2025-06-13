using System.ComponentModel.DataAnnotations;

namespace SalonTrack.ViewModels
{
    public class UserEditViewModel
    {

        public string Id { get; set; }

        [Required(ErrorMessage = "İstifadəçi adı tələb olunur.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Rol seçilməlidir.")]
        public string Role { get; set; }
        public bool IsDeleted { get; set; }
    }
}

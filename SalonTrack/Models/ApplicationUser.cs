using Microsoft.AspNetCore.Identity;

namespace SalonTrack.Models
{
    public class ApplicationUser:IdentityUser
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

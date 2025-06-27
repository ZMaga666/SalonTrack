using Microsoft.AspNetCore.Identity;

namespace SalonTrackApi.Entities

{
    public class User:IdentityUser
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

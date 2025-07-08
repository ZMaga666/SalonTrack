namespace SalonTrackApi.DTO
{
    public class UserEditDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public bool IsDeleted { get; set; }
    }
}

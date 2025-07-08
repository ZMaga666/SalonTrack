namespace SalonTrackApi.DTO
{
    public class UserListDto
    {
        public string Id { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string Role { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public decimal TotalIncome { get; set; }
        public bool IsDeleted { get; set; }
    }
}


namespace ASP_Dot_Net_MVC_CRUD_Template.Models
{
    public class UserProfileDto
    {
        public string Email { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? BirthDate { get; set; }
    }
}

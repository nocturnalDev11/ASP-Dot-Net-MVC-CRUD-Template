using System.ComponentModel.DataAnnotations;

namespace ASP_Dot_Net_MVC_CRUD_Template.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? PasswordHash { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public ICollection<Item> Items { get; set; } = new List<Item>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace users_api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();
        [Required]
        public string Password { get; set; } = string.Empty;
        [Required]
        public string Email { get; set; } = string.Empty;
        public ICollection<Note> Notes { get; set; } = new List<Note>();

    }
}

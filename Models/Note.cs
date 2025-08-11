using System.ComponentModel.DataAnnotations;

namespace users_api.Models
{
    public class Note
    {
        [Key]
        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        [Required]
        public User? User { get; set; }
    }
}

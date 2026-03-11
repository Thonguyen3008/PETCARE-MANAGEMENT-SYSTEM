using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace PetCareSystem.API.Models
{
    public class UserAD
    {
        [Key]
        public int UserID { get; set; }

        public string?FullName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Username { get; set; }

        [Required]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        [MaxLength(50)]
        public string Role { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
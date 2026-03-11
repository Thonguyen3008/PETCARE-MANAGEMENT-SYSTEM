using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareSystem.API.Models
{
    public class Customers
    {
        [Key]
        public int CustomerID { get; set; }

        [Required]
        [MaxLength(100)]
        public string CustomerName { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Email { get; set; }

        
        [MaxLength(255)]
        public string? Password { get; set; }

        public string? Address { get; set; }

        [Required]
        public int PhoneNumber { get; set; }
        public virtual ICollection<Pets>? Pets { get; set; } //lien ket den bang Pets
    }
}
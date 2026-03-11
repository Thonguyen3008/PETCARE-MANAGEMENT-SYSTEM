using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareSystem.API.Models
{
    public class Pets
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PetID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customers? Owner { get; set; } //lien ket den bang Customers

        [Required]
        [MaxLength(100)]
        public string PetName { get; set; }

        
        [MaxLength(50)]
        public string Species { get; set; }

        
        [MaxLength(100)]
        public string Breed { get; set; }

       
        [MaxLength(10)]
        public string? Gender { get; set; }

        public int Age { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDate { get; set; }

        [Column(TypeName = "decimal(5,2)")] //decimal co 5 chu so va 2 chu so thap phan
        public decimal? Weight { get; set; }

        public virtual ICollection<MedicalRecords>? MedicalRecords { get; set; } //lien ket den bang MedicalRecords

        public virtual ICollection<Appointments>? Appointments { get; set; } //lien ket den bang Appointments
    }
}
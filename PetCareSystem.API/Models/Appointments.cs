using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareSystem.API.Models
{
    public class Appointments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentID { get; set; }

        [Required]
        public int PetID { get; set; }

        [ForeignKey("PetID")]
        public virtual Pets? Pet { get; set; } //lien ket den bang Pets

        [Required]
        public int StaffID { get; set; }
        [ForeignKey("StaffId")]
        public virtual UserAD? Staff { get; set; } //lien ket den bang UserAD

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime AppointmentDate { get; set; }//datetime de luu tru ngay gio

        [MaxLength(100)]
        public string? ServiceType { get; set; }

        [MaxLength(255)]
        public string? Notes { get; set; }

        [MaxLength(20)]
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Canceled

        [MaxLength(255)]
        public string? CancellationReason { get; set; }

    }
}
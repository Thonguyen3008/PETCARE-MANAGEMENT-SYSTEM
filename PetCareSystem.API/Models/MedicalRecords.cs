using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareSystem.API.Models
{
    public class MedicalRecords
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MedicalRecordId { get; set; }

        [Required]
        public int PetId { get; set; }
        [ForeignKey("PetId")]
        public virtual Pets? Pet { get; set; } //lien ket den bang Pets

        [Required]
        public int VetId { get; set; }
        [ForeignKey("VetId")]
        public virtual UserAD? Vet { get; set; } //lien ket den bang UserAD

        public string? Diagnosis { get; set; }

        public string? Treatment { get; set; }

        [Required]
        public DateTime VisitDate { get; set; } = DateTime.Now; //datetime de luu tru ngay gio

        public virtual ICollection<Inventory>? Inventory { get; set; } //lien ket den bang Inventory
        public virtual ICollection<Appointments>? Appointments { get; set; } //lien ket den bang Appointments
    }
}
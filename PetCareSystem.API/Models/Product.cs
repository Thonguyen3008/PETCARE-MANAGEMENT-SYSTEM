using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareSystem.API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(50)]
        public string SKU { get; set; } //SKU de quan ly san pham, co the la ma so duy nhat de xac dinh san pham trong kho]

        [Required]
        [MaxLength(100)]
        public string ProductName { get; set; }

        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,0)")] //decimal co 10 chu so va 2 chu so thap phan
        public decimal SellPrice { get; set; } //decimal de luu tru gia ca, co the co 2 chu so thap phan

        public virtual ICollection<Inventory>? Inventory { get; set; } //lien ket den bang Inventory

        // san pham se luu vao bang Inventory, va moi lan co san pham moi thi se tao mot batch moi trong bang Inventory de quan ly so luong va thong tin lien quan den san pham do
    }

}
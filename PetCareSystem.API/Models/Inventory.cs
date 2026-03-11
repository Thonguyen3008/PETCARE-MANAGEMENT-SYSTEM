using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetCareSystem.API.Models
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BatchID { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product? Product { get; set; } //hang nhap vao kho lien ket den bang Product

        [Required]
        public string? BatchName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LotNumber { get; set; }

        [Required]
        [Column(TypeName = "datetime")]
        public DateTime ExpiryDate { get; set; } = DateTime.Now; //datetime de luu tru ngay gio

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Quantity must be a non-negative integer.")]
        // cơ chế hoạt động ASP.NET CORE sẽ tự động kiểm tra giá trị của trường Quantity khi dữ liệu được gửi đến API. Nếu giá trị của Quantity là âm, hệ thống sẽ trả về lỗi với thông báo "Quantity must be a non-negative integer."
        public int Quantity { get; set; }
        public string? Status { get; set; } = "Reported"; // Reported, Resolved


    }
}
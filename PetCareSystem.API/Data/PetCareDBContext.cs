using Microsoft.EntityFrameworkCore;
using PetCareSystem.API.Models;

namespace PetCareSystem.API.Data
{
    public class PetCareDBContext : DbContext
    {
        public PetCareDBContext(DbContextOptions<PetCareDBContext> options) : base(options)
        {
        }

        public DbSet<UserAD> UserADs { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<Pets> Pets { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<MedicalRecords> MedicalRecords { get; set; }
        public DbSet<Inventory> Inventory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình quan hệ giữa Pets và Customers
            modelBuilder.Entity<Pets>() // Dùng tên class số ít (Pet)
            .HasOne(p => p.Owner) // Một bé thú cưng thuộc về MỘT chủ nuôi (Owner)
            .WithMany(c => c.Pets) // Một chủ nuôi có thể có NHIỀU thú cưng (Pets)
            .HasForeignKey(p => p.CustomerID) // Khóa ngoại trỏ đến CustomerID trong bảng Customers
            .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa MedicalRecords và Pets
            modelBuilder.Entity<MedicalRecords>()
                .HasOne(m => m.Pet) // Một bản ghi y tế thuộc về MỘT bé thú cưng (Pet)
                .WithMany(p => p.MedicalRecords) // Một bé thú cưng có thể có NHIỀU bản ghi y tế (MedicalRecords)
                .HasForeignKey(m => m.PetId) // Khóa ngoại trỏ đến PetId trong bảng Pets
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa MedicalRecords và UserAD (Vet)
            modelBuilder.Entity<MedicalRecords>()
                .HasOne(m => m.Vet) // Một bản ghi y tế được tạo bởi MỘT bác sĩ thú y (Vet)
                .WithMany()
                .HasForeignKey(m => m.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            //Cấu hình quan hệ giữa Inventory và Products
            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product) // Một mục tồn kho có thể liên quan đến MỘT sản phẩm (Product)
                .WithMany(p => p.Inventory) // Một sản phẩm có thể liên quan đến NHIỀU mục tồn kho (Inventory)
                .HasForeignKey(i => i.ProductId) // Khóa ngoại trỏ đến ProductId trong bảng Products
                .OnDelete(DeleteBehavior.SetNull); // Nếu một sản phẩm bị xóa, các mục tồn kho liên quan sẽ không bị xóa mà chỉ đặt ProductId thành null

             modelBuilder.Entity<UserAD>()
                .HasData(
                    new UserAD { UserID = 1, Username = "admin", Password = "admin123", Role = "Admin" },
                    new UserAD { UserID = 2, Username = "vet1", Password = "vet1", Role = "Vet" },
                    new UserAD { UserID = 3, Username = "vet2", Password = "vet2", Role = "Vet" },
                    new UserAD { UserID = 4, Username = "staff1", Password = "staff1", Role = "Staff" }
                );
            modelBuilder.Entity<Customers>()
                .HasData(
                    new Customers { CustomerID = 1, CustomerName = "John Doe", Email = "john.doe@example.com", PhoneNumber = 1234567890 },
                    new Customers { CustomerID = 2, CustomerName = "Jane Smith", Email = "jane.smith@example.com", PhoneNumber = 0987654321 },
                    new Customers { CustomerID = 3, CustomerName = "Alice Johnson", Email = "alice.johnson@example.com", PhoneNumber = 1112223333 }
                );
            modelBuilder.Entity<Pets>()
                .HasData(
                    new Pets { PetID = 1, PetName = "Buddy", Species = "Dog", Breed = "Golden Retriever", Age = 3, CustomerID = 1 },
                    new Pets { PetID = 2, PetName = "Whiskers", Species = "Cat", Breed = "Maine Coon", Age = 2, CustomerID = 2 },
                    new Pets { PetID = 3, PetName = "Nemo", Species = "Fish", Breed = "Clownfish", Age = 1, CustomerID = 3 }
                );
            modelBuilder.Entity<Appointments>()
                .HasData(
                    new Appointments { AppointmentID = 1, PetID = 1, StaffID = 2, AppointmentDate = DateTime.Now.AddDays(1), Notes= "Annual checkup" },
                    new Appointments { AppointmentID = 2, PetID = 2, StaffID = 3, AppointmentDate = DateTime.Now.AddDays(2), Notes = "Vaccination" }
                );
        }
    }
}
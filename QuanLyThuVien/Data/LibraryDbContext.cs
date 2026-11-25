using System;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<BorrowSlip> BorrowSlips { get; set; }
        public DbSet<BorrowDetail> BorrowDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Employee>().HasIndex(e => e.EmployeeCode).IsUnique();
            modelBuilder.Entity<Employee>().HasIndex(e => e.Email).IsUnique();
            modelBuilder.Entity<Reader>().HasIndex(r => r.ReaderCode).IsUnique();
            modelBuilder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();
            modelBuilder.Entity<BorrowSlip>().HasIndex(bs => bs.SlipCode).IsUnique();

            modelBuilder.Entity<Book>()
                .HasOne(b => b.Category).WithMany(c => c.Books)
                .HasForeignKey(b => b.CategoryId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BorrowSlip>()
                .HasOne(bs => bs.Reader).WithMany(r => r.BorrowSlips)
                .HasForeignKey(bs => bs.ReaderId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<BorrowSlip>()
                .HasOne(bs => bs.Employee).WithMany(e => e.BorrowSlips)
                .HasForeignKey(bs => bs.EmployeeId).OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<BorrowDetail>()
                .HasOne(bd => bd.BorrowSlip).WithMany(bs => bs.BorrowDetails)
                .HasForeignKey(bd => bd.BorrowSlipId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BorrowDetail>()
                .HasOne(bd => bd.Book).WithMany(b => b.BorrowDetails)
                .HasForeignKey(bd => bd.BookId).OnDelete(DeleteBehavior.Restrict);

            // Seed Data
            modelBuilder.Entity<Employee>().HasData(
                new Employee { Id = 1, EmployeeCode = "NV_ADMIN", FullName = "Nguyễn Quản Trị (Admin)", Email = "admin@thuvien.edu.vn", Password = "Admin@123", Phone = "0911223344", Role = "Admin", Active = true },
                new Employee { Id = 2, EmployeeCode = "NV_THUTHU", FullName = "Trần Thị Thủ Thư", Email = "thuthu@thuvien.edu.vn", Password = "Thuthu@123", Phone = "0988776655", Role = "Librarian", Active = true }
            );

            modelBuilder.Entity<Reader>().HasData(
                new Reader { Id = 1, ReaderCode = "DG001", FullName = "Trần Minh Tuấn", Email = "tuan.tm@gmail.com", Phone = "0933112233", RegisterDate = new DateTime(2026, 1, 1), ExpiryDate = new DateTime(2027, 1, 1), Active = true },
                new Reader { Id = 2, ReaderCode = "DG002", FullName = "Lê Hoàng Yến", Email = "yen.lh@gmail.com", Phone = "0944223344", RegisterDate = new DateTime(2026, 2, 1), ExpiryDate = new DateTime(2027, 2, 1), Active = true },
                new Reader { Id = 3, ReaderCode = "DG003", FullName = "Phạm Quốc Bảo", Email = "bao.pq@gmail.com", Phone = "0955334455", RegisterDate = new DateTime(2026, 3, 1), ExpiryDate = new DateTime(2027, 3, 1), Active = true }
            );

            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Công nghệ thông tin", Description = "Lập trình, CSDL, AI", Active = true },
                new Category { Id = 2, Name = "Kinh tế & Quản trị", Description = "Kinh doanh, Khởi nghiệp", Active = true },
                new Category { Id = 3, Name = "Văn học & Nghệ thuật", Description = "Tiểu thuyết, Truyện ngắn", Active = true },
                new Category { Id = 4, Name = "Kỹ năng sống", Description = "Tư duy, Tâm lý", Active = true },
                new Category { Id = 5, Name = "Khoa học cơ bản", Description = "Lịch sử, Thiên văn", Active = true }
            );

            modelBuilder.Entity<Book>().HasData(
                new Book { Id = 1, ISBN = "9780132350884", Title = "Clean Code", CategoryId = 1, Author = "Robert C. Martin", Price = 250000, Quantity = 15, AvailableQuantity = 10, Active = true },
                new Book { Id = 2, ISBN = "9780201616224", Title = "The Pragmatic Programmer", CategoryId = 1, Author = "Andrew Hunt", Price = 280000, Quantity = 12, AvailableQuantity = 8, Active = true },
                new Book { Id = 3, ISBN = "9786045882885", Title = "Đắc Nhân Tâm", CategoryId = 4, Author = "Dale Carnegie", Price = 95000, Quantity = 20, AvailableQuantity = 14, Active = true },
                new Book { Id = 4, ISBN = "9786042171120", Title = "Mắt Biếc", CategoryId = 3, Author = "Nguyễn Nhật Ánh", Price = 110000, Quantity = 15, AvailableQuantity = 12, Active = true },
                new Book { Id = 5, ISBN = "9780062316097", Title = "Sapiens: Lược Sử Loài Người", CategoryId = 5, Author = "Yuval Noah Harari", Price = 185000, Quantity = 12, AvailableQuantity = 9, Active = true },
                new Book { Id = 6, ISBN = "9781492078005", Title = "Designing Data-Intensive Apps", CategoryId = 1, Author = "Martin Kleppmann", Price = 420000, Quantity = 8, AvailableQuantity = 6, Active = true },
                new Book { Id = 7, ISBN = "9786043440123", Title = "Nhà Giả Kim", CategoryId = 3, Author = "Paulo Coelho", Price = 89000, Quantity = 18, AvailableQuantity = 16, Active = true }
            );

            // Seed 6 Hóa đơn / Phiếu mượn across November 2026
            modelBuilder.Entity<BorrowSlip>().HasData(
                new BorrowSlip { Id = 1, SlipCode = "HD0001", ReaderId = 1, EmployeeId = 1, BorrowDate = new DateTime(2026, 11, 2, 9, 30, 0), DueDate = new DateTime(2026, 11, 16), ActualReturnDate = new DateTime(2026, 11, 14), TotalAmount = 140000, Status = "Returned", Notes = "Đã thanh toán phí thuê và trả đủ" },
                new BorrowSlip { Id = 2, SlipCode = "HD0002", ReaderId = 2, EmployeeId = 2, BorrowDate = new DateTime(2026, 11, 5, 14, 15, 0), DueDate = new DateTime(2026, 11, 19), ActualReturnDate = new DateTime(2026, 11, 18), TotalAmount = 195000, Status = "Returned", Notes = "Mượn sách lập trình & kỹ năng" },
                new BorrowSlip { Id = 3, SlipCode = "HD0003", ReaderId = 3, EmployeeId = 2, BorrowDate = new DateTime(2026, 11, 10, 10, 0, 0), DueDate = new DateTime(2026, 11, 24), TotalAmount = 260000, Status = "Paid", Notes = "Thuê dài hạn tài liệu chuyên ngành" },
                new BorrowSlip { Id = 4, SlipCode = "HD0004", ReaderId = 1, EmployeeId = 1, BorrowDate = new DateTime(2026, 11, 15, 16, 45, 0), DueDate = new DateTime(2026, 11, 29), TotalAmount = 155000, Status = "Borrowing", Notes = "Đang mượn" },
                new BorrowSlip { Id = 5, SlipCode = "HD0005", ReaderId = 2, EmployeeId = 2, BorrowDate = new DateTime(2026, 11, 20, 11, 20, 0), DueDate = new DateTime(2026, 12, 4), TotalAmount = 310000, Status = "Paid", Notes = "Mượn theo nhóm đồ án" },
                new BorrowSlip { Id = 6, SlipCode = "HD0006", ReaderId = 3, EmployeeId = 1, BorrowDate = new DateTime(2026, 11, 24, 8, 50, 0), DueDate = new DateTime(2026, 12, 8), TotalAmount = 225000, Status = "Paid", Notes = "Thanh toán tiền mặt tại quầy" }
            );

            // Seed Chi tiết phiếu mượn (Top 5 sách rõ ràng: Clean Code=8, Đắc Nhân Tâm=6, Pragmatic=5, Sapiens=4, Mắt Biếc=3)
            modelBuilder.Entity<BorrowDetail>().HasData(
                new BorrowDetail { Id = 1, BorrowSlipId = 1, BookId = 1, Quantity = 2, UnitPrice = 40000 },
                new BorrowDetail { Id = 2, BorrowSlipId = 1, BookId = 3, Quantity = 2, UnitPrice = 30000 },

                new BorrowDetail { Id = 3, BorrowSlipId = 2, BookId = 1, Quantity = 2, UnitPrice = 40000 },
                new BorrowDetail { Id = 4, BorrowSlipId = 2, BookId = 2, Quantity = 2, UnitPrice = 45000 },
                new BorrowDetail { Id = 5, BorrowSlipId = 2, BookId = 4, Quantity = 1, UnitPrice = 25000 },

                new BorrowDetail { Id = 6, BorrowSlipId = 3, BookId = 1, Quantity = 2, UnitPrice = 40000 },
                new BorrowDetail { Id = 7, BorrowSlipId = 3, BookId = 5, Quantity = 3, UnitPrice = 40000 },
                new BorrowDetail { Id = 8, BorrowSlipId = 3, BookId = 6, Quantity = 1, UnitPrice = 60000 },

                new BorrowDetail { Id = 9, BorrowSlipId = 4, BookId = 3, Quantity = 3, UnitPrice = 30000 },
                new BorrowDetail { Id = 10, BorrowSlipId = 4, BookId = 4, Quantity = 2, UnitPrice = 32500 },

                new BorrowDetail { Id = 11, BorrowSlipId = 5, BookId = 1, Quantity = 2, UnitPrice = 40000 },
                new BorrowDetail { Id = 12, BorrowSlipId = 5, BookId = 2, Quantity = 3, UnitPrice = 45000 },
                new BorrowDetail { Id = 13, BorrowSlipId = 5, BookId = 3, Quantity = 1, UnitPrice = 30000 },
                new BorrowDetail { Id = 14, BorrowSlipId = 5, BookId = 7, Quantity = 2, UnitPrice = 32500 },

                new BorrowDetail { Id = 15, BorrowSlipId = 6, BookId = 5, Quantity = 1, UnitPrice = 45000 },
                new BorrowDetail { Id = 16, BorrowSlipId = 6, BookId = 6, Quantity = 2, UnitPrice = 60000 },
                new BorrowDetail { Id = 17, BorrowSlipId = 6, BookId = 7, Quantity = 2, UnitPrice = 30000 }
            );
        }
    }
}

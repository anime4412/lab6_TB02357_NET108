using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string ISBN { get; set; } = string.Empty;

        [Required, StringLength(250)]
        public string Title { get; set; } = string.Empty;

        public int? CategoryId { get; set; }

        [StringLength(150)]
        public string? Author { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        public int Quantity { get; set; } = 1;
        public int AvailableQuantity { get; set; } = 1;
        public bool Active { get; set; } = true;

        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();
    }
}

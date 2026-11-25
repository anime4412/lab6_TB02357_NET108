using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyThuVien.Models
{
    public class BorrowSlip
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string SlipCode { get; set; } = string.Empty;

        [Required]
        public int ReaderId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime BorrowDate { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ActualReturnDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;

        [StringLength(50)]
        public string Status { get; set; } = "Paid"; // Paid, Borrowing, Returned, Cancelled

        [StringLength(500)]
        public string? Notes { get; set; }

        [ForeignKey("ReaderId")]
        public virtual Reader? Reader { get; set; }

        [ForeignKey("EmployeeId")]
        public virtual Employee? Employee { get; set; }

        public virtual ICollection<BorrowDetail> BorrowDetails { get; set; } = new List<BorrowDetail>();
    }
}

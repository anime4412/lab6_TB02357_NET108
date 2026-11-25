using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyThuVien.Models
{
    public class Reader
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string ReaderCode { get; set; } = string.Empty;

        [Required, StringLength(150)]
        public string FullName { get; set; } = string.Empty;

        [Required, EmailAddress, StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [StringLength(15)]
        public string? Phone { get; set; }

        public DateTime RegisterDate { get; set; } = new DateTime(2026, 1, 1);
        public DateTime ExpiryDate { get; set; } = new DateTime(2027, 1, 1);
        public bool Active { get; set; } = true;

        public virtual ICollection<BorrowSlip> BorrowSlips { get; set; } = new List<BorrowSlip>();
    }
}

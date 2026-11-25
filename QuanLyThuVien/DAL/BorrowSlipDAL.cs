using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    /// <summary>
    /// BÀI 1 (5 ĐIỂM): Quản lý hóa đơn / phiếu mượn trả phía Quản lý
    /// </summary>
    public class BorrowSlipDAL
    {
        private readonly LibraryDbContext _context;

        public BorrowSlipDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public List<BorrowSlip> GetAll()
        {
            return _context.BorrowSlips
                .Include(bs => bs.Reader)
                .Include(bs => bs.Employee)
                .Include(bs => bs.BorrowDetails)
                .OrderByDescending(bs => bs.BorrowDate)
                .AsNoTracking()
                .ToList();
        }

        public BorrowSlip? GetByIdWithDetails(int id)
        {
            return _context.BorrowSlips
                .Include(bs => bs.Reader)
                .Include(bs => bs.Employee)
                .Include(bs => bs.BorrowDetails)
                    .ThenInclude(bd => bd.Book)
                        .ThenInclude(b => b!.Category)
                .AsNoTracking()
                .FirstOrDefault(bs => bs.Id == id);
        }

        public bool UpdateStatus(int id, string newStatus)
        {
            var slip = _context.BorrowSlips.Find(id);
            if (slip == null) return false;

            slip.Status = newStatus;
            if (newStatus == "Returned" && slip.ActualReturnDate == null)
            {
                slip.ActualReturnDate = System.DateTime.Now;
            }
            return _context.SaveChanges() > 0;
        }
    }
}

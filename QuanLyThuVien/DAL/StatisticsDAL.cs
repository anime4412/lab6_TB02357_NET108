using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    /// <summary>
    /// BÀI 2 (3 ĐIỂM): Thống kê Top 5 sách mượn nhiều nhất & Doanh thu theo khoảng thời gian
    /// </summary>
    public class StatisticsDAL
    {
        private readonly LibraryDbContext _context;

        public StatisticsDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public StatisticsViewModel GetStatistics(DateTime fromDate, DateTime toDate)
        {
            var start = fromDate.Date;
            var end = toDate.Date.AddDays(1).AddTicks(-1); // Bao trọn cuối ngày toDate

            // 1. Lọc các hóa đơn / phiếu mượn hợp lệ (loại trừ Cancelled) trong khoảng thời gian
            var slipsQuery = _context.BorrowSlips
                .AsNoTracking()
                .Where(bs => bs.BorrowDate >= start && bs.BorrowDate <= end && bs.Status != "Cancelled");

            var slips = slipsQuery.ToList();

            // 2. Thống kê doanh thu theo từng ngày
            var dailyRevenues = slips
                .GroupBy(bs => bs.BorrowDate.Date)
                .Select(g => new DailyRevenueStatItem
                {
                    Date = g.Key,
                    SlipCount = g.Count(),
                    Revenue = g.Sum(x => x.TotalAmount)
                })
                .OrderBy(d => d.Date)
                .ToList();

            // 3. Thống kê Top 5 sách được mượn nhiều nhất trong khoảng thời gian
            var top5Books = _context.BorrowDetails
                .Include(bd => bd.Book)
                    .ThenInclude(b => b!.Category)
                .Include(bd => bd.BorrowSlip)
                .AsNoTracking()
                .Where(bd => bd.BorrowSlip != null
                          && bd.BorrowSlip.BorrowDate >= start
                          && bd.BorrowSlip.BorrowDate <= end
                          && bd.BorrowSlip.Status != "Cancelled"
                          && bd.Book != null)
                .GroupBy(bd => new
                {
                    bd.BookId,
                    bd.Book!.ISBN,
                    bd.Book.Title,
                    CategoryName = bd.Book.Category != null ? bd.Book.Category.Name : "Khác"
                })
                .Select(g => new TopBookStatItem
                {
                    BookId = g.Key.BookId,
                    ISBN = g.Key.ISBN,
                    BookTitle = g.Key.Title,
                    CategoryName = g.Key.CategoryName,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
                })
                .OrderByDescending(x => x.TotalQuantity)
                .ThenByDescending(x => x.TotalRevenue)
                .Take(5)
                .ToList();

            return new StatisticsViewModel
            {
                FromDate = start,
                ToDate = toDate.Date,
                Top5Books = top5Books,
                RevenueByDates = dailyRevenues,
                TotalRevenue = slips.Sum(s => s.TotalAmount),
                TotalSlips = slips.Count,
                TotalBooksBorrowed = top5Books.Sum(b => b.TotalQuantity)
            };
        }
    }
}

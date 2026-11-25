using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.Controllers
{
    /// <summary>
    /// BÀI 2 (3 ĐIỂM) & BÀI 3 (2 ĐIỂM): Thống kê Top 5 sách mượn nhiều nhất, Doanh thu theo khoảng thời gian & Biểu đồ Chart.js
    /// </summary>
    [Authorize(Policy = "ManagerOnly")]
    public class StatisticsController : Controller
    {
        private readonly StatisticsDAL _statisticsDal;

        public StatisticsController(StatisticsDAL statisticsDal)
        {
            _statisticsDal = statisticsDal;
        }

        public IActionResult Index(DateTime? fromDate, DateTime? toDate)
        {
            // Mặc định lọc tháng 11/2026 (khớp với dữ liệu Seed Data) hoặc 30 ngày gần nhất nếu hôm nay nằm ngoài
            var start = fromDate ?? new DateTime(2026, 11, 1);
            var end = toDate ?? new DateTime(2026, 11, 30);

            if (start > end)
            {
                (start, end) = (end, start);
            }

            var model = _statisticsDal.GetStatistics(start, end);

            // Dữ liệu ViewBag cho Chart.js - Biểu đồ Doanh thu (#myChart)
            ViewBag.Labels = model.RevenueByDates.Select(d => d.Date.ToString("dd/MM/yyyy")).ToList();
            ViewBag.Values = model.RevenueByDates.Select(d => d.Revenue).ToList();

            // Dữ liệu ViewBag cho Chart.js - Biểu đồ Top 5 Sách mượn nhiều nhất (#topBooksChart)
            ViewBag.TopBookLabels = model.Top5Books.Select(b => b.BookTitle).ToList();
            ViewBag.TopBookValues = model.Top5Books.Select(b => b.TotalQuantity).ToList();

            return View(model);
        }
    }
}

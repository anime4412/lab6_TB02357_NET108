using System;
using System.Collections.Generic;

namespace QuanLyThuVien.Models
{
    public class TopBookStatItem
    {
        public int BookId { get; set; }
        public string ISBN { get; set; } = string.Empty;
        public string BookTitle { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class DailyRevenueStatItem
    {
        public DateTime Date { get; set; }
        public int SlipCount { get; set; }
        public decimal Revenue { get; set; }
    }

    public class StatisticsViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public List<TopBookStatItem> Top5Books { get; set; } = new();
        public List<DailyRevenueStatItem> RevenueByDates { get; set; } = new();
        public decimal TotalRevenue { get; set; }
        public int TotalSlips { get; set; }
        public int TotalBooksBorrowed { get; set; }
    }
}

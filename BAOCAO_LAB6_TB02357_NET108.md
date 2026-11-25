# BÁO CÁO THỰC HÀNH LAB 6: QUẢN LÝ HÓA ĐƠN, THỐNG KÊ & BIỂU ĐỒ CHART.JS (EF CORE CODE FIRST)
**MÔN HỌC: DỰ ÁN .NET 1 (NET108 / SOF1022)**  
**LỚP: DỰ ÁN 1 — NHÓM THỰC HIỆN: NHÓM 5**  
**MÃ SINH VIÊN: TB02357**  
**ĐỀ TÀI: HỆ THỐNG QUẢN LÝ THƯ VIỆN ĐIỆN TỬ (LIBRARY MANAGEMENT SYSTEM)**  

---

## MỤC TIÊU BÀI THỰC HÀNH LAB 6
1. **Xây dựng chức năng Quản lý Hóa đơn / Phiếu mượn trả ở phía Quản lý (Bài 1 - 5 điểm)**: Hiển thị toàn bộ danh sách đơn mượn và xem chi tiết từng đơn hàng (danh sách sách, số lượng, đơn giá, tổng tiền, trạng thái, tên nhân viên tạo đơn, ngày tạo đơn).
2. **Xây dựng chức năng Thống kê dữ liệu (Bài 2 - 3 điểm)**: Thống kê Top 5 sách được mượn nhiều nhất và thống kê doanh thu theo khoảng thời gian tùy chọn (`FromDate` – `ToDate`) bằng LINQ trên nền **EF Core 8.0 Code First**.
3. **Trực quan hóa biểu đồ với Chart.js & Ứng dụng AI (Bài 3 - 2 điểm)**: Vẽ biểu đồ Doanh thu (`<canvas id="myChart">`) và biểu đồ Top 5 Sách (`<canvas id="topBooksChart">`) từ dữ liệu JSON truyền qua `ViewBag`.

---

## PHẦN I: ÁP DỤNG BÀI HỌC

### BÀI 1 (5 ĐIỂM): XÂY DỰNG CHỨC NĂNG QUẢN LÝ HÓA ĐƠN / PHIẾU MƯỢN PHÍA QUẢN LÝ

#### 1. Kiến trúc dữ liệu Code First (`Data/LibraryDbContext.cs`)
* Sử dụng EF Core 8.0 Code First định nghĩa quan hệ `BorrowSlip` (1) — `BorrowDetail` (N) — `Book` (1) và liên kết tới `Employee` (Nhân viên tạo đơn) và `Reader` (Độc giả).
* Sinh CSDL và dữ liệu mẫu bằng lệnh `dotnet ef migrations add InitialCreate` và `dotnet ef database update`.

#### 2. `BorrowSlipsController.cs` & `BorrowSlipDAL.cs`
* Toàn bộ Controller được bảo vệ bởi `[Authorize(Policy = "ManagerOnly")]`.
* **`Index()`**: Lấy danh sách tất cả hóa đơn / phiếu mượn kèm thông tin Độc giả, Nhân viên lập phiếu, số lượng sách, tổng tiền và trạng thái.
* **`Details(int id)`**: Eager Loading qua `.Include(bs => bs.Reader).Include(bs => bs.Employee).Include(bs => bs.BorrowDetails).ThenInclude(bd => bd.Book)` để hiển thị đầy đủ:
  - Danh sách sách trong đơn (`Title`, `ISBN`, `Category`),
  - Số lượng (`Quantity`) & Đơn giá (`UnitPrice`) & Thành tiền (`LineTotal`),
  - Tổng tiền (`TotalAmount`), Trạng thái (`Status`), Tên nhân viên tạo đơn (`Employee.FullName`) và Ngày tạo đơn (`BorrowDate`).

---

### BÀI 2 (3 ĐIỂM): THỐNG KÊ TOP 5 SÁCH BÁN CHẠY / MƯỢN NHIỀU & DOANH THU THEO THỜI GIAN

Tại `StatisticsDAL.GetStatistics(DateTime fromDate, DateTime toDate)`:
```csharp
// 1. Thống kê doanh thu theo ngày trong khoảng thời gian tùy chọn
var dailyRevenues = _context.BorrowSlips
    .AsNoTracking()
    .Where(bs => bs.BorrowDate >= start && bs.BorrowDate <= end && bs.Status != "Cancelled")
    .ToList()
    .GroupBy(bs => bs.BorrowDate.Date)
    .Select(g => new DailyRevenueStatItem
    {
        Date = g.Key,
        SlipCount = g.Count(),
        Revenue = g.Sum(x => x.TotalAmount)
    })
    .OrderBy(d => d.Date)
    .ToList();

// 2. Thống kê Top 5 sách được mượn nhiều nhất
var top5Books = _context.BorrowDetails
    .Include(bd => bd.Book).ThenInclude(b => b!.Category)
    .Include(bd => bd.BorrowSlip)
    .AsNoTracking()
    .Where(bd => bd.BorrowSlip != null && bd.BorrowSlip.BorrowDate >= start
              && bd.BorrowSlip.BorrowDate <= end && bd.BorrowSlip.Status != "Cancelled")
    .GroupBy(bd => new { bd.BookId, bd.Book!.ISBN, bd.Book.Title, CategoryName = bd.Book.Category!.Name })
    .Select(g => new TopBookStatItem
    {
        BookId = g.Key.BookId,
        BookTitle = g.Key.Title,
        CategoryName = g.Key.CategoryName,
        TotalQuantity = g.Sum(x => x.Quantity),
        TotalRevenue = g.Sum(x => x.Quantity * x.UnitPrice)
    })
    .OrderByDescending(x => x.TotalQuantity)
    .Take(5)
    .ToList();
```

---

## PHẦN II: NÂNG CẤP TÍNH NĂNG

### BÀI 3 (2 ĐIỂM): TẠO BIỂU ĐỒ THỐNG KÊ VỚI CHART.JS & ỨNG DỤNG AI

#### 1. Câu lệnh Prompt ứng dụng AI tạo biểu đồ
> *"Tôi đang xây dựng ứng dụng .NET Core 8 MVC với Razor View cho hệ thống Quản lý Thư viện. Tôi đã tính được dữ liệu thống kê doanh thu theo ngày (ViewBag.Labels, ViewBag.Values) và Top 5 sách được mượn nhiều nhất (ViewBag.TopBookLabels, ViewBag.TopBookValues). Hãy hỗ trợ viết mã Razor + JavaScript sử dụng thư viện Chart.js để vẽ biểu đồ lên thẻ `<canvas id="myChart"></canvas>` và cho phép chuyển đổi giữa biểu đồ cột (bar) và biểu đồ đường (line)."*

#### 2. Mã nguồn triển khai tại `Views/Statistics/Index.cshtml`
```html
<div style="width: 100%; margin: auto;">
    <canvas id="myChart"></canvas>
</div>

<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<script>
    // Nhận dữ liệu từ ViewBag (Convert sang JSON để JS hiểu được)
    const labels = @Json.Serialize(ViewBag.Labels);
    const dataValues = @Json.Serialize(ViewBag.Values);

    const ctx = document.getElementById('myChart').getContext('2d');
    new Chart(ctx, {
        type: 'bar', // Có thể chuyển sang 'line', 'pie', 'doughnut'
        data: {
            labels: labels,
            datasets: [{
                label: 'Doanh thu (VNĐ)',
                data: dataValues,
                backgroundColor: 'rgba(54, 162, 235, 0.5)',
                borderColor: 'rgba(54, 162, 235, 1)',
                borderWidth: 2
            }]
        },
        options: {
            responsive: true,
            scales: {
                y: { beginAtZero: true }
            }
        }
    });
</script>
```

---

## TỔNG KẾT ĐÁNH GIÁ KẾT QUẢ LAB 6
* **Bài 1 (5/5 điểm)**: Hoàn thiện `BorrowSlipsController` hiển thị danh sách tất cả hóa đơn và xem chi tiết đầy đủ các trường theo yêu cầu.
* **Bài 2 (3/3 điểm)**: Hoàn thiện `StatisticsController` và `StatisticsDAL` thống kê Top 5 sách mượn nhiều nhất + doanh thu theo khoảng thời gian tùy chọn bằng EF Core Code First LINQ.
* **Bài 3 (2/2 điểm)**: Tích hợp Chart.js vẽ 2 biểu đồ trực quan (`#myChart` cho doanh thu có nút đổi kiểu `bar`/`line`, `#topBooksChart` dạng `doughnut` cho Top 5 sách) kèm phần ứng dụng AI đầy đủ.
* Dự án biên dịch thành công: **0 Warning(s), 0 Error(s)**.

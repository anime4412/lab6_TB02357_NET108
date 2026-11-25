using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuanLyThuVien.DAL;

namespace QuanLyThuVien.Controllers
{
    /// <summary>
    /// BÀI 1 (5 ĐIỂM): Quản lý hóa đơn / phiếu mượn trả phía Quản lý
    /// </summary>
    [Authorize(Policy = "ManagerOnly")]
    public class BorrowSlipsController : Controller
    {
        private readonly BorrowSlipDAL _borrowSlipDal;

        public BorrowSlipsController(BorrowSlipDAL borrowSlipDal)
        {
            _borrowSlipDal = borrowSlipDal;
        }

        // Hiển thị danh sách tất cả hóa đơn / phiếu mượn
        public IActionResult Index()
        {
            var slips = _borrowSlipDal.GetAll();
            return View(slips);
        }

        // Xem chi tiết đơn hàng / phiếu mượn: sách, số lượng, đơn giá, tổng tiền, trạng thái, nhân viên tạo, ngày tạo
        public IActionResult Details(int id)
        {
            var slip = _borrowSlipDal.GetByIdWithDetails(id);
            if (slip == null) return NotFound();
            return View(slip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            if (_borrowSlipDal.UpdateStatus(id, status))
            {
                TempData["Success"] = $"Đã cập nhật trạng thái đơn #{id} sang '{status}' thành công!";
            }
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}

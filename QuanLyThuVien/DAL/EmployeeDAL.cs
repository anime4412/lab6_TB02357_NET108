using System.Linq;
using Microsoft.EntityFrameworkCore;
using QuanLyThuVien.Data;
using QuanLyThuVien.Models;

namespace QuanLyThuVien.DAL
{
    public class EmployeeDAL
    {
        private readonly LibraryDbContext _context;

        public EmployeeDAL(LibraryDbContext context)
        {
            _context = context;
        }

        public Employee? Authenticate(string email, string password)
        {
            return _context.Employees
                .AsNoTracking()
                .FirstOrDefault(e => e.Email == email && e.Password == password && e.Active);
        }
    }
}

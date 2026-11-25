using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuanLyThuVien.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReaderCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    RegisterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Readers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ISBN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Author = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AvailableQuantity = table.Column<int>(type: "int", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Books_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "BorrowSlips",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SlipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReaderId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    BorrowDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActualReturnDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowSlips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowSlips_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_BorrowSlips_Readers_ReaderId",
                        column: x => x.ReaderId,
                        principalTable: "Readers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BorrowDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BorrowSlipId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BorrowDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BorrowDetails_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BorrowDetails_BorrowSlips_BorrowSlipId",
                        column: x => x.BorrowSlipId,
                        principalTable: "BorrowSlips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Active", "Description", "Name" },
                values: new object[,]
                {
                    { 1, true, "Lập trình, CSDL, AI", "Công nghệ thông tin" },
                    { 2, true, "Kinh doanh, Khởi nghiệp", "Kinh tế & Quản trị" },
                    { 3, true, "Tiểu thuyết, Truyện ngắn", "Văn học & Nghệ thuật" },
                    { 4, true, "Tư duy, Tâm lý", "Kỹ năng sống" },
                    { 5, true, "Lịch sử, Thiên văn", "Khoa học cơ bản" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "Active", "Email", "EmployeeCode", "FullName", "Password", "Phone", "Role" },
                values: new object[,]
                {
                    { 1, true, "admin@thuvien.edu.vn", "NV_ADMIN", "Nguyễn Quản Trị (Admin)", "Admin@123", "0911223344", "Admin" },
                    { 2, true, "thuthu@thuvien.edu.vn", "NV_THUTHU", "Trần Thị Thủ Thư", "Thuthu@123", "0988776655", "Librarian" }
                });

            migrationBuilder.InsertData(
                table: "Readers",
                columns: new[] { "Id", "Active", "Email", "ExpiryDate", "FullName", "Phone", "ReaderCode", "RegisterDate" },
                values: new object[,]
                {
                    { 1, true, "tuan.tm@gmail.com", new DateTime(2027, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Trần Minh Tuấn", "0933112233", "DG001", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, true, "yen.lh@gmail.com", new DateTime(2027, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lê Hoàng Yến", "0944223344", "DG002", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, true, "bao.pq@gmail.com", new DateTime(2027, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Phạm Quốc Bảo", "0955334455", "DG003", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Active", "Author", "AvailableQuantity", "CategoryId", "ISBN", "Price", "Quantity", "Title" },
                values: new object[,]
                {
                    { 1, true, "Robert C. Martin", 10, 1, "9780132350884", 250000m, 15, "Clean Code" },
                    { 2, true, "Andrew Hunt", 8, 1, "9780201616224", 280000m, 12, "The Pragmatic Programmer" },
                    { 3, true, "Dale Carnegie", 14, 4, "9786045882885", 95000m, 20, "Đắc Nhân Tâm" },
                    { 4, true, "Nguyễn Nhật Ánh", 12, 3, "9786042171120", 110000m, 15, "Mắt Biếc" },
                    { 5, true, "Yuval Noah Harari", 9, 5, "9780062316097", 185000m, 12, "Sapiens: Lược Sử Loài Người" },
                    { 6, true, "Martin Kleppmann", 6, 1, "9781492078005", 420000m, 8, "Designing Data-Intensive Apps" },
                    { 7, true, "Paulo Coelho", 16, 3, "9786043440123", 89000m, 18, "Nhà Giả Kim" }
                });

            migrationBuilder.InsertData(
                table: "BorrowSlips",
                columns: new[] { "Id", "ActualReturnDate", "BorrowDate", "DueDate", "EmployeeId", "Notes", "ReaderId", "SlipCode", "Status", "TotalAmount" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 11, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 2, 9, 30, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 16, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Đã thanh toán phí thuê và trả đủ", 1, "HD0001", "Returned", 140000m },
                    { 2, new DateTime(2026, 11, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 5, 14, 15, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 19, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Mượn sách lập trình & kỹ năng", 2, "HD0002", "Returned", 195000m },
                    { 3, null, new DateTime(2026, 11, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Thuê dài hạn tài liệu chuyên ngành", 3, "HD0003", "Paid", 260000m },
                    { 4, null, new DateTime(2026, 11, 15, 16, 45, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 11, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Đang mượn", 1, "HD0004", "Borrowing", 155000m },
                    { 5, null, new DateTime(2026, 11, 20, 11, 20, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), 2, "Mượn theo nhóm đồ án", 2, "HD0005", "Paid", 310000m },
                    { 6, null, new DateTime(2026, 11, 24, 8, 50, 0, 0, DateTimeKind.Unspecified), new DateTime(2026, 12, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, "Thanh toán tiền mặt tại quầy", 3, "HD0006", "Paid", 225000m }
                });

            migrationBuilder.InsertData(
                table: "BorrowDetails",
                columns: new[] { "Id", "BookId", "BorrowSlipId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 2, 40000m },
                    { 2, 3, 1, 2, 30000m },
                    { 3, 1, 2, 2, 40000m },
                    { 4, 2, 2, 2, 45000m },
                    { 5, 4, 2, 1, 25000m },
                    { 6, 1, 3, 2, 40000m },
                    { 7, 5, 3, 3, 40000m },
                    { 8, 6, 3, 1, 60000m },
                    { 9, 3, 4, 3, 30000m },
                    { 10, 4, 4, 2, 32500m },
                    { 11, 1, 5, 2, 40000m },
                    { 12, 2, 5, 3, 45000m },
                    { 13, 3, 5, 1, 30000m },
                    { 14, 7, 5, 2, 32500m },
                    { 15, 5, 6, 1, 45000m },
                    { 16, 6, 6, 2, 60000m },
                    { 17, 7, 6, 2, 30000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_CategoryId",
                table: "Books",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Books_ISBN",
                table: "Books",
                column: "ISBN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BorrowDetails_BookId",
                table: "BorrowDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowDetails_BorrowSlipId",
                table: "BorrowDetails",
                column: "BorrowSlipId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowSlips_EmployeeId",
                table: "BorrowSlips",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowSlips_ReaderId",
                table: "BorrowSlips",
                column: "ReaderId");

            migrationBuilder.CreateIndex(
                name: "IX_BorrowSlips_SlipCode",
                table: "BorrowSlips",
                column: "SlipCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeCode",
                table: "Employees",
                column: "EmployeeCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Readers_ReaderCode",
                table: "Readers",
                column: "ReaderCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BorrowDetails");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "BorrowSlips");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Readers");
        }
    }
}

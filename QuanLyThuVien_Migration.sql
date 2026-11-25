IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Categories] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(150) NOT NULL,
    [Description] nvarchar(500) NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Categories] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Employees] (
    [Id] int NOT NULL IDENTITY,
    [EmployeeCode] nvarchar(20) NOT NULL,
    [FullName] nvarchar(150) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Password] nvarchar(255) NOT NULL,
    [Phone] nvarchar(15) NULL,
    [Role] nvarchar(50) NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Readers] (
    [Id] int NOT NULL IDENTITY,
    [ReaderCode] nvarchar(20) NOT NULL,
    [FullName] nvarchar(150) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [Phone] nvarchar(15) NULL,
    [RegisterDate] datetime2 NOT NULL,
    [ExpiryDate] datetime2 NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Readers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [Books] (
    [Id] int NOT NULL IDENTITY,
    [ISBN] nvarchar(20) NOT NULL,
    [Title] nvarchar(250) NOT NULL,
    [CategoryId] int NULL,
    [Author] nvarchar(150) NULL,
    [Price] decimal(18,2) NOT NULL,
    [Quantity] int NOT NULL,
    [AvailableQuantity] int NOT NULL,
    [Active] bit NOT NULL,
    CONSTRAINT [PK_Books] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Books_Categories_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [Categories] ([Id]) ON DELETE SET NULL
);
GO

CREATE TABLE [BorrowSlips] (
    [Id] int NOT NULL IDENTITY,
    [SlipCode] nvarchar(20) NOT NULL,
    [ReaderId] int NOT NULL,
    [EmployeeId] int NULL,
    [BorrowDate] datetime2 NOT NULL,
    [DueDate] datetime2 NOT NULL,
    [ActualReturnDate] datetime2 NULL,
    [TotalAmount] decimal(18,2) NOT NULL,
    [Status] nvarchar(50) NOT NULL,
    [Notes] nvarchar(500) NULL,
    CONSTRAINT [PK_BorrowSlips] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BorrowSlips_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_BorrowSlips_Readers_ReaderId] FOREIGN KEY ([ReaderId]) REFERENCES [Readers] ([Id]) ON DELETE NO ACTION
);
GO

CREATE TABLE [BorrowDetails] (
    [Id] int NOT NULL IDENTITY,
    [BorrowSlipId] int NOT NULL,
    [BookId] int NOT NULL,
    [Quantity] int NOT NULL,
    [UnitPrice] decimal(18,2) NOT NULL,
    CONSTRAINT [PK_BorrowDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BorrowDetails_Books_BookId] FOREIGN KEY ([BookId]) REFERENCES [Books] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_BorrowDetails_BorrowSlips_BorrowSlipId] FOREIGN KEY ([BorrowSlipId]) REFERENCES [BorrowSlips] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] ON;
INSERT INTO [Categories] ([Id], [Active], [Description], [Name])
VALUES (1, CAST(1 AS bit), N'Lập trình, CSDL, AI', N'Công nghệ thông tin'),
(2, CAST(1 AS bit), N'Kinh doanh, Khởi nghiệp', N'Kinh tế & Quản trị'),
(3, CAST(1 AS bit), N'Tiểu thuyết, Truyện ngắn', N'Văn học & Nghệ thuật'),
(4, CAST(1 AS bit), N'Tư duy, Tâm lý', N'Kỹ năng sống'),
(5, CAST(1 AS bit), N'Lịch sử, Thiên văn', N'Khoa học cơ bản');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Description', N'Name') AND [object_id] = OBJECT_ID(N'[Categories]'))
    SET IDENTITY_INSERT [Categories] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Email', N'EmployeeCode', N'FullName', N'Password', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] ON;
INSERT INTO [Employees] ([Id], [Active], [Email], [EmployeeCode], [FullName], [Password], [Phone], [Role])
VALUES (1, CAST(1 AS bit), N'admin@thuvien.edu.vn', N'NV_ADMIN', N'Nguyễn Quản Trị (Admin)', N'Admin@123', N'0911223344', N'Admin'),
(2, CAST(1 AS bit), N'thuthu@thuvien.edu.vn', N'NV_THUTHU', N'Trần Thị Thủ Thư', N'Thuthu@123', N'0988776655', N'Librarian');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Email', N'EmployeeCode', N'FullName', N'Password', N'Phone', N'Role') AND [object_id] = OBJECT_ID(N'[Employees]'))
    SET IDENTITY_INSERT [Employees] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Email', N'ExpiryDate', N'FullName', N'Phone', N'ReaderCode', N'RegisterDate') AND [object_id] = OBJECT_ID(N'[Readers]'))
    SET IDENTITY_INSERT [Readers] ON;
INSERT INTO [Readers] ([Id], [Active], [Email], [ExpiryDate], [FullName], [Phone], [ReaderCode], [RegisterDate])
VALUES (1, CAST(1 AS bit), N'tuan.tm@gmail.com', '2027-01-01T00:00:00.0000000', N'Trần Minh Tuấn', N'0933112233', N'DG001', '2026-01-01T00:00:00.0000000'),
(2, CAST(1 AS bit), N'yen.lh@gmail.com', '2027-02-01T00:00:00.0000000', N'Lê Hoàng Yến', N'0944223344', N'DG002', '2026-02-01T00:00:00.0000000'),
(3, CAST(1 AS bit), N'bao.pq@gmail.com', '2027-03-01T00:00:00.0000000', N'Phạm Quốc Bảo', N'0955334455', N'DG003', '2026-03-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Email', N'ExpiryDate', N'FullName', N'Phone', N'ReaderCode', N'RegisterDate') AND [object_id] = OBJECT_ID(N'[Readers]'))
    SET IDENTITY_INSERT [Readers] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Author', N'AvailableQuantity', N'CategoryId', N'ISBN', N'Price', N'Quantity', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] ON;
INSERT INTO [Books] ([Id], [Active], [Author], [AvailableQuantity], [CategoryId], [ISBN], [Price], [Quantity], [Title])
VALUES (1, CAST(1 AS bit), N'Robert C. Martin', 10, 1, N'9780132350884', 250000.0, 15, N'Clean Code'),
(2, CAST(1 AS bit), N'Andrew Hunt', 8, 1, N'9780201616224', 280000.0, 12, N'The Pragmatic Programmer'),
(3, CAST(1 AS bit), N'Dale Carnegie', 14, 4, N'9786045882885', 95000.0, 20, N'Đắc Nhân Tâm'),
(4, CAST(1 AS bit), N'Nguyễn Nhật Ánh', 12, 3, N'9786042171120', 110000.0, 15, N'Mắt Biếc'),
(5, CAST(1 AS bit), N'Yuval Noah Harari', 9, 5, N'9780062316097', 185000.0, 12, N'Sapiens: Lược Sử Loài Người'),
(6, CAST(1 AS bit), N'Martin Kleppmann', 6, 1, N'9781492078005', 420000.0, 8, N'Designing Data-Intensive Apps'),
(7, CAST(1 AS bit), N'Paulo Coelho', 16, 3, N'9786043440123', 89000.0, 18, N'Nhà Giả Kim');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Active', N'Author', N'AvailableQuantity', N'CategoryId', N'ISBN', N'Price', N'Quantity', N'Title') AND [object_id] = OBJECT_ID(N'[Books]'))
    SET IDENTITY_INSERT [Books] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ActualReturnDate', N'BorrowDate', N'DueDate', N'EmployeeId', N'Notes', N'ReaderId', N'SlipCode', N'Status', N'TotalAmount') AND [object_id] = OBJECT_ID(N'[BorrowSlips]'))
    SET IDENTITY_INSERT [BorrowSlips] ON;
INSERT INTO [BorrowSlips] ([Id], [ActualReturnDate], [BorrowDate], [DueDate], [EmployeeId], [Notes], [ReaderId], [SlipCode], [Status], [TotalAmount])
VALUES (1, '2026-11-14T00:00:00.0000000', '2026-11-02T09:30:00.0000000', '2026-11-16T00:00:00.0000000', 1, N'Đã thanh toán phí thuê và trả đủ', 1, N'HD0001', N'Returned', 140000.0),
(2, '2026-11-18T00:00:00.0000000', '2026-11-05T14:15:00.0000000', '2026-11-19T00:00:00.0000000', 2, N'Mượn sách lập trình & kỹ năng', 2, N'HD0002', N'Returned', 195000.0),
(3, NULL, '2026-11-10T10:00:00.0000000', '2026-11-24T00:00:00.0000000', 2, N'Thuê dài hạn tài liệu chuyên ngành', 3, N'HD0003', N'Paid', 260000.0),
(4, NULL, '2026-11-15T16:45:00.0000000', '2026-11-29T00:00:00.0000000', 1, N'Đang mượn', 1, N'HD0004', N'Borrowing', 155000.0),
(5, NULL, '2026-11-20T11:20:00.0000000', '2026-12-04T00:00:00.0000000', 2, N'Mượn theo nhóm đồ án', 2, N'HD0005', N'Paid', 310000.0),
(6, NULL, '2026-11-24T08:50:00.0000000', '2026-12-08T00:00:00.0000000', 1, N'Thanh toán tiền mặt tại quầy', 3, N'HD0006', N'Paid', 225000.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'ActualReturnDate', N'BorrowDate', N'DueDate', N'EmployeeId', N'Notes', N'ReaderId', N'SlipCode', N'Status', N'TotalAmount') AND [object_id] = OBJECT_ID(N'[BorrowSlips]'))
    SET IDENTITY_INSERT [BorrowSlips] OFF;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BookId', N'BorrowSlipId', N'Quantity', N'UnitPrice') AND [object_id] = OBJECT_ID(N'[BorrowDetails]'))
    SET IDENTITY_INSERT [BorrowDetails] ON;
INSERT INTO [BorrowDetails] ([Id], [BookId], [BorrowSlipId], [Quantity], [UnitPrice])
VALUES (1, 1, 1, 2, 40000.0),
(2, 3, 1, 2, 30000.0),
(3, 1, 2, 2, 40000.0),
(4, 2, 2, 2, 45000.0),
(5, 4, 2, 1, 25000.0),
(6, 1, 3, 2, 40000.0),
(7, 5, 3, 3, 40000.0),
(8, 6, 3, 1, 60000.0),
(9, 3, 4, 3, 30000.0),
(10, 4, 4, 2, 32500.0),
(11, 1, 5, 2, 40000.0),
(12, 2, 5, 3, 45000.0),
(13, 3, 5, 1, 30000.0),
(14, 7, 5, 2, 32500.0),
(15, 5, 6, 1, 45000.0),
(16, 6, 6, 2, 60000.0),
(17, 7, 6, 2, 30000.0);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BookId', N'BorrowSlipId', N'Quantity', N'UnitPrice') AND [object_id] = OBJECT_ID(N'[BorrowDetails]'))
    SET IDENTITY_INSERT [BorrowDetails] OFF;
GO

CREATE INDEX [IX_Books_CategoryId] ON [Books] ([CategoryId]);
GO

CREATE UNIQUE INDEX [IX_Books_ISBN] ON [Books] ([ISBN]);
GO

CREATE INDEX [IX_BorrowDetails_BookId] ON [BorrowDetails] ([BookId]);
GO

CREATE INDEX [IX_BorrowDetails_BorrowSlipId] ON [BorrowDetails] ([BorrowSlipId]);
GO

CREATE INDEX [IX_BorrowSlips_EmployeeId] ON [BorrowSlips] ([EmployeeId]);
GO

CREATE INDEX [IX_BorrowSlips_ReaderId] ON [BorrowSlips] ([ReaderId]);
GO

CREATE UNIQUE INDEX [IX_BorrowSlips_SlipCode] ON [BorrowSlips] ([SlipCode]);
GO

CREATE UNIQUE INDEX [IX_Employees_Email] ON [Employees] ([Email]);
GO

CREATE UNIQUE INDEX [IX_Employees_EmployeeCode] ON [Employees] ([EmployeeCode]);
GO

CREATE UNIQUE INDEX [IX_Readers_ReaderCode] ON [Readers] ([ReaderCode]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20261125055315_InitialCreate', N'8.0.11');
GO

COMMIT;
GO


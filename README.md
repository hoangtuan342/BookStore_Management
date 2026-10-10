# Trang Sách

Đồ án quản lý cửa hàng sách gồm Web bán hàng, Web quản lý, API ASP.NET Core, SQL Server và ứng dụng khách hàng .NET MAUI cho Android.

## Bắt đầu nhanh

Yêu cầu tối thiểu: Windows 10/11, .NET SDK 10, SQL Server đang chạy và quyền tạo database trên SQL Server. API tự áp dụng EF Core migrations và nạp dữ liệu mẫu khi khởi động lần đầu.

```powershell
dotnet restore .\src\BookStore.sln
dotnet run --project .\src\BookStore.Api\BookStore.Api.csproj
```

Mở PowerShell thứ hai:

```powershell
dotnet run --project .\src\BookStore.Web\BookStore.Web.csproj
```

- Web: <http://localhost:5245>
- Đăng nhập quản lý: <http://localhost:5245/admin/login>
- API OpenAPI: <http://localhost:5017/openapi/v1.json>
- Tạo quản trị viên đầu tiên (Development): <http://localhost:5245/admin/setup>

Hướng dẫn cài đặt chi tiết, Android, database, đăng nhập, xử lý lỗi và quy trình nhóm: [HUONG_DAN_NHOM.md](HUONG_DAN_NHOM.md).

## Thành phần

| Project | Vai trò |
| --- | --- |
| `src/BookStore.Web` | Web Blazor cho khách hàng và nhân viên quản lý |
| `src/BookStore` | Ứng dụng .NET MAUI Blazor Hybrid cho Android |
| `src/BookStore.Api` | REST API, xác thực JWT, phân quyền, nhật ký kiểm toán |
| `src/BookStore.Shared` | Trang Razor, models và clients dùng chung |
| `src/BookStore.Domain` | Các thực thể nghiệp vụ |
| `src/BookStore.Infrastructure` | EF Core, SQL Server, migrations và dữ liệu demo |

## Lưu ý khi chia sẻ mã nguồn

- Không commit signing key, mật khẩu, token, file database, APK hoặc thư mục `bin`/`obj`.
- Mỗi thành viên nên chạy SQL Server và database riêng trên máy mình; bộ dữ liệu demo được tạo tự động.
- Thanh toán chuyển khoản là mô phỏng, chưa kết nối ngân hàng/cổng thanh toán.
- MAUI Android cần Android SDK Platform 36. Nên clone dự án vào đường dẫn ASCII, ví dụ `C:\src\TrangSach`, do AAPT2 có thể lỗi nếu đường dẫn chứa dấu tiếng Việt.

# Hướng dẫn cài đặt và làm việc nhóm — Trang Sách

Tài liệu dành cho thành viên nhận mã nguồn trên GitHub hoặc thư mục chia sẻ. Mỗi người tự chạy Web, API và SQL Server trên máy mình; không cần kết nối tới máy của người gửi dự án.

## 1. Cấu trúc và công nghệ

- `BookStore.Web`: Blazor Web App, gồm cửa hàng khách hàng và trang quản lý.
- `BookStore.Api`: ASP.NET Core REST API; đăng nhập JWT, phân quyền và nhật ký hoạt động.
- `BookStore`: .NET MAUI Blazor Hybrid, hiện nhắm Android cho ứng dụng khách hàng.
- `BookStore.Shared`: giao diện Razor, clients và models được dùng chung.
- `BookStore.Domain`: entity nghiệp vụ.
- `BookStore.Infrastructure`: EF Core, SQL Server, migrations và dữ liệu mẫu.
- `ERD.txt`: ERD Mermaid đầu vào. Bảng `SystemAuditLog` được thêm để lưu nhật ký vận hành.
- `Tài liệu kỹ thuật`: tài liệu mô tả chức năng đồ án.

Luồng chính: Web/Mobile → API → SQL Server. Web và API chạy riêng; điện thoại/emulator phải truy cập được API.

## 2. Phần mềm cần cài trên Windows

1. Git for Windows (nếu lấy mã bằng Git).
2. .NET SDK 10.x. SDK đã kèm .NET Runtime và ASP.NET Core Runtime cùng phiên bản nên không cần cài runtime riêng để phát triển/chạy Web và API. Kiểm tra:

   ```powershell
   dotnet --list-sdks
   dotnet --info
   ```

3. SQL Server Developer hoặc Express. Bật Database Engine, đăng nhập Windows Authentication và đảm bảo tài khoản Windows của thành viên được phép tạo database. SSMS chỉ là công cụ quản trị giao diện, không bắt buộc để chạy ứng dụng.
4. Chỉ để build Mobile Android: workload .NET MAUI/Android, Android SDK Platform 36, Android SDK Build-Tools 36 và Android Platform-Tools (`adb`), cùng JDK tương thích. Cài qua Visual Studio Installer (workload .NET MAUI) hoặc theo [hướng dẫn cài .NET MAUI chính thức](https://learn.microsoft.com/dotnet/maui/get-started/installation?view=net-maui-10.0). Chấp nhận Android SDK licenses. Điện thoại Android hoặc emulator chỉ cần nếu muốn cài/chạy app; không cần để build Web/API.

### Cái gì cần cài, cái gì tự tải?

| Thành phần | Cách có trên máy | Cần cho |
| --- | --- | --- |
| .NET SDK 10.x | Thành viên cài một lần từ Microsoft/Visual Studio Installer | Build và chạy mọi project .NET |
| NuGet packages (EF Core, SQL Server provider, OpenAPI, JWT, Blazor, logging, MAUI packages...) | Đã khai báo trong các `.csproj`; `dotnet restore` tự tải xuống NuGet cache | Các project tương ứng |
| `dotnet-ef` 10.0.5 | Công cụ local khai báo trong `.config/dotnet-tools.json`; `dotnet tool restore` cài vào cache công cụ | Chỉ khi tạo migration; không cần cài riêng để chạy ứng dụng |
| SQL Server Database Engine | Cài riêng, phải chạy được trên máy; API tự tạo database và áp migrations | Web/API |
| MAUI/Android workload, Android SDK, Build-Tools, Platform-Tools và JDK | Cài riêng bằng Visual Studio Installer hoặc công cụ Android; `dotnet restore` không cài thay các thành phần này | Chỉ build/chạy Mobile Android |
| SSMS, emulator, điện thoại | Tùy chọn theo công việc | Quản trị DB trực quan / chạy Mobile |

NuGet restore cần Internet ở lần đầu hoặc khi có package mới. Máy không có Internet chỉ chạy được nếu đã có đầy đủ package trong cache cục bộ. Với Android, cần tải và chấp nhận giấy phép riêng cho SDK/workload. Hướng dẫn giúp phát hiện thiếu dependency và chỉ cách cài, nhưng không thể đảm bảo mọi máy chạy được nếu thiếu quyền cài đặt, kết nối mạng, dung lượng hoặc Windows tương thích.

### Kiểm tra máy trước khi chạy

Sau khi cài .NET SDK, mở PowerShell mới và chạy:

```powershell
dotnet --list-sdks
dotnet --list-runtimes
```

Phải thấy SDK 10.x và runtime `Microsoft.NETCore.App 10.x`, `Microsoft.AspNetCore.App 10.x`. Tại thư mục gốc repository, chạy:

```powershell
dotnet workload list
dotnet tool restore
dotnet restore .\src\BookStore.sln
```

`dotnet restore` thành công là xác nhận các NuGet package của solution đã sẵn sàng. Nếu chỉ làm Web/API thì có thể bỏ qua workload Android và các bước Mobile; nếu làm Mobile mà `dotnet workload list` không có Android/MAUI hoặc thiếu `android.jar`, cài workload và Android SDK tương ứng trước khi build. Xem [yêu cầu cài MAUI/Android của Microsoft](https://learn.microsoft.com/dotnet/maui/get-started/installation?view=net-maui-10.0).

Không cần cài SQL Server vào thư mục dự án. Database được EF Core tạo trên SQL Server cục bộ khi API chạy lần đầu.

## 3. Lấy mã nguồn

### GitHub (khuyến nghị)

Clone dự án vào đường dẫn chỉ dùng ký tự ASCII. Tránh dấu tiếng Việt, ký tự đặc biệt và đường dẫn quá dài, đặc biệt khi build Android.

```powershell
New-Item -ItemType Directory -Force C:\src | Out-Null
Set-Location C:\src
git clone <URL-repository> TrangSach
Set-Location C:\src\TrangSach
```

Nếu người quản lý gửi ZIP thay vì GitHub, giải nén thành ví dụ `C:\src\TrangSach`. Không đưa `.git`, `bin`, `obj`, file `.apk` hoặc database lên Drive.

### Google Drive

Drive phù hợp để chia sẻ tài liệu, ảnh, bản ZIP hoặc bản phát hành. Không nên để nhiều người sửa mã nguồn đồng thời trong cùng thư mục Drive: đồng bộ file đang build và file `.git` dễ gây xung đột. Với thay đổi mã, dùng GitHub branches/pull requests; nếu chỉ được dùng Drive, phân công mỗi người một bản cục bộ và chỉ một người tổng hợp thay đổi.

## 4. Khôi phục package

Tại thư mục gốc dự án, sau khi đã cài .NET SDK:

```powershell
dotnet tool restore
dotnet restore .\src\BookStore.sln
```

Nếu restore lỗi mạng, kiểm tra proxy/Internet rồi chạy lại. Không xóa migrations để xử lý lỗi restore.

Lệnh restore chỉ tải thư viện NuGet đã khai báo vào cache người dùng; nó không cài .NET SDK, SQL Server, Android SDK/JDK hoặc Android workload. Khi clone bản mới hoặc có người thêm package, chạy lại `dotnet restore`.

## 5. Cấu hình SQL Server và khóa JWT

### Kết nối database

Cấu hình mặc định trong `src/BookStore.Api/appsettings.json` là SQL Server default instance trên máy local:

```text
Server=localhost;Database=TrangSachDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

Nếu dùng SQL Server Express, instance có thể là `localhost\SQLEXPRESS`. Lưu connection string riêng bằng User Secrets, không sửa thành thông tin đăng nhập thật trong file được commit:

```powershell
dotnet user-secrets set "ConnectionStrings:BookStore" "Server=localhost\SQLEXPRESS;Database=TrangSachDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true" --project .\src\BookStore.Api\BookStore.Api.csproj
```

Thay `localhost\SQLEXPRESS` theo tên instance trên máy. API dùng Windows Authentication; tài khoản Windows chạy API cần quyền tạo database ở lần đầu. Không phải chạy file SQL thủ công: khi API khởi động, nó tự tạo/cập nhật schema bằng toàn bộ EF Core migrations rồi seed dữ liệu ban đầu.

Trên một database mới, các thành viên nhận cùng bộ dữ liệu nền: 4 danh mục, 1 nhà xuất bản, 6 tác giả, 7 chức danh, 6 sách; dữ liệu báo cáo gồm 8 khách demo, 1 nhân viên demo, 1 nhà cung cấp demo, 60 đơn hàng và 25 phiếu nhập. Đơn/phiếu có mã `DEMO-*`; tồn kho được điều chỉnh theo các giao dịch mẫu. Tên, mã, số lượng và trạng thái mẫu được sinh nhất quán từ seeder. Ngày giờ giao dịch được tính lùi từ ngày người đó khởi tạo database lần đầu, nên biểu đồ có cùng phân bố tương đối nhưng ngày lịch cụ thể có thể khác giữa các thành viên. Seeder chạy lặp an toàn và không tạo lại bộ đơn demo nếu đã tìm thấy đơn `DEMO-HD-*`.

Để có bộ dữ liệu mẫu ban đầu, dùng database mới chưa có dữ liệu nghiệp vụ rồi chạy API theo mục 6. Nếu API không thể kết nối SQL Server hoặc không có quyền tạo database, bước khởi tạo sẽ lỗi và API không sẵn sàng. Database cục bộ của từng thành viên độc lập; đăng ký tài khoản khách và tạo tài khoản quản trị viên riêng trên từng máy. Hồ sơ demo do seeder thêm không phải tài khoản đăng nhập.

### JWT

Không có signing key cố định trong source. Trong Development, API tự tạo khóa ngẫu nhiên nếu không khai báo key; token đăng nhập cũ sẽ hết hiệu lực khi API khởi động lại. Muốn khóa ổn định giữa các lần chạy trên máy cá nhân, tạo User Secret một lần:

```powershell
$rng = [Security.Cryptography.RandomNumberGenerator]::Create()
$bytes = New-Object byte[] 64
$rng.GetBytes($bytes)
$jwtKey = [Convert]::ToBase64String($bytes)
dotnet user-secrets set "Jwt:SigningKey" $jwtKey --project .\src\BookStore.Api\BookStore.Api.csproj
$rng.Dispose()
Remove-Variable jwtKey,bytes,rng
```

User Secrets lưu ngoài thư mục repository. Không gửi key lên GitHub, Drive, ảnh chụp, chat nhóm hoặc file cấu hình. Môi trường ngoài Development bắt buộc phải cung cấp `Jwt:SigningKey` qua secret manager/environment variable; API sẽ dừng nếu thiếu key.

## 6. Chạy Web và API

Mở hai cửa sổ PowerShell tại thư mục gốc.

**Cửa sổ 1 — API:**

```powershell
dotnet run --project .\src\BookStore.Api\BookStore.Api.csproj
```

Chờ log `Now listening on: http://localhost:5017`. API sẽ kết nối SQL Server, migrate database và seed dữ liệu trước khi bắt đầu nghe cổng.

**Cửa sổ 2 — Web:**

```powershell
dotnet run --project .\src\BookStore.Web\BookStore.Web.csproj
```

Đường dẫn:

- Trang cửa hàng: <http://localhost:5245>
- Đăng nhập/đăng ký khách: <http://localhost:5245/account>
- Đăng nhập nhân viên: <http://localhost:5245/admin/login>
- Thiết lập quản trị viên đầu tiên: <http://localhost:5245/admin/setup>
- API OpenAPI JSON (Development): <http://localhost:5017/openapi/v1.json>

`http://localhost:5017/` không có trang chủ; 404 ở URL gốc là bình thường. Hãy mở `/openapi/v1.json` để xem mô tả API.

### Tạo tài khoản kiểm thử

- Tạo Quản trị viên đầu tiên ở `/admin/setup`. Endpoint chỉ hoạt động trong Development và chỉ tạo tài khoản Manager đầu tiên. Sau đó quản trị viên có thể tạo nhân viên, tài khoản và chức danh ở mục Nhân viên.
- Đăng ký khách hàng ở `/account`.
- Dữ liệu khách hàng/nhân viên/sách/đơn demo tạo bởi seeder không đồng nghĩa tài khoản đăng nhập demo. Mỗi người tạo tài khoản riêng trên database cục bộ của họ.

### Khu vực quản lý

- Quản trị viên: toàn quyền và xem Nhật ký hệ thống.
- Bán hàng: khách hàng và đơn hàng.
- Kho: nhập hàng/tồn kho.
- Danh mục sách: sách và danh mục.
- Khuyến mãi: chương trình và voucher.
- Kế toán/báo cáo: tổng quan và báo cáo.

Menu ẩn các khu vực không có quyền; API cũng kiểm tra quyền ở server.

## 7. Build Web/API

```powershell
dotnet build .\src\BookStore.Api\BookStore.Api.csproj --no-restore
dotnet build .\src\BookStore.Web\BookStore.Web.csproj --no-restore
```

API và Web cần chạy ở hai tiến trình riêng để Web gọi được API. Khi sửa API, dừng tiến trình đang giữ DLL trước khi build lại; nếu không có thể gặp lỗi copy file bị khóa.

## 8. Build và chạy Android

### Cấu hình thiết bị

- Android Emulator gọi API qua `http://10.0.2.2:5017/`.
- Điện thoại Android thật gọi API qua `http://localhost:5017/`; nối USB, bật USB debugging, sau khi API chạy thì mở PowerShell và chạy:

  ```powershell
  adb devices
  adb reverse tcp:5017 tcp:5017
  ```

  `adb reverse` phải chạy lại sau khi rút cáp hoặc restart ADB. Điện thoại phải giữ kết nối USB trong khi thử.

### Build

```powershell
dotnet build .\src\BookStore\BookStore.csproj -f net10.0-android
```

Nếu .NET chọn nhầm Android SDK, chỉ rõ SDK đã cài, ví dụ:

```powershell
dotnet build .\src\BookStore\BookStore.csproj -f net10.0-android "-p:AndroidSdkDirectory=$env:LOCALAPPDATA\Android\Sdk"
```

Nếu AAPT2 báo `APT2265` về ký tự không ASCII trong đường dẫn dự án, clone/giải nén lại sang đường dẫn ASCII như `C:\src\TrangSach` rồi restore/build tại đó. Trên máy chủ dự án hiện tại, SDK/workload Android và Platform 36 đã có; build tại đường dẫn chứa “Quản lý cửa hàng sách” đã gặp đúng lỗi APT2265.

Tìm APK sau build:

```powershell
Get-ChildItem .\src\BookStore\bin\Debug\net10.0-android -Filter *.apk -Recurse | Select-Object FullName,Length
```

Cài APK lên thiết bị đang kết nối:

```powershell
adb install -r "<đường-dẫn-APK-tìm-được>"
```

## 9. Làm việc với GitHub

1. Mỗi đầu việc tạo một branch ngắn, ví dụ `feature/order-cancel` hoặc `fix/admin-log-area`.
2. Commit thay đổi liên quan với thông điệp rõ: `feat: add staff audit filter`, `fix: map promotion audit area`.
3. Push branch, mở Pull Request và nhờ một thành viên khác xem diff trước khi merge.
4. Trước khi commit, kiểm tra không có `appsettings` chứa secret, token, file `.bak`/`.mdf`/`.ldf`, APK hoặc thư mục build:

   ```powershell
   git status --short
   git diff --check
   ```

5. Sau khi pull code mới, chạy lại `dotnet restore`; nếu có migration mới thì lần API startup sẽ áp dụng migration lên database cục bộ.
6. Không dùng chung database local để các thành viên cùng phát triển. Demo seeder ghi nhận đơn/phiếu có mã `DEMO-*`; không xóa hoặc sửa lẫn với dữ liệu kiểm thử cá nhân nếu cần tái tạo báo cáo.

## 10. Thêm migration

Khi thay đổi entity hoặc EF model, tại thư mục gốc:

```powershell
dotnet tool restore
dotnet tool run dotnet-ef migrations add TenMigration --project .\src\BookStore.Infrastructure\BookStore.Infrastructure.csproj --startup-project .\src\BookStore.Api\BookStore.Api.csproj --output-dir Persistence/Migrations
```

Commit cả file migration `.cs`, `.Designer.cs` và `BookStoreDbContextModelSnapshot.cs`. Không sửa schema database thủ công mà không có migration tương ứng. Không xóa migration đã được người khác áp dụng.

## 11. Giới hạn hiện tại và đánh giá sẵn sàng

- Web/API đã build trên môi trường hiện tại. MAUI Android chưa xác nhận build hoàn chỉnh tại thư mục hiện tại do AAPT2 từ chối đường dẫn có dấu; hãy clone vào `C:\src\TrangSach` rồi build lại trước khi phát hành APK.
- Chưa có project test tự động hoặc workflow CI trong solution. Nhóm cần kiểm thử thủ công các luồng đăng nhập, CRUD, hủy đơn/phiếu, tồn kho, báo cáo và quyền nhân viên sau khi clone.
- Chuyển khoản và thanh toán là mô phỏng; chưa có cổng thanh toán, email/SMS, triển khai cloud hoặc cấu hình production.
- Migrations và dữ liệu mẫu khởi tạo database local. Dữ liệu đơn hàng demo phục vụ biểu đồ, không phải dữ liệu bán hàng thật.
- `SystemAuditLog` là phần mở rộng phục vụ nhật ký, ngoài các entity nghiệp vụ ban đầu trong `ERD.txt`.

Vì vậy dự án có thể chia sẻ để nhóm tiếp tục làm việc và chạy Web/API local. Trước khi tuyên bố phát hành hoàn chỉnh, cần build Android từ đường dẫn ASCII và hoàn tất kiểm thử đầu-cuối/CI theo yêu cầu đồ án.

## 12. Xử lý lỗi thường gặp

| Triệu chứng | Cách xử lý |
| --- | --- |
| `localhost:5245` refused | Khởi động Web bằng lệnh ở mục 6. |
| Web báo API chưa kết nối | Khởi động API tại cổng 5017 trước Web; xác nhận SQL Server service đang chạy. |
| API báo không kết nối database | Kiểm tra SQL Server service, instance trong connection string, Windows Authentication và quyền tạo database. |
| `Could not find android.jar` | Cài Android SDK Platform 36, kiểm tra SDK path và truyền `AndroidSdkDirectory` nếu cần. |
| AAPT2 `APT2265` | Chuyển repository sang thư mục ASCII như `C:\src\TrangSach`. |
| DLL đang bị process sử dụng | Dừng API/Web cũ rồi build lại. |
| Đăng nhập bị từ chối sau API restart | Nếu không cấu hình User Secret, khóa Development được tạo ngẫu nhiên và token cũ mất hiệu lực; đăng nhập lại hoặc thiết lập secret ổn định. |
| Điện thoại báo không kết nối API | Bật API, cắm USB, xác nhận `adb devices`, chạy `adb reverse tcp:5017 tcp:5017`. |
| `dotnet` không nhận lệnh hoặc thiếu `Microsoft.AspNetCore.App` | Cài .NET SDK 10.x, mở PowerShell mới rồi kiểm tra `dotnet --info`. |
| `NU1301`, lỗi tải package hoặc restore thất bại | Kiểm tra Internet/proxy/NuGet source rồi chạy `dotnet restore .\src\BookStore.sln` lại. |
| `NETSDK1147` hoặc thiếu workload | Chạy `dotnet workload list`; cài .NET MAUI/Android workload theo mục 2. |
| `Java SDK/JDK not found` | Cài/chọn JDK tương thích trong cấu hình MAUI/Android của Visual Studio; kiểm tra đường dẫn SDK/JDK. |


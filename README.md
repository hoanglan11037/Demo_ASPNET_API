README - Demo ASP.NET API

Yêu cầu hệ thống
 - .NET SDK 8.0
 - Visual Studio 2022 hoặc Visual Studio Code
 - SQL Server

1. Clone dự án
   Sử dụng Git để clone dự án về máy tính
git clone https://github.com/yourusername/your-repo-name.git
cd your-repo-name

2. Cài đặt các gói cần thiết
dotnet restore

3. Cấu hình cơ sở dữ liệu
- Mở tệp appsettings.json và chỉnh sửa chuỗi kết nối đến cơ sở dữ liệu SQL Server
"ConnectionStrings": {
  "DemoConnection": "Data Source=DESKTOP-LDKNLOS\\SQLEXPRESS;Initial Catalog=DemoDatabase;Integrated Security=True;Encrypt=False;Trust Server Certificate=True"
}

- Tạo bảng và áp dụng migration để tạo bảng trong cơ sở dữ liệu
Add-Migration Initial
Update-Database

4. Chạy ứng dụng
dotnet run

5.Endpoint API
Kiểm tra các endpoint của API bằng cách sử dụng Postman. Mặc định, API sẽ chạy trên http://localhost:5000

Endpoint:
- Product
  + GET api/v1/Product/GetProducts - Lấy danh sách sản phẩm
  + POST api/v1/Product/AddProduct - Thêm sản phẩm mới
  + PUT api/v1/Product/UpdateProduct - Cập nhật thông tin sản phẩm
  + DELETE api/v1/Product/RemoveProduct - Xóa sản phẩm theo ID
 
  + POST api/v1/Account/Login - Đăng nhập
  + POST api/v1/Account/Register - Đăng ký
  + GET api/v1/Account/GetUsers - Lấy danh sách người dùng
  + PUT api/v1/Account/UpdateUser - cập nhật thông tin người dùng
  + DELETE api/v1/Account/RemoveUser - Xóa người dùng theo ID

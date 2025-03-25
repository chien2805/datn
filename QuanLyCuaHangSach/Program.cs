using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using QuanLyCuaHangSach.Context;
using QuanLyCuaHangSach.Models.Momo;
using QuanLyCuaHangSach.Services;



var builder = WebApplication.CreateBuilder(args);
//Momo API Payment

// 🛠️ Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

builder.Services.Configure<MomoOptionModel>(builder.Configuration.GetSection("MomoAPI"));
builder.Services.AddScoped<IMomoService, MomoService>();
// ✅ Cấu hình xác thực bằng Cookie Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/DangNhap/Index"; // Trang đăng nhập
        options.AccessDeniedPath = "/Home/AccessDenied"; // Trang bị chặn quyền
    });

// ✅ Cấu hình Entity Framework với SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ Cấu hình Session (Lưu trữ thông tin đăng nhập, giỏ hàng, v.v.)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session tồn tại
    options.Cookie.HttpOnly = true; // Bảo mật cookie (chỉ dùng HTTP)
    options.Cookie.IsEssential = true; // Cookie luôn được lưu
});



var app = builder.Build();




// ✅ Kích hoạt Session
app.UseSession();

// 🔥 Middleware xử lý lỗi (nếu không ở môi trường Development)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Trang lỗi chung
    app.UseHsts(); // Tăng cường bảo mật với HSTS (HTTPS Strict Transport Security)
}

// ✅ Middleware xử lý HTTP & Static Files
app.UseHttpsRedirection(); // Chuyển hướng HTTP sang HTTPS
app.UseStaticFiles(); // Cho phép truy cập tệp tĩnh (CSS, JS, hình ảnh)

// ✅ Cấu hình định tuyến
app.UseRouting();

// ✅ Bắt buộc xác thực trước khi kiểm tra quyền truy cập
app.UseAuthentication();
app.UseAuthorization();

// ✅ Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=TrangChu}/{action=Index}/{id?}");

// 🔥 Chạy ứng dụng
app.Run();

var builder = WebApplication.CreateBuilder(args);

// 🔹 Thêm dịch vụ Session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Thời gian session tồn tại 30 phút
    options.Cookie.HttpOnly = true; // Chỉ truy cập qua HTTP (bảo mật hơn)
    options.Cookie.IsEssential = true; // Đảm bảo session hoạt động ngay cả khi tắt cookie
});

// 🔹 Thêm dịch vụ MVC (Controllers & Views)
builder.Services.AddControllersWithViews();

var app = builder.Build();

// 🔹 Kích hoạt Session Middleware
app.UseSession();

app.UseRouting();
app.UseAuthorization();

// 🔹 Cấu hình Route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Cart}/{action=Index}/{id?}"
);

app.Run();

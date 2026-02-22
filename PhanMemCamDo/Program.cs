using Microsoft.EntityFrameworkCore;
using PhanMemCamDo.Data;

var builder = WebApplication.CreateBuilder(args);

// --- 1. KHAI BÁO DỊCH VỤ (SERVICES) ---

// Thêm dịch vụ Session (Để lưu trạng thái đăng nhập)
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // Đăng nhập giữ trong 60 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm dịch vụ truy cập HttpContext (Để gọi Session từ View/Controller dễ hơn)
builder.Services.AddHttpContextAccessor();

// Thêm Controllers và Views
builder.Services.AddControllersWithViews();

// Kết nối SQL Server
builder.Services.AddDbContext<PawnShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// --- 2. CẤU HÌNH PIPELINE (MIDDLEWARE) ---

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

// Xử lý file tĩnh (CSS/JS)
app.UseRouting();

app.UseAuthorization();

// --- QUAN TRỌNG: KÍCH HOẠT SESSION ---
// (Phải đặt SAU UseRouting và TRƯỚC MapControllerRoute)
app.UseSession();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
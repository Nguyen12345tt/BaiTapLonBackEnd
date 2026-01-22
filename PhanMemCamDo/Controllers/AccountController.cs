using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Để dùng AnyAsync
using PhanMemCamDo.Data;
using PhanMemCamDo.Models.Entities;
using PhanMemCamDo.Models.ViewModels;
//using System.Security.Cryptography; // Để mã hóa mật khẩu
using System.Text;

namespace PhanMemCamDo.Controllers
{
    public class AccountController(PawnShopDbContext context) : Controller
    {
        // 1. Hiện Form Đăng Ký (GET)
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // 2. Xử lý khi bấm nút Đăng Ký (POST)
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem Username đã có người dùng chưa
                bool isExist = await context.Users.AnyAsync(u => u.Username == model.Username);
                if (isExist)
                {
                    ModelState.AddModelError("Username", "Tên đăng nhập này đã có người dùng!");
                    return View(model);
                }

                // Tạo người dùng mới
                var newUser = new User
                {
                    Username = model.Username,
                    FullName = model.FullName,
                    Password = model.Password,
                    // MÃ HÓA MẬT KHẨU (An toàn tuyệt đối)
                    //PasswordHash = HashPassword(model.Password ?? ""),
                    Role = Models.Enums.UserRole.Staff, // Mặc định đăng ký mới là Nhân viên
                    IsActive = true
                };

                context.Users.Add(newUser);
                await context.SaveChangesAsync(); // Lưu vào SQL

                // Đăng ký xong thì chuyển sang trang Đăng nhập (Login)
                return RedirectToAction("Login", "Account");
            }

            return View(model);
        }

        // Hàm phụ: Mã hóa mật khẩu thành chuỗi ký tự loằng ngoằng (MD5/SHA256)
        //private static string HashPassword(string password)
        //{
            //var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            //var builder = new StringBuilder();
            //foreach (var b in bytes)
            //{
             //   builder.Append(b.ToString("x2"));
            //}
            //return builder.ToString();
        //}

        // 3. Hiện Form Đăng Nhập (GET)
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // 4. Xử lý Đăng Nhập (POST) - CẬP NHẬT
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Username == model.Username);
                if (user == null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập không tồn tại!");
                    return View(model);
                }

                //string inputHash = HashPassword(model.Password ?? "");
                //if (user.PasswordHash != inputHash)
                if (user.Password != model.Password)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng!");
                    return View(model);
                }

                // --- ĐOẠN MỚI THÊM: LƯU SESSION ---
                // Lưu tên và quyền hạn vào bộ nhớ tạm
                HttpContext.Session.SetString("Username", user.Username ?? "");
                HttpContext.Session.SetString("Role", user.Role.ToString());

                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        // 5. Đăng Xuất (Thêm mới)
        public IActionResult Logout()
        {
            // Xóa sạch bộ nhớ phiên làm việc
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}

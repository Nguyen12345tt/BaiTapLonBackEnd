using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhanMemCamDo.Data;
using PhanMemCamDo.Models.Entities;
using PhanMemCamDo.Models.Enums;

namespace PhanMemCamDo.Controllers
{
    public class PawnContractsController(PawnShopDbContext context) : Controller
    {
        // 1. DANH SÁCH HỢP ĐỒNG (INDEX)
        public async Task<IActionResult> Index(string? searchString)
        {
            // 1. Tạo câu truy vấn (Chưa chạy xuống Database ngay)
            var contracts = context.PawnContracts
                .Include(p => p.Asset)
                    .ThenInclude(a => a!.AssetCategory) // Load danh mục
                .Include(p => p.Customer)
                .AsQueryable();

            // 2. Lọc tìm kiếm
            if (!string.IsNullOrEmpty(searchString))
            {
                // Thêm (searchString ?? "") để chắc chắn không bị Null
                contracts = contracts.Where(s =>
                    (s.ContractCode != null && s.ContractCode.Contains(searchString ?? "")) ||
                    (s.Customer != null && s.Customer.FullName != null && s.Customer.FullName.Contains(searchString ?? ""))
                );
            }

            // --- 3. TÍNH TOÁN SỐ LIỆU (Dùng Async để không chặn luồng) ---

            // Tổng vốn đang vay (Dùng SumAsync)
            // Lưu ý: PawnAmount là nullable nên phải check null (?? 0)
            ViewBag.VonDangVay = await contracts
                .Where(c => c.Status == ContractStatus.Active)
                .SumAsync(c => c.PawnAmount);

            // Số hợp đồng đang chạy (Dùng CountAsync)
            ViewBag.DangChay = await contracts
                .CountAsync(c => c.Status == ContractStatus.Active);

            // Số hợp đồng sắp đến hạn (3 ngày tới)
            var today = DateTime.Now.Date;
            var threeDaysLater = today.AddDays(3);
            ViewBag.SapDenHan = await contracts
                .CountAsync(c => c.Status == ContractStatus.Active
                                 && c.EndDate >= today
                                 && c.EndDate <= threeDaysLater);

            // Lãi dự kiến (Dùng SumAsync)
            ViewBag.LaiDuKien = await contracts
                .Where(c => c.Status == ContractStatus.Active)
                .SumAsync(c => (c.PawnAmount) * (c.InterestRate) / 100);

            // 4. Lấy danh sách hiển thị (Dùng ToListAsync)
            var resultList = await contracts
                .OrderByDescending(c => c.StartDate)
                .ToListAsync(); // <--- Đây là chỗ quan trọng để fix lỗi GetAwaiter

            return View(resultList);
        }

        // 2. XEM CHI TIẾT (DETAILS)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pawnContract = await context.PawnContracts
                .Include(p => p.Asset)
                    .ThenInclude(a => a!.AssetCategory)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pawnContract == null) return NotFound();

            return View(pawnContract);
        }

        // 3. TẠO HỢP ĐỒNG MỚI (GET)
        public async Task<IActionResult> Create(int? customerId)
        {
            if (customerId != null)
            {
                var khachQuen = await context.Customers.FindAsync(customerId);
                if (khachQuen != null) ViewBag.KhachQuen = khachQuen;
            }

            ViewData["CustomerId"] = new SelectList(context.Customers, "Id", "FullName");
            ViewData["AssetCategoryId"] = new SelectList(context.AssetCategories, "Id", "Name");

            var configLaiSuat = await context.SystemConfigs.FirstOrDefaultAsync(x => x.ConfigKey == "LaiSuatMacDinh");
            ViewBag.LaiSuatGoiY = configLaiSuat?.ConfigValue ?? "3";

            return View();
        }

        // 4. XỬ LÝ TẠO HỢP ĐỒNG (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PawnContract pawnContract,
                                                string TenTaiSan, string MoTaTaiSan,
                                                string TenKhach, string SDT, string CCCD,
                                                int AssetCategoryId)
        {
            if (string.IsNullOrEmpty(pawnContract.ContractCode))
            {
                pawnContract.ContractCode = "HD" + DateTime.Now.ToString("yyyyMMddHHmmss");
                ModelState.Remove("ContractCode");
            }

            ModelState.Remove("Customer");
            ModelState.Remove("Asset");
            ModelState.Remove("CustomerId");
            ModelState.Remove("AssetId");

            if (!ModelState.IsValid)
            {
                return Content("Lỗi nhập liệu: " + string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
            }

            try
            {
                var existingCustomer = await context.Customers.FirstOrDefaultAsync(c => c.IdentityCard == CCCD);
                if (existingCustomer != null)
                {
                    pawnContract.CustomerId = existingCustomer.Id;
                    existingCustomer.FullName = TenKhach;
                    existingCustomer.PhoneNumber = SDT;
                    context.Update(existingCustomer);
                }
                else
                {
                    var newCustomer = new Customer { FullName = TenKhach, PhoneNumber = SDT, IdentityCard = CCCD, Address = "Chưa cập nhật" };
                    context.Customers.Add(newCustomer);
                    await context.SaveChangesAsync();
                    pawnContract.CustomerId = newCustomer.Id;
                }

                var newAsset = new Asset { AssetName = string.IsNullOrEmpty(TenTaiSan) ? "Tài sản chưa tên" : TenTaiSan, Description = MoTaTaiSan ?? "", AssetCategoryId = AssetCategoryId };
                context.Assets.Add(newAsset);
                await context.SaveChangesAsync();
                pawnContract.AssetId = newAsset.Id;

                if (pawnContract.EndDate == DateTime.MinValue || pawnContract.EndDate <= pawnContract.StartDate)
                    pawnContract.EndDate = pawnContract.StartDate.AddMonths(1);

                pawnContract.Status = ContractStatus.Active;
                context.Add(pawnContract);
                await context.SaveChangesAsync();

                await GhiNhatKy("TẠO MỚI", $"Tạo HĐ {pawnContract.ContractCode} - Khách: {TenKhach} - Số tiền: {pawnContract.PawnAmount:N0}đ");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content($"🔥 LỖI HỆ THỐNG: {ex.Message} - {ex.InnerException?.Message}");
            }
        }

        // 5. SỬA HỢP ĐỒNG (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var pawnContract = await context.PawnContracts.Include(a => a.Asset).FirstOrDefaultAsync(x => x.Id == id);
            if (pawnContract == null) return NotFound();

            ViewData["CustomerId"] = new SelectList(context.Customers, "Id", "FullName", pawnContract.CustomerId);
            ViewData["AssetCategoryId"] = new SelectList(context.AssetCategories, "Id", "Name", pawnContract.Asset?.AssetCategoryId);
            return View(pawnContract);
        }

        // 6. XỬ LÝ SỬA (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PawnContract pawnContract, int AssetCategoryId, string TenTaiSan, string MoTaTaiSan)
        {
            if (id != pawnContract.Id) return NotFound();
            ModelState.Remove("Customer");
            ModelState.Remove("Asset");

            if (ModelState.IsValid)
            {
                try
                {
                    context.Update(pawnContract);
                    var assetToUpdate = await context.Assets.FindAsync(pawnContract.AssetId);
                    if (assetToUpdate != null)
                    {
                        assetToUpdate.AssetName = TenTaiSan;
                        assetToUpdate.Description = MoTaTaiSan;
                        assetToUpdate.AssetCategoryId = AssetCategoryId;
                        context.Update(assetToUpdate);
                    }
                    await context.SaveChangesAsync();
                    await GhiNhatKy("CẬP NHẬT", $"Sửa HĐ {pawnContract.ContractCode}. Trạng thái mới: {pawnContract.Status}");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PawnContractExists(pawnContract.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(pawnContract);
        }

        // 7. XÓA HỢP ĐỒNG (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var pawnContract = await context.PawnContracts.Include(p => p.Asset).Include(p => p.Customer).FirstOrDefaultAsync(m => m.Id == id);
            if (pawnContract == null) return NotFound();
            return View(pawnContract);
        }

        // 8. XÁC NHẬN XÓA (POST)
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pawnContract = await context.PawnContracts.Include(p => p.Customer).FirstOrDefaultAsync(m => m.Id == id);
            if (pawnContract != null)
            {
                await GhiNhatKy("XÓA HỢP ĐỒNG", $"Đã xóa HĐ {pawnContract.ContractCode} của khách {pawnContract.Customer?.FullName}");
                context.PawnContracts.Remove(pawnContract);
                await context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        // KIỂM TRA HỢP ĐỒNG TỒN TẠI
        private bool PawnContractExists(int id)
        {
            return context.PawnContracts.Any(e => e.Id == id);
        }

        // GHI NHẬT KÝ HÀNH ĐỘNG
        private async Task GhiNhatKy(string hanhDong, string chiTiet)
        {
            var log = new ActionLog { ActionName = hanhDong, Description = chiTiet, EntityName = "Hợp Đồng", Timestamp = DateTime.Now, UserName = "Admin" };
            context.ActionLogs.Add(log);
            await context.SaveChangesAsync();
        }
    }
}
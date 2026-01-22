# BÁO CÁO BÀI TẬP LỚN: XÂY DỰNG WEBSITE QUẢN LÝ CẦM ĐỒ

> **Môn học:** [Thiết kế, lập trình Back-end]
> **Giảng viên hướng dẫn:** [Thầy Tạ Chí Hiếu]
> **Nhóm thực hiện:** [Nhóm 3]
> **Lớp:** [CNTT 18-09]

---

## 👥 Thành Viên Nhóm

| STT | Họ và Tên | Mã SV | Nhiệm Vụ Chính |
|:---:|:---|:---:|:---|
| 1 | **[Nguyễn Thành Nguyên]** ⭐️ | ... | Thiết kế Database theo code, Module Hợp Đồng, Thu tiền, Đăng nhập/Đăng ký |
| 2 | **[Đỗ Văn Vinh]** | ... | Module Tài Sản, Giao diện (Frontend), Báo cáo |
| 3 | **[Nguyễn Hồng Sơn]** | ... | Module Sổ Quỹ, Thông báo, Báo cáo |

## 1. Giới Thiệu Đề Tài

**PhanMemCamDo** là một ứng dụng web được xây dựng nhằm hỗ trợ việc quản lý hoạt động kinh doanh cầm đồ. Đề tài tập trung vào việc xử lý các nghiệp vụ cơ bản như: quản lý hồ sơ khách hàng, tạo hợp đồng cầm cố, tính lãi suất, quản lý kho tài sản và kiểm soát dòng tiền thu chi.

Mục tiêu của đồ án là áp dụng kiến thức **ASP.NET Core MVC** và **Entity Framework Core** để giải quyết bài toán thực tế.

---

## 2. Các Chức Năng Đã Hoàn Thiện

Nhóm chúng em đã xây dựng (chưa hoàn chỉnh lắm) các chức năng sau:

### 🏠 Quản Lý Hợp Đồng (Chức năng chính)
* Tạo mới hợp đồng cầm đồ (Tính toán ngày hết hạn, lãi suất).
* **Thu tiền:** Hỗ trợ thu tiền lãi (có tùy chọn gia hạn ngày) và thu tiền gốc (trừ dần vào nợ).
* **Thanh lý/Chuộc đồ:** Xử lý kết thúc hợp đồng khi khách trả đủ tiền hoặc quá hạn.

### 📦 Quản Lý Kho Tài Sản
* Thêm/Sửa/Xóa tài sản.
* **Ràng buộc dữ liệu:** Hệ thống chặn không cho xóa các tài sản đang nằm trong hợp đồng chưa tất toán (Sử dụng `try-catch` bắt lỗi DbUpdateException).

### 💰 Quản Lý Sổ Quỹ (Cash Flow)
* Tự động ghi lại lịch sử dòng tiền khi thực hiện thu/chi.
* Thống kê dòng tiền vào (Thu lãi/gốc) và dòng tiền ra (Chi cầm đồ/chi phí khác).

### 🔔 Tiện Ích Khác
* **Thông báo tự động:** Hệ thống tự động quét và cảnh báo các hợp đồng sắp đến hạn thanh toán trên Dashboard.
* **Giao diện:** Sử dụng SweetAlert2 để hiện thông báo xác nhận xóa và kết quả thao tác đẹp mắt.

---

## 3. Công Nghệ Sử Dụng

* **Nền tảng:** .NET 9.0
* **Mô hình:** ASP.NET Core MVC
* **Cơ sở dữ liệu:** SQL Server (Code-First Migration)
* **Giao diện:** Bootstrap 5, JavaScript, jQuery, HTML
* **Thư viện hỗ trợ:** SweetAlert2 (Popup)

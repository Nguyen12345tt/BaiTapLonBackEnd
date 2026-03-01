# BÁO CÁO BÀI TẬP LỚN: XÂY DỰNG WEBSITE QUẢN LÝ CẦM ĐỒ

> **Môn học:** Thiết kế, lập trình Back-end

> **Giảng viên hướng dẫn:** Thầy Tạ Chí Hiếu

> **Nhóm thực hiện:** Nhóm 13

> **Lớp:** CNTT 18-09

---

## 👥 Thành Viên Nhóm

| STT | Họ và Tên | Mã SV | Nhiệm Vụ Chính |
|:---:|:---|:---:|:---|
| 1 | **Nguyễn Thành Nguyên**  | 1871020437 | Thiết kế DB, Module Hợp Đồng, Thu tiền |
| 2 | **Đỗ Văn Vinh** ⭐️| 1871020660 | Module Tài Sản, Giao diện (Frontend) |
| 3 | **Nguyễn Hồng Sơn** | 1871020509 | Module Sổ Quỹ, Thông báo, Báo cáo |

---

## 1. Giới Thiệu Đề Tài

**PhanMemCamDo** là hệ thống quản lý nghiệp vụ cầm đồ, được xây dựng nhằm số hóa quy trình quản lý hồ sơ, tài sản và dòng tiền. Đồ án tập trung giải quyết các bài toán thực tế như tính lãi suất tự động, kiểm soát nợ xấu và ràng buộc toàn vẹn dữ liệu kho hàng.

---

## 2. Các Chức Năng Đã Hoàn Thiện

Em đã áp dụng kiến thức **ASP.NET Core MVC** để xử lý các nghiệp vụ sau:

### 🏠 Nghiệp Vụ Hợp Đồng (Pawn Contracts)
* **Tự động hóa:** Hệ thống tự động tính ngày hết hạn và số tiền lãi dự kiến dựa trên lãi suất cấu hình.
* **Xử lý thu tiền linh hoạt:**
    * Hỗ trợ đóng lãi (có tùy chọn **gia hạn thêm ngày** ngay trên form thu tiền).
    * Hỗ trợ trả bớt gốc (hệ thống tự trừ dư nợ).
    * Xử lý chuộc đồ và thanh lý hợp đồng.

### 📦 Quản Lý Kho & Ràng Buộc Dữ Liệu
* Quản lý trạng thái tài sản chi tiết.
* **Xử lý ngoại lệ (Exception Handling):** Sử dụng `try-catch` để bắt lỗi `DbUpdateException`, ngăn chặn việc xóa nhầm các tài sản đang nằm trong hợp đồng chưa tất toán.

### 🔔 Tiện Ích & Trải Nghiệm Người Dùng (UX)
* **Hệ thống thông báo (Notification):** Tự động quét Database và cảnh báo các hợp đồng sắp đến hạn thanh toán trên Dashboard.
* **Giao diện tương tác:** Tích hợp thư viện **SweetAlert2** để hiển thị các popup xác nhận xóa, thông báo lỗi/thành công đẹp mắt thay vì dùng `alert()` mặc định.

### 💰 Quản Lý Dòng Tiền (Cash Flow)
* Tự động ghi log lịch sử thu/chi vào Sổ Quỹ mỗi khi phát sinh giao dịch tài chính.

---

## 3. Công Nghệ Sử Dụng

* **Framework:** .NET 8.0 (ASP.NET Core MVC)
* **Database:** SQL Server (Entity Framework Core - Code First)
* **Frontend:** Bootstrap 5, JavaScript (ES6), jQuery.
* **Thư viện:** SweetAlert2, FontAwesome.


**Em xin cảm ơn Thầy/Cô đã xem xét!**

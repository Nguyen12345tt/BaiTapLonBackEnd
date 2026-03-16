# BÁO CÁO BÀI TẬP LỚN

## Đề tài: Xây dựng Hệ thống Quản lý Nghiệp vụ Cầm Đồ (Pawn Shop Management)

---

## PHẦN I: MỞ ĐẦU

### 1. Tính cấp thiết của đề tài
Hiện nay, phần lớn các cửa hàng cầm đồ quy mô vừa và nhỏ vẫn quản lý sổ sách theo cách truyền thống hoặc dùng Excel, dẫn đến nhiều bất cập:
- **Vấn đề quản lý tài sản:** Khó theo dõi tình trạng tài sản (đang cầm, đã thanh lý, đã chuộc), dễ xảy ra thất thoát.
- **Vấn đề tính toán:** Việc tính lãi suất thủ công (theo ngày, tuần, tháng) dễ dẫn đến sai sót, nhầm lẫn gây thiệt hại cho cả chủ tiệm và khách hàng.
- **Vấn đề cảnh báo:** Không có hệ thống nhắc nhở tự động các hợp đồng sắp đến hạn hoặc đã quá hạn, làm giảm hiệu quả thu hồi vốn.
- **Vấn đề tổng hợp:** Khó khăn trong việc thống kê dòng tiền (thu/chi), lợi nhuận và đánh giá tình hình kinh doanh tổng thể.

**Giải pháp:** Xây dựng một hệ thống phần mềm quản lý tập trung, tự động hóa quy trình tính lãi, cảnh báo đáo hạn và quản lý chặt chẽ vòng đời của hợp đồng cầm đồ.

### 2. Mục tiêu đề tài
1. **Thiết kế cơ sở dữ liệu** tối ưu với 10 bảng, đảm bảo ràng buộc chặt chẽ giữa Khách hàng, Tài sản và Hợp đồng.
2. **Xây dựng ứng dụng Web** hoàn chỉnh với giao diện thân thiện, dễ sử dụng.
3. **Triển khai logic tính toán** lãi suất linh hoạt (theo nhiều hình thức) và tự động cập nhật trạng thái hợp đồng.
4. **Phát triển hệ thống Dashboard** báo cáo trực quan tình hình kinh doanh và cảnh báo đáo hạn.
5. **Đảm bảo tính bảo mật** thông qua cơ chế phân quyền và quản lý phiên đăng nhập (Session).

### 3. Phạm vi công việc
- **Phạm vi chức năng:** Hỗ trợ đầy đủ luồng nghiệp vụ: Tạo hợp đồng -> Thu lãi/Trả bớt gốc -> Chuộc đồ/Thanh lý.
- **Phạm vi công nghệ:** C# .NET 8.0, ASP.NET Core MVC, SQL Server, Entity Framework Core, Bootstrap 5.

---

## PHẦN II: PHÂN TÍCH YÊU CẦU HỆ THỐNG

### 1. Phân tích đối tượng sử dụng
Trong khuôn khổ dự án này, hệ thống tập trung phục vụ một nhóm người dùng chính là **Nhân viên/Chủ tiệm cầm đồ** (Admin) với các nhu cầu:
- Quản lý danh mục (Khách hàng, Tài sản).
- Lập và theo dõi Hợp đồng cầm đồ.
- Ghi nhận dòng tiền (Thu/Chi).
- Xem báo cáo thống kê.

### 2. Phân tích yêu cầu chức năng chính

#### **Nhóm chức năng A: Quản lý Danh mục cơ bản**
- Quản lý thông tin Khách hàng (CRUD).
- Quản lý thông tin Tài sản cầm cố (Phân loại theo danh mục: Điện thoại, Xe máy...).
- Quản lý tài khoản hệ thống (Đăng nhập, Đổi mật khẩu).

#### **Nhóm chức năng B: Nghiệp vụ Hợp đồng & Tính toán**
- Tạo mới hợp đồng cầm đồ (Liên kết Khách hàng - Tài sản).
- Cấu hình hình thức tính lãi (Theo ngày, tuần, tháng).
- Cập nhật trạng thái hợp đồng (Đang cầm, Đã chuộc, Quá hạn, Thanh lý).
- Ràng buộc an toàn: Không cho phép xóa Khách hàng/Tài sản khi đang có hợp đồng tồn tại (Active).

#### **Nhóm chức năng C: Dòng tiền & Cảnh báo**
- Ghi nhận lịch sử thanh toán (Đóng lãi, Trả bớt gốc).
- Bảng điều khiển (Dashboard) thống kê tổng vốn đang cho vay, hợp đồng đang chạy.
- Cảnh báo tự động các hợp đồng hết hạn trong 3 ngày tới hoặc đã quá hạn.
- Biểu đồ trực quan (Chart.js) thể hiện tỷ lệ các trạng thái hợp đồng.

---

## PHẦN III: THIẾT KẾ CƠ SỞ DỮ LIỆU (DATABASE DESIGN)

Hệ thống sử dụng phương pháp **Code-First** của Entity Framework Core để thiết kế và quản lý cơ sở dữ liệu. Tổng cộng có 10 thực thể (bảng) được chia thành 4 nhóm nghiệp vụ chính, đảm bảo tính chuẩn hóa (Normalization) và không dư thừa dữ liệu.

### 1. Nhóm Core (Nghiệp vụ Cầm đồ Cốt lõi)
* **`Customers` (Khách hàng):** Lưu trữ thông tin cá nhân (Họ tên, CCCD, Số điện thoại, Địa chỉ) để định danh người cầm cố.
* **`Assets` (Tài sản):** Quản lý chi tiết các món đồ được mang đi cầm cố.
* **`PawnContracts` (Hợp đồng Cầm đồ):** Bảng trung tâm của hệ thống. Lưu trữ thông tin số tiền vay (`PawnAmount`), ngày bắt đầu/kết thúc, và liên kết khóa ngoại tới `Customers` và `Assets`.
    * *Ràng buộc an toàn:* Sử dụng `DeleteBehavior.Restrict` để khóa việc xóa Khách hàng/Tài sản nếu hợp đồng vẫn đang tồn tại.

### 2. Nhóm Dòng tiền & Tài chính (Cashflow)
* **`PaymentHistories` (Lịch sử thanh toán):** Ghi chép chi tiết từng lần khách hàng đến đóng tiền (Tiền lãi, Trả bớt gốc). Sử dụng kiểu dữ liệu `decimal(18,0)` để đảm bảo độ chính xác tuyệt đối cho tiền tệ VNĐ.
* **`CashFlows` (Sổ quỹ):** Theo dõi tổng luân chuyển dòng tiền ra/vào của toàn bộ cửa hàng theo thời gian thực.

### 3. Nhóm Tiện ích & Phân loại
* **`AssetCategories` (Danh mục Tài sản):** Phân loại tài sản (Xe máy, Ô tô, Điện thoại, Giấy tờ...). Cung cấp sẵn *Seed Data* cơ bản ngay khi khởi tạo hệ thống.
* **`Notifications` (Thông báo):** Lưu trữ các cảnh báo hệ thống (như thông báo hợp đồng sắp đáo hạn).
* **`SystemConfigs` (Cấu hình hệ thống):** Các tham số cài đặt thay đổi linh hoạt cho tiệm cầm đồ.

### 4. Nhóm Quản trị & Phân quyền
* **`Users` (Người dùng):** Tài khoản đăng nhập nội bộ (Admin/Nhân viên), tích hợp phân quyền (Role).
* **`ActionLogs` (Nhật ký hoạt động):** Theo dõi dấu vết (Audit Trail) các thao tác quan trọng để quy trách nhiệm khi xảy ra sai sót.

### 5. Các Enum Trạng thái cốt lõi
* **`ContractStatus`:** Quản lý vòng đời hợp đồng với 4 trạng thái: `Active` (Đang hoạt động), `Redeemed` (Đã chuộc), `Overdue` (Quá hạn) và `Liquidated` (Đã thanh lý).
* **`InterestType`:** Quy định hình thức tính lãi suất động: `TheoNgay`, `TheoTuan`, `TheoThang`.

---

## PHẦN IV: THIẾT KẾ KIẾN TRÚC & LOGIC XỬ LÝ

### 1. Kiến trúc Hệ thống
Dự án được xây dựng theo mô hình **MVC (Model - View - Controller)** chuẩn mực của ASP.NET Core:
* **Models:** Chứa các Entities ánh xạ với Database và các ViewModels (`DashboardVM`, `ChangePasswordVM`) dùng để vận chuyển dữ liệu tối ưu ra View.
* **Views:** Sử dụng Razor Syntax (`.cshtml`) kết hợp Bootstrap 5. Tách biệt thành các layout tái sử dụng (`_Layout.cshtml`).
* **Controllers:** Chịu trách nhiệm điều hướng và xử lý nghiệp vụ chính.

### 2. Luồng xử lý Logic cốt lõi (Core Logic)

#### A. Logic Bảng điều khiển (Dashboard)
Được đặt tại `HomeController`, xử lý khối lượng dữ liệu lớn qua các câu truy vấn LINQ bất đồng bộ (`Async/Await`):
* Tính tổng vốn đang cho vay bằng cách `Sum` tiền gốc của các hợp đồng có trạng thái `Active`.
* Truy vấn lọc các hợp đồng có `EndDate <= DateTime.Now.AddDays(3)` để đưa ra **Cảnh báo đáo hạn**.

#### B. Logic Quản lý Dòng tiền
Được đặt tại `PaymentHistoriesController`:
* Mỗi khi có một giao dịch "Đóng lãi" hoặc "Chuộc đồ" diễn ra, hệ thống tự động cập nhật lại sổ quỹ (`CashFlow`).
* Dựa vào `InterestType` của hợp đồng để gợi ý mức tiền lãi chuẩn xác mà khách cần đóng.

#### C. Logic Bảo mật & Xác thực
Được đặt tại `AccountController`:
* Xác thực người dùng bằng `HttpContext.Session`. Mọi Controller nghiệp vụ đều kiểm tra Session trước khi cấp quyền truy cập.
* Cung cấp tính năng đổi mật khẩu an toàn với các ràng buộc Validate (so khớp mật khẩu cũ/mới) bằng Data Annotations.

---

## PHẦN V: KIỂM THỬ VÀ ĐÁNH GIÁ (TESTING)

Quá trình kiểm thử hệ thống được thực hiện nghiêm ngặt để đảm bảo tính chính xác của các thuật toán tài chính và trải nghiệm người dùng:

### 1. Kiểm thử Chức năng (Functional Testing)
- **Kiểm thử hộp đen (Black-box Testing):** Đóng vai trò là chủ tiệm, thực hiện tạo mới hợp đồng, đóng lãi, chuộc đồ để đảm bảo luồng nghiệp vụ không bị gián đoạn.
- **Kiểm thử biên (Boundary Testing):** Test các trường hợp nhập số tiền âm, nhập ngày kết thúc nhỏ hơn ngày bắt đầu để kiểm tra các thông báo lỗi (Validation) có hoạt động đúng không.

### 2. Kiểm thử Giao diện (UI/UX Testing)
- Kiểm tra tính đáp ứng (Responsive) của giao diện trên nhiều kích thước màn hình khác nhau nhờ hệ thống Grid của Bootstrap 5.
- Đảm bảo các Popup cảnh báo (SweetAlert2) hiển thị đúng lúc khi người dùng định xóa dữ liệu quan trọng.

### 3. Kiểm thử API & Tích hợp (Nếu có)
- Sử dụng **Postman / Swagger** để kiểm thử các Endpoint (AJAX) cung cấp số liệu dạng JSON cho biểu đồ Chart.js ở trang Bảng điều khiển.
- Đảm bảo thời gian phản hồi (Response Time) nhanh và dữ liệu trả về chính xác theo thời gian thực.
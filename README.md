# Bài Tập Lớn Back-End

Đây là dự án bài tập lớn môn Back-end, xây dựng một RESTful API sử dụng Node.js, Express và MongoDB.

## Tính năng

- ✅ RESTful API với Express.js
- ✅ Xác thực và phân quyền với JWT
- ✅ Mã hóa mật khẩu với bcryptjs
- ✅ Kết nối MongoDB với Mongoose
- ✅ Quản lý sản phẩm (CRUD operations)
- ✅ Quản lý người dùng
- ✅ Validation dữ liệu
- ✅ Xử lý lỗi tập trung
- ✅ CORS hỗ trợ

## Công nghệ sử dụng

- **Node.js** - Runtime environment
- **Express.js** - Web framework
- **MongoDB** - NoSQL database
- **Mongoose** - ODM cho MongoDB
- **JWT** - JSON Web Token cho authentication
- **bcryptjs** - Mã hóa mật khẩu
- **dotenv** - Quản lý biến môi trường

## Cài đặt

### Yêu cầu

- Node.js (phiên bản 14 trở lên)
- MongoDB (local hoặc MongoDB Atlas)
- npm hoặc yarn

### Các bước cài đặt

1. Clone repository:
```bash
git clone https://github.com/Nguyen12345tt/BaiTapLonBackEnd.git
cd BaiTapLonBackEnd
```

2. Cài đặt dependencies:
```bash
npm install
```

3. Tạo file `.env` từ `.env.example`:
```bash
cp .env.example .env
```

4. Cấu hình biến môi trường trong file `.env`:
```env
PORT=3000
MONGODB_URI=mongodb://localhost:27017/baitaplonbackend
JWT_SECRET=your_jwt_secret_key_here
JWT_EXPIRE=7d
```

5. Khởi động MongoDB (nếu dùng local):
```bash
mongod
```

6. Chạy server:
```bash
# Development mode với nodemon
npm run dev

# Production mode
npm start
```

Server sẽ chạy tại `http://localhost:3000`

## API Endpoints

### Authentication

#### Đăng ký người dùng
```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "Nguyễn Văn A",
  "email": "nguyen@example.com",
  "password": "123456",
  "role": "user"
}
```

#### Đăng nhập
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "nguyen@example.com",
  "password": "123456"
}
```

#### Lấy thông tin người dùng hiện tại
```http
GET /api/auth/me
Authorization: Bearer <token>
```

### Products (Sản phẩm)

#### Lấy tất cả sản phẩm
```http
GET /api/products
```

#### Lấy một sản phẩm theo ID
```http
GET /api/products/:id
```

#### Tạo sản phẩm mới (yêu cầu đăng nhập)
```http
POST /api/products
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Sản phẩm A",
  "description": "Mô tả sản phẩm A",
  "price": 100000,
  "category": "Điện tử",
  "stock": 50,
  "image": "product-a.jpg"
}
```

#### Cập nhật sản phẩm (yêu cầu đăng nhập và quyền sở hữu)
```http
PUT /api/products/:id
Authorization: Bearer <token>
Content-Type: application/json

{
  "name": "Sản phẩm A (đã cập nhật)",
  "price": 120000
}
```

#### Xóa sản phẩm (yêu cầu đăng nhập và quyền sở hữu)
```http
DELETE /api/products/:id
Authorization: Bearer <token>
```

## Cấu trúc thư mục

```
BaiTapLonBackEnd/
├── config/
│   ├── config.js          # Cấu hình ứng dụng
│   └── database.js        # Kết nối MongoDB
├── controllers/
│   ├── authController.js  # Controller xác thực
│   └── productController.js # Controller sản phẩm
├── middleware/
│   ├── auth.js           # Middleware xác thực
│   └── error.js          # Middleware xử lý lỗi
├── models/
│   ├── User.js           # Model người dùng
│   └── Product.js        # Model sản phẩm
├── routes/
│   ├── auth.js           # Routes xác thực
│   └── products.js       # Routes sản phẩm
├── .env.example          # File mẫu biến môi trường
├── .gitignore           # Git ignore file
├── package.json         # Dependencies và scripts
├── server.js            # Entry point
└── README.md           # Tài liệu dự án
```

## Models

### User Model
- `name`: Tên người dùng (required)
- `email`: Email (required, unique)
- `password`: Mật khẩu đã mã hóa (required, min 6 ký tự)
- `role`: Vai trò (user/admin)
- `createdAt`: Ngày tạo

### Product Model
- `name`: Tên sản phẩm (required)
- `description`: Mô tả (required)
- `price`: Giá (required, >= 0)
- `category`: Danh mục (required)
- `stock`: Số lượng tồn kho (required, >= 0)
- `image`: Hình ảnh (default: 'no-image.jpg')
- `createdBy`: Người tạo (reference to User)
- `createdAt`: Ngày tạo
- `updatedAt`: Ngày cập nhật

## Bảo mật

- Mật khẩu được mã hóa bằng bcryptjs trước khi lưu vào database
- Sử dụng JWT để xác thực và phân quyền
- Middleware bảo vệ các routes yêu cầu đăng nhập
- Validation dữ liệu đầu vào
- Xử lý lỗi tập trung

## Scripts

```bash
# Chạy server ở môi trường development
npm run dev

# Chạy server ở môi trường production
npm start

# Chạy tests (chưa cài đặt)
npm test
```

## Tác giả

Dự án bài tập lớn môn Back-end

## License

ISC
# API Testing Guide

Hướng dẫn test API sử dụng curl hoặc Postman

## 1. Test API Root
```bash
curl http://localhost:3000/
```

## 2. Authentication

### Đăng ký người dùng mới
```bash
curl -X POST http://localhost:3000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Nguyễn Văn A",
    "email": "nguyenvana@example.com",
    "password": "123456"
  }'
```

### Đăng ký Admin
```bash
curl -X POST http://localhost:3000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Admin User",
    "email": "admin@example.com",
    "password": "123456",
    "role": "admin"
  }'
```

### Đăng nhập
```bash
curl -X POST http://localhost:3000/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "nguyenvana@example.com",
    "password": "123456"
  }'
```

**Lưu token từ response để dùng cho các requests sau!**

### Lấy thông tin user hiện tại
```bash
curl -X GET http://localhost:3000/api/auth/me \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## 3. Products

### Lấy tất cả sản phẩm (Public)
```bash
curl -X GET http://localhost:3000/api/products
```

### Tạo sản phẩm mới (Cần đăng nhập)
```bash
curl -X POST http://localhost:3000/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "name": "Laptop Dell XPS 13",
    "description": "Laptop cao cấp cho dân văn phòng",
    "price": 25000000,
    "category": "Laptop",
    "stock": 10,
    "image": "dell-xps-13.jpg"
  }'
```

### Tạo thêm sản phẩm
```bash
curl -X POST http://localhost:3000/api/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "name": "iPhone 15 Pro",
    "description": "Điện thoại thông minh cao cấp",
    "price": 30000000,
    "category": "Điện thoại",
    "stock": 20
  }'
```

### Lấy một sản phẩm theo ID
```bash
curl -X GET http://localhost:3000/api/products/PRODUCT_ID_HERE
```

### Cập nhật sản phẩm (Cần đăng nhập và là người tạo)
```bash
curl -X PUT http://localhost:3000/api/products/PRODUCT_ID_HERE \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "price": 24000000,
    "stock": 15
  }'
```

### Xóa sản phẩm (Cần đăng nhập và là người tạo)
```bash
curl -X DELETE http://localhost:3000/api/products/PRODUCT_ID_HERE \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## Testing với Postman

1. Tạo collection mới trong Postman
2. Import các endpoints từ file này
3. Tạo environment variable cho `baseUrl` và `token`
4. Test từng endpoint theo thứ tự:
   - Register user
   - Login (lưu token)
   - Get Me
   - Create products
   - Get all products
   - Get single product
   - Update product
   - Delete product

## Test Scenarios

### Scenario 1: User flow hoàn chỉnh
1. Đăng ký user mới
2. Đăng nhập và lấy token
3. Tạo một sản phẩm
4. Xem danh sách sản phẩm
5. Cập nhật sản phẩm vừa tạo
6. Xóa sản phẩm

### Scenario 2: Authorization test
1. User A tạo sản phẩm
2. User B cố gắng cập nhật/xóa sản phẩm của User A (should fail)
3. Admin cập nhật/xóa sản phẩm của User A (should succeed)

### Scenario 3: Validation test
1. Thử đăng ký với email không hợp lệ
2. Thử đăng ký với password < 6 ký tự
3. Thử tạo sản phẩm với giá âm
4. Thử tạo sản phẩm thiếu required fields

## Expected Response Formats

### Success Response
```json
{
  "success": true,
  "message": "...",
  "data": {...}
}
```

### Error Response
```json
{
  "success": false,
  "message": "..."
}
```

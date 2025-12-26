const express = require('express');
const cors = require('cors');
const connectDB = require('./config/database');
const errorHandler = require('./middleware/error');
const config = require('./config/config');

// Kết nối database
connectDB();

// Khởi tạo Express app
const app = express();

// Middleware
app.use(cors());
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// Routes
app.use('/api/auth', require('./routes/auth'));
app.use('/api/products', require('./routes/products'));

// Route mặc định
app.get('/', (req, res) => {
  res.json({
    success: true,
    message: 'Chào mừng đến với API Bài tập lớn Back-end',
    endpoints: {
      auth: {
        register: 'POST /api/auth/register',
        login: 'POST /api/auth/login',
        getMe: 'GET /api/auth/me'
      },
      products: {
        getAll: 'GET /api/products',
        getOne: 'GET /api/products/:id',
        create: 'POST /api/products',
        update: 'PUT /api/products/:id',
        delete: 'DELETE /api/products/:id'
      }
    }
  });
});

// Error handler middleware
app.use(errorHandler);

// Khởi động server
const PORT = config.port;
const server = app.listen(PORT, () => {
  console.log(`Server đang chạy trên cổng ${PORT}`);
});

// Xử lý unhandled promise rejections
process.on('unhandledRejection', (err, promise) => {
  console.log(`Lỗi: ${err.message}`);
  server.close(() => process.exit(1));
});

module.exports = app;

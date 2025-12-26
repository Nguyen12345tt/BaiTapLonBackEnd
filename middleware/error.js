// Xử lý lỗi chung
const errorHandler = (err, req, res, next) => {
  let error = { ...err };
  error.message = err.message;

  // Lỗi Mongoose - CastError
  if (err.name === 'CastError') {
    const message = 'Không tìm thấy tài nguyên';
    error = { message, statusCode: 404 };
  }

  // Lỗi Mongoose - Trùng lặp key
  if (err.code === 11000) {
    const message = 'Giá trị đã tồn tại';
    error = { message, statusCode: 400 };
  }

  // Lỗi Mongoose - Validation
  if (err.name === 'ValidationError') {
    const message = Object.values(err.errors).map(val => val.message);
    error = { message, statusCode: 400 };
  }

  res.status(error.statusCode || 500).json({
    success: false,
    message: error.message || 'Lỗi server'
  });
};

module.exports = errorHandler;

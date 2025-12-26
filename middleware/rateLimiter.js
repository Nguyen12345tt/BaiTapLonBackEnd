const rateLimit = require('express-rate-limit');

// Rate limiter cho authentication routes (đăng nhập, đăng ký)
const authLimiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 phút
  max: 5, // Giới hạn 5 requests mỗi IP trong 15 phút
  message: {
    success: false,
    message: 'Quá nhiều yêu cầu từ địa chỉ IP này, vui lòng thử lại sau 15 phút'
  },
  standardHeaders: true,
  legacyHeaders: false,
});

// Rate limiter chung cho các API routes khác
const apiLimiter = rateLimit({
  windowMs: 15 * 60 * 1000, // 15 phút
  max: 100, // Giới hạn 100 requests mỗi IP trong 15 phút
  message: {
    success: false,
    message: 'Quá nhiều yêu cầu từ địa chỉ IP này, vui lòng thử lại sau'
  },
  standardHeaders: true,
  legacyHeaders: false,
});

module.exports = {
  authLimiter,
  apiLimiter
};

/**
 * Validation script - Kiểm tra cấu trúc code và imports
 * Script này không cần MongoDB để chạy
 */

console.log('=== Bắt đầu kiểm tra cấu trúc dự án ===\n');

// Test 1: Kiểm tra config
console.log('✓ Test 1: Kiểm tra config');
try {
  const config = require('./config/config');
  console.log('  - Config loaded:', {
    port: config.port,
    hasJwtSecret: !!config.jwtSecret,
    jwtExpire: config.jwtExpire
  });
} catch (error) {
  console.error('  ✗ Error loading config:', error.message);
  process.exit(1);
}

// Test 2: Kiểm tra Models
console.log('\n✓ Test 2: Kiểm tra Models');
try {
  const User = require('./models/User');
  const Product = require('./models/Product');
  console.log('  - User model loaded successfully');
  console.log('  - Product model loaded successfully');
} catch (error) {
  console.error('  ✗ Error loading models:', error.message);
  process.exit(1);
}

// Test 3: Kiểm tra Middleware
console.log('\n✓ Test 3: Kiểm tra Middleware');
try {
  const { protect, authorize } = require('./middleware/auth');
  const errorHandler = require('./middleware/error');
  console.log('  - Auth middleware loaded successfully');
  console.log('  - Error handler loaded successfully');
  console.log('  - protect function exists:', typeof protect === 'function');
  console.log('  - authorize function exists:', typeof authorize === 'function');
} catch (error) {
  console.error('  ✗ Error loading middleware:', error.message);
  process.exit(1);
}

// Test 4: Kiểm tra Controllers
console.log('\n✓ Test 4: Kiểm tra Controllers');
try {
  const authController = require('./controllers/authController');
  const productController = require('./controllers/productController');
  console.log('  - Auth controller loaded successfully');
  console.log('  - Product controller loaded successfully');
  console.log('  - authController.register exists:', typeof authController.register === 'function');
  console.log('  - authController.login exists:', typeof authController.login === 'function');
  console.log('  - productController.getProducts exists:', typeof productController.getProducts === 'function');
  console.log('  - productController.createProduct exists:', typeof productController.createProduct === 'function');
} catch (error) {
  console.error('  ✗ Error loading controllers:', error.message);
  process.exit(1);
}

// Test 5: Kiểm tra Routes
console.log('\n✓ Test 5: Kiểm tra Routes');
try {
  const authRoutes = require('./routes/auth');
  const productRoutes = require('./routes/products');
  console.log('  - Auth routes loaded successfully');
  console.log('  - Product routes loaded successfully');
} catch (error) {
  console.error('  ✗ Error loading routes:', error.message);
  process.exit(1);
}

// Test 6: Kiểm tra package.json
console.log('\n✓ Test 6: Kiểm tra package.json');
try {
  const pkg = require('./package.json');
  const requiredDeps = ['express', 'mongoose', 'dotenv', 'bcryptjs', 'jsonwebtoken', 'cors'];
  const missingDeps = requiredDeps.filter(dep => !pkg.dependencies[dep]);
  
  if (missingDeps.length > 0) {
    console.error('  ✗ Missing dependencies:', missingDeps);
    process.exit(1);
  }
  
  console.log('  - All required dependencies present');
  console.log('  - Scripts configured:', Object.keys(pkg.scripts).join(', '));
} catch (error) {
  console.error('  ✗ Error checking package.json:', error.message);
  process.exit(1);
}

console.log('\n=== ✓ Tất cả kiểm tra đã pass! ===');
console.log('\nCấu trúc dự án hợp lệ.');
console.log('\nĐể chạy server:');
console.log('1. Cài đặt MongoDB (local hoặc MongoDB Atlas)');
console.log('2. Cấu hình MONGODB_URI trong file .env');
console.log('3. Chạy: npm start hoặc npm run dev');
console.log('\nXem file API_TESTING.md để biết cách test API.');

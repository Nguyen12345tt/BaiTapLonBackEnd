const express = require('express');
const router = express.Router();
const {
  getProducts,
  getProduct,
  createProduct,
  updateProduct,
  deleteProduct
} = require('../controllers/productController');
const { protect, authorize } = require('../middleware/auth');
const { apiLimiter } = require('../middleware/rateLimiter');

router.route('/')
  .get(apiLimiter, getProducts)
  .post(protect, apiLimiter, createProduct);

router.route('/:id')
  .get(apiLimiter, getProduct)
  .put(protect, apiLimiter, updateProduct)
  .delete(protect, apiLimiter, deleteProduct);

module.exports = router;

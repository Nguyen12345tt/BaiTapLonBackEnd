require('dotenv').config();

module.exports = {
  port: process.env.PORT || 3000,
  mongodbUri: process.env.MONGODB_URI || 'mongodb://localhost:27017/baitaplonbackend',
  jwtSecret: process.env.JWT_SECRET || 'default_secret_change_this',
  jwtExpire: process.env.JWT_EXPIRE || '7d'
};

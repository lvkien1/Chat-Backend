# API Endpoints

## Authentication
- POST /api/auth/register
  - Đăng ký user mới
- POST /api/auth/login  
  - Đăng nhập, nhận JWT token
- POST /api/auth/refresh-token
  - Làm mới access token

## Users
- GET /api/users/me
  - Lấy thông tin user hiện tại
- GET /api/users/{id}
  - Lấy thông tin user theo ID
- GET /api/users/search?q={query}
  - Tìm kiếm user

## Conversations
- GET /api/conversations
  - Lấy danh sách conversations
- POST /api/conversations
  - Tạo conversation mới
- GET /api/conversations/{id}
  - Lấy chi tiết conversation
- GET /api/conversations/{id}/messages
  - Lấy lịch sử chat

## Messages
- POST /api/messages
  - Gửi message mới
- DELETE /api/messages/{id}
  - Xóa message

## WebSocket
- /chatHub
  - SignalR hub cho real-time chat

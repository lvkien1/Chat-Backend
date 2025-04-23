# Backend Project Brief

## Mục tiêu
- Xây dựng API cho ứng dụng chat realtime sử dụng .NET 9.0
- Hỗ trợ WebSocket qua SignalR (đang lên kế hoạch)
- Sử dụng MySQL với Entity Framework Core
- API RESTful cho các chức năng quản lý
- JWT Authentication với Refresh Token

## Tiến độ hiện tại
- [x] Cài đặt và cấu hình ASP.NET Core Identity với MySQL
- [x] Triển khai JWT Authentication
- [x] Nâng cấp lên .NET 9.0
- [x] Áp dụng kiến trúc 3 tầng: Controllers, BL, DL
- [x] Cấu hình Entity Framework Core với Migrations
- [x] Tạo database schema
- [x] Triển khai API đăng ký, đăng nhập, refresh token
- [ ] Triển khai SignalR cho chat realtime
- [ ] Triển khai User Management
- [ ] Triển khai Conversation và Message APIs

## Phạm vi
1. Authentication Service (✅ Đã triển khai):
- Đăng ký/đăng nhập người dùng
- JWT token management
- Refresh token mechanism

2. Chat Service (⏳ Đang lên kế hoạch):
- Quản lý conversations
- Real-time messaging qua WebSocket
- Lịch sử chat

3. User Management (⏳ Đang lên kế hoạch):
- Profile management
- Danh sách bạn bè/contacts
- Online status

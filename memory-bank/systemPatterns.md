# Kiến trúc hệ thống

## Mô hình tổng thể
1. Controller Layer (API endpoints)
   - AuthController (quản lý xác thực)
   - Các controller khác (sẽ triển khai)

2. Business Logic Layer (BL)
   - Interface: I[Function]BL (IAuthBL, ...)
   - Implementation: [Function]BL (AuthBL, ...)
   - Xử lý logic nghiệp vụ, validate dữ liệu

3. Data Layer (DL)
   - Interface: I[Function]DL (IAuthDL, ...)
   - Implementation: [Function]DL (AuthDL, ...)
   - Tương tác với database thông qua Entity Framework Core

4. Database (MySQL)

## Flow xử lý authentication:
1. Client gửi request đến API (register/login/refresh-token)
2. AuthController nhận request và chuyển tới AuthBL
3. AuthBL xử lý logic nghiệp vụ (validate, generate token)
4. AuthDL tương tác với database thông qua Entity Framework/Identity
5. Response trả về cho client với token/refresh token

## Flow xử lý message (triển khai hoàn thành):
1. Client kết nối SignalR ChatHub với JWT token qua query string
   ```
   ws://localhost:5016/chatHub?access_token=your_jwt_token
   ```
2. Hub xác thực JWT token tự động thông qua middleware JwtBearer
3. Client gọi `JoinConversation(conversationId)` để tham gia conversation
4. ChatHub kiểm tra quyền truy cập conversation thông qua `IChatBL.IsUserInConversationAsync`
5. Client gửi tin nhắn thông qua gọi `SendMessage(conversationId, content)`
6. ChatBL thực hiện lưu message vào database và trả về message với đầy đủ thông tin
7. ChatHub broadcast message đến tất cả clients trong cùng conversation group
8. Clients nhận tin nhắn realtime qua sự kiện `ReceiveMessage`

## Pattern Realtime Communication:
1. **Group-based Broadcasting**:
   - Mỗi conversation là một group trong SignalR
   - User phải join vào group trước khi gửi/nhận tin nhắn
   - Broadcast chỉ trong phạm vi group

2. **Authentication với WebSocket**:
   - JWT token truyền qua query string `access_token`
   - Context.User chứa thông tin người dùng đã authenticate
   - Kiểm tra quyền truy cập conversation trước mỗi thao tác

3. **Connection Management**:
   - OnConnectedAsync: Lưu trạng thái online, add connection vào user group
   - OnDisconnectedAsync: Cập nhật trạng thái offline
   - Logging cho kết nối và ngắt kết nối

## Database Access Pattern:
- ApplicationDbContext kế thừa từ IdentityDbContext
- Entity Framework Core (Code First) với migrations
- Pomelo.EntityFrameworkCore.MySql kết nối tới MySQL
- DbSet<T> cho các entity (AspNetUsers, RefreshTokens, ...)

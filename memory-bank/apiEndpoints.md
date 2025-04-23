# API Endpoints

## Authentication
- **POST /api/auth/register**
  - Đăng ký user mới
  - Request:
    ```json
    {
      "username": "string",
      "email": "string",
      "password": "string",
      "fullName": "string"
    }
    ```
  - Response:
    ```json
    {
      "success": true,
      "message": "Đăng ký thành công!",
      "token": "string (JWT)",
      "refreshToken": "string",
      "expiration": "datetime",
      "user": {
        "id": "string",
        "username": "string",
        "email": "string",
        "fullName": "string",
        "isOnline": true
      }
    }
    ```

- **POST /api/auth/login**
  - Đăng nhập, nhận JWT token
  - Request:
    ```json
    {
      "username": "string",
      "password": "string"
    }
    ```
  - Response: (tương tự như register)

- **POST /api/auth/refresh-token**
  - Làm mới access token
  - Request:
    ```json
    {
      "refreshToken": "string"
    }
    ```
  - Response: (tương tự như login)

- **POST /api/auth/revoke-token**
  - Hủy refresh token
  - Yêu cầu xác thực (JWT Bearer token)
  - Response:
    ```json
    {
      "success": true,
      "message": "Token revoked successfully"
    }
    ```

## Users (kế hoạch triển khai)
- GET /api/users/me
  - Lấy thông tin user hiện tại
- GET /api/users/{id}
  - Lấy thông tin user theo ID
- GET /api/users/search?q={query}
  - Tìm kiếm user

## Conversations (kế hoạch triển khai)
- GET /api/conversations
  - Lấy danh sách conversations
- POST /api/conversations
  - Tạo conversation mới
- GET /api/conversations/{id}
  - Lấy chi tiết conversation
- GET /api/conversations/{id}/messages
  - Lấy lịch sử chat

## Messages (kế hoạch triển khai)
- POST /api/messages
  - Gửi message mới
- DELETE /api/messages/{id}
  - Xóa message

## WebSocket / SignalR (triển khai hoàn thành)
- **/chatHub**
  - SignalR hub cho real-time chat
  - Kết nối qua WebSocket, với JWT token gửi qua query string:
    ```
    ws://localhost:5016/chatHub?access_token=your_jwt_token
    ```
  - **Methods:**
    - `JoinConversation(int conversationId)`
      - Tham gia vào group conversation
      - Yêu cầu user phải là thành viên của conversation
    - `SendMessage(int conversationId, string content)`
      - Gửi tin nhắn đến conversation
      - Lưu vào database và phát tin nhắn đến tất cả thành viên conversation
    - `MarkAsRead(int conversationId, int messageId)`
      - Đánh dấu tin nhắn đã đọc
      - Thông báo cho tất cả thành viên conversation biết
    - `SendTypingNotification(int conversationId)`
      - Thông báo user đang gõ tin nhắn
      - Phát thông báo đến tất cả thành viên conversation
  
  - **Events:**
    - `ReceiveMessage`
      - Nhận tin nhắn mới trong conversation
    - `MessageRead`
      - Nhận thông báo tin nhắn đã được đọc
    - `UserTyping`
      - Nhận thông báo có user đang gõ tin nhắn

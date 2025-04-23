# Backend Chat Demo

Đây là dự án backend cho ứng dụng chat realtime, được xây dựng bằng .NET 9.0, ASP.NET Core Web API, SignalR, Entity Framework Core và MySQL.

## Mục tiêu
- Cung cấp API RESTful cho các chức năng quản lý người dùng, cuộc trò chuyện.
- Hỗ trợ gửi/nhận tin nhắn realtime qua WebSocket (SignalR).
- Sử dụng JWT để xác thực và ủy quyền.

## Hướng dẫn cài đặt

1.  **Clone repository:**
    ```bash
    git clone <your-repository-url>
    cd Backend
    ```
2.  **Cài đặt .NET 9.0 SDK:**
    Tải và cài đặt từ [trang chủ .NET](https://dotnet.microsoft.com/download/dotnet/9.0).
3.  **Cấu hình Database:**
    - Mở file `appsettings.json`.
    - Cập nhật chuỗi kết nối `DefaultConnection` để trỏ đến MySQL server của bạn.
    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "server=your_server;port=3306;database=your_database;user=your_user;password=your_password;"
      },
      // ... other settings
    }
    ```
    - **Lưu ý:** Không commit file `appsettings.Development.json` hoặc các file chứa thông tin nhạy cảm khác lên Git. Sử dụng User Secrets hoặc các phương pháp quản lý secrets khác cho môi trường development.
4.  **Chạy Migrations:**
    Mở terminal trong thư mục gốc của dự án (`Backend`) và chạy lệnh:
    ```bash
    dotnet ef database update
    ```
    Lệnh này sẽ tạo database (nếu chưa có) và áp dụng các schema migrations.
5.  **Chạy ứng dụng:**
    ```bash
    dotnet run
    ```
    API sẽ chạy tại địa chỉ được cấu hình trong `Properties/launchSettings.json` (thường là `https://localhost:xxxx` hoặc `http://localhost:yyyy`).

## Chức năng chính

-   **Authentication:**
    -   Đăng ký tài khoản mới (`/api/auth/register`).
    -   Đăng nhập (`/api/auth/login`).
    -   Làm mới JWT token bằng refresh token (`/api/auth/refresh-token`).
-   **Chat Realtime (SignalR):**
    -   Kết nối tới Hub: `ws://your_server/chatHub?access_token=<jwt_token>`.
    -   Gửi tin nhắn: Gọi method `SendMessage(conversationId, content)`.
    -   Nhận tin nhắn: Lắng nghe sự kiện `ReceiveMessage(messageObject)`.
    -   Tham gia cuộc trò chuyện: Gọi method `JoinConversation(conversationId)`.
-   **Quản lý Conversations (API):**
    -   Lấy danh sách conversations của user (`/api/conversations`).
    -   Tạo conversation mới (`/api/conversations`).
    -   Lấy tin nhắn trong conversation (`/api/conversations/{conversationId}/messages`).
-   **User Management (Đang phát triển):**
    -   Quản lý thông tin cá nhân.
    -   Quản lý danh sách bạn bè/contacts.

## Thông tin kỹ thuật

-   **Framework:** .NET 9.0, ASP.NET Core
-   **Database:** MySQL 8.0+, Entity Framework Core 9.0
-   **Realtime:** SignalR
-   **Authentication:** JWT Bearer Tokens, ASP.NET Core Identity
-   **Kiến trúc:** 3 tầng (Controller, Business Logic Layer - BL, Data Layer - DL)
-   **Packages chính:**
    -   `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
    -   `Microsoft.EntityFrameworkCore.Design`
    -   `Pomelo.EntityFrameworkCore.MySql`
    -   `Microsoft.AspNetCore.Authentication.JwtBearer`
    -   `Microsoft.AspNetCore.SignalR`

## Testing

-   **API:** Sử dụng Swagger UI (truy cập `/swagger` khi chạy ứng dụng) hoặc Postman.
-   **SignalR:** Sử dụng client SignalR (ví dụ: thư viện `@microsoft/signalr` cho JavaScript/TypeScript) để kết nối và test các method của Hub.

## Bảo mật

-   **KHÔNG** commit các file chứa thông tin nhạy cảm như `appsettings.Development.json`, `secrets.json` hoặc chuỗi kết nối trực tiếp trong code.
-   Sử dụng User Secrets trong môi trường development và các giải pháp quản lý secrets an toàn (Azure Key Vault, HashiCorp Vault, etc.) cho môi trường production.

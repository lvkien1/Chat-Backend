# Kiến trúc hệ thống

## Mô hình tổng thể
1. Controller Layer (API endpoints)
2. Service Layer (business logic) 
3. Repository Layer (Dapper data access)
4. Database (MySQL)

## Flow xử lý message:
1. Client kết nối SignalR Hub
2. Hub xác thực JWT token
3. Service xử lý business logic
4. Repository lưu message vào MySQL
5. Hub broadcast message tới clients

## Database Access Pattern:
- Sử dụng Dapper cho các query phức tạp
- Dapper.Contrib cho CRUD đơn giản
- Connection pooling với MySqlConnector

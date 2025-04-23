# Progress Status

## [15/04/2025] Đã test thành công gửi tin nhắn realtime SignalR

- Đã test thành công tính năng gửi tin nhắn realtime qua SignalR (ChatHub.SendMessage) từ frontend Angular.
- Quy trình test:
  - Kết nối SignalR từ Angular bằng @microsoft/signalr.
  - Join vào conversation qua method JoinConversation.
  - Gửi tin nhắn qua method SendMessage.
  - Nhận lại tin nhắn realtime qua ReceiveMessage.
- Đã xử lý các vấn đề về CORS, xác thực JWT, và quyền truy cập conversation.

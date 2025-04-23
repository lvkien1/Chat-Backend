# Database Schema (ASP.NET Core Identity với MySQL)

## Identity Tables (Tự động tạo bởi Entity Framework Core)

1. AspNetUsers (Thay thế cho Users):
```
- Id (string, PK)
- UserName (string, unique)
- NormalizedUserName (string, unique)
- Email (string)
- NormalizedEmail (string)
- EmailConfirmed (bool)
- PasswordHash (string)
- SecurityStamp (string)
- ConcurrencyStamp (string)
- PhoneNumber (string)
- PhoneNumberConfirmed (bool)
- TwoFactorEnabled (bool)
- LockoutEnd (DateTimeOffset)
- LockoutEnabled (bool)
- AccessFailedCount (int)
- FullName (string) - Thêm vào
- CreatedAt (DateTime) - Thêm vào
- IsOnline (bool) - Thêm vào
- LastLogin (DateTime) - Thêm vào
```

2. AspNetRoles:
```
- Id (string, PK)
- Name (string)
- NormalizedName (string, unique)
- ConcurrencyStamp (string)
```

3. AspNetUserRoles:
```
- UserId (string, PK, FK -> AspNetUsers)
- RoleId (string, PK, FK -> AspNetRoles)
```

4. AspNetUserClaims:
```
- Id (int, PK)
- UserId (string, FK -> AspNetUsers)
- ClaimType (string)
- ClaimValue (string)
```

5. AspNetRoleClaims:
```
- Id (int, PK)
- RoleId (string, FK -> AspNetRoles)
- ClaimType (string)
- ClaimValue (string)
```

6. AspNetUserLogins:
```
- LoginProvider (string, PK)
- ProviderKey (string, PK)
- ProviderDisplayName (string)
- UserId (string, FK -> AspNetUsers)
```

7. AspNetUserTokens:
```
- UserId (string, PK, FK -> AspNetUsers)
- LoginProvider (string, PK)
- Name (string, PK)
- Value (string)
```

## Custom Tables

8. RefreshTokens:
```
- Id (int, PK, auto-increment)
- UserId (string, FK -> AspNetUsers)
- Token (string)
- ExpiryDate (DateTime)
- IsRevoked (bool)
- CreatedAt (DateTime)
```

## Kế hoạch triển khai các bảng khác

9. Conversations (kế hoạch):
```
- Id (int, PK)
- Name (string)
- CreatedAt (DateTime)
- LastMessageAt (DateTime)
```

10. Participants (kế hoạch):
```
- UserId (string, PK, FK -> AspNetUsers)
- ConversationId (int, PK, FK -> Conversations)
- JoinedAt (DateTime)
```

11. Messages (kế hoạch):
```
- Id (int, PK)
- Content (text)
- SenderId (string, FK -> AspNetUsers)
- ConversationId (int, FK -> Conversations)
- SentAt (DateTime)
- Status (tinyint)
```

12. Attachments (kế hoạch):
```
- Id (int, PK)
- MessageId (int, FK -> Messages)
- FileUrl (string)
- FileName (string)
- FileType (string)
- FileSize (int)
```
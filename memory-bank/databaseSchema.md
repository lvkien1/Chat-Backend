# Database Schema (MySQL)

## Tables chính:

1. Users:
```sql
CREATE TABLE Users (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Username VARCHAR(50) NOT NULL UNIQUE,
  Email VARCHAR(100) NOT NULL UNIQUE,
  PasswordHash VARCHAR(255) NOT NULL,
  CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  LastLogin DATETIME,
  IsOnline BOOLEAN DEFAULT FALSE
);
```

2. Conversations:
```sql
CREATE TABLE Conversations (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Name VARCHAR(100),
  CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  LastMessageAt DATETIME
);
```

3. Participants:
```sql
CREATE TABLE Participants (
  UserId INT NOT NULL,
  ConversationId INT NOT NULL,
  JoinedAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (UserId, ConversationId),
  FOREIGN KEY (UserId) REFERENCES Users(Id),
  FOREIGN KEY (ConversationId) REFERENCES Conversations(Id)
);
```

4. Messages:
```sql
CREATE TABLE Messages (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  Content TEXT NOT NULL,
  SenderId INT NOT NULL,
  ConversationId INT NOT NULL,
  SentAt DATETIME DEFAULT CURRENT_TIMESTAMP,
  Status TINYINT DEFAULT 0,
  FOREIGN KEY (SenderId) REFERENCES Users(Id),
  FOREIGN KEY (ConversationId) REFERENCES Conversations(Id)
);
```

5. Attachments:
```sql
CREATE TABLE Attachments (
  Id INT AUTO_INCREMENT PRIMARY KEY,
  MessageId INT NOT NULL,
  FileUrl VARCHAR(255) NOT NULL,
  FileName VARCHAR(100) NOT NULL,
  FileType VARCHAR(50) NOT NULL,
  FileSize INT NOT NULL,
  FOREIGN KEY (MessageId) REFERENCES Messages(Id)
);

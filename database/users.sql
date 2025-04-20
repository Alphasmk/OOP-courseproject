use gamelauncher;
create TABLE Users
(
UserID INT PRIMARY KEY IDENTITY(1, 1),
Email NVARCHAR(100) NOT NULL UNIQUE,
Password NVARCHAR(100) NOT NULL,
Created DATETIME DEFAULT GETDATE()
)

ALTER TABLE Users ADD IsAdmin tinyint,
CONSTRAINT admin_check CHECK(IsAdmin in (1, 0))
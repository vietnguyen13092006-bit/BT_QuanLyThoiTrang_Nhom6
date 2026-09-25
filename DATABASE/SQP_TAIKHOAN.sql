CREATE DATABASE TAIKHOAN
go
use TAIKHOAN
go

CREATE TABLE Users(
	Userid INT IDENTITY(1,1) PRIMARY KEY,
	Username NVARCHAR(50) NOT NULL,
	PassW VARCHAR(250) NOT NULL,
	SDT VARCHAR(15) null,
	Email NVARCHAR(100) null
);

GO
ALTER TABLE Users
ADD AvatarPath NVARCHAR(255) null;
GO

select* from Users

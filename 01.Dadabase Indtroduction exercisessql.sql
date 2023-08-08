--1
CREATE DATABASE [Minions]
CREATE TABLE [Minions]
(
Id INT PRIMARY KEY,
[Name] NVARCHAR(50) NOT NULL,
Age INT NOT NULL
)

--2
CREATE TABLE [Towns]
(
Id INT PRIMARY KEY,
[Name] NVARCHAR(50) NOT NULL,
)

--3
ALTER TABLE [Minions] 
ADD  [TownId] INT FOREIGN KEY REFERENCES [Towns]([Id]) NOT NULL

ALTER TABLE [Minions] 
ALTER COLUMN [Age] INT

--4
INSERT INTO [Towns]([Id],[Name])
VALUES 
(1, 'Sofia'),
(2, 'Plovdiv'),
(3, 'Varna')

INSERT INTO [Minions]([Id],[Name],[Age], [TownId])
VALUES
(1,'Kevin',22,1),
(2,'Bob',15,3),
(3,'Steward',NULL,2)

SELECT * FROM  [Towns]
SELECT * FROM [Minions]

--5
TRUNCATE TABLE [Minions]

--6
DROP TABLE [Minions]
DROP TABLE [Towns]

--7
CREATE TABLE [PEOPLE]
(
[Id] INT PRIMARY KEY IDENTITY,
[Name] NVARCHAR(200) NOT NULL,
[Picture] VARBINARY(MAX),
CHECK (DATALENGTH([Picture]) <= 2000000),
[Height] DECIMAL(3,2),
[Weight] DECIMAL(5,2),
[Gender] CHAR(1) NOT NULL,
CHECK ([Gender] = 'm' OR [Gender] = 'f'),
[Birthdate] DATE NOT NULL,
[Biography] NVARCHAR(MAX)
)

INSERT INTO [PEOPLE]([Name],[Height],[Weight],[Gender],[Birthdate])
VALUES
('Viktor',1.50,70.5,'m','1999-08-12'),
('Peter',1.60,70.8,'f','1990-12-12'),
('Shosho',1.70,70.9,'m','1995-12-12'),
('Bleh',1.80,70.1,'f','1994-08-25'),
('Meh',NULL,NULL,'m','2999-08-25')

--8
CREATE TABLE [Users]
(
[Id] INT PRIMARY KEY IDENTITY,
[Username] NVARCHAR(30) NOT NULL,
[Password] NVARCHAR(26) NOT NULL,
[ProfilePicture] VARBINARY(MAX),
CHECK (DATALENGTH([ProfilePicture]) <= 900000),
[LastLoginTime] DATETIME2,
[IsDeleted] BIT NOT NULL
)

INSERT INTO [Users] ([Username],[Password],[ProfilePicture],[LastLoginTime],[IsDeleted])
VALUES
	('Username 1', 'Password 1', NULL, NULL, 0),
	('Username 2', 'Password 2', NULL, NULL, 0),
	('Username 3', 'Password 3', NULL, NULL, 0),
	('Username 4', 'Password 4', NULL, NULL, 1),
	('Username 5', 'Password 5', NULL, NULL, 1)

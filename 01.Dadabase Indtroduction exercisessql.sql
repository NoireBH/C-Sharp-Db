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

--9

	ALTER TABLE [Users]
	DROP CONSTRAINT PK__Users__3214EC07CF4EBC6A

	ALTER TABLE [Users]
	ADD CONSTRAINT PK_Users 
	PRIMARY KEY (Id, Username)

--10

	ALTER TABLE [Users]
	ADD CONSTRAINT CheckPassword
	CHECK (LEN([Password]) >= 5)

--11

	ALTER TABLE [Users]
	ADD CONSTRAINT df_LastLoginTime DEFAULT GETDATE() FOR [LastLoginTime]

	INSERT INTO [Users] ([Username],[Password],[ProfilePicture],[LastLoginTime],[IsDeleted])
VALUES
	('Username 6', 'Password 6', NULL, NULL, 0)

	INSERT INTO [Users] ([Username],[Password],[ProfilePicture],[IsDeleted])
VALUES
	('Username 7', 'Password 7', NULL, 0)

--12

	ALTER TABLE [Users]
	DROP PK_Users

	ALTER TABLE [Users]
	ADD CONSTRAINT PK_Users 
	PRIMARY KEY (Id)

--13

	CREATE DATABASE [Movies]
	USE [Movies]

	CREATE TABLE [Directors]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[DirectorName] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(1000)
)
	CREATE TABLE [Genres]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[GenreName] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(1000)
)

	CREATE TABLE [Categories]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[CategoryName] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [Movies]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[Title] VARCHAR(50) NOT NULL,
	[DirectorId] INT FOREIGN KEY REFERENCES [Directors](Id) NOT NULL,
	[CopyrightYear] INT NOT NULL,
	[Length] TIME NOT NULL,
	[GenreId] INT FOREIGN KEY REFERENCES [Genres](Id) NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES [Categories](Id) NOT NULL,
	[Rating] INT NOT NULL,
	[Notes] NVARCHAR(1000)
)

	INSERT INTO [Directors] VALUES
	('me', NULL),
	('director1', NULL),
	('Obi Wan', NULL),
	('Steven Spielberg', NULL),
	('Adam Sandler', NULL)

INSERT INTO [Genres] VALUES
	('Fantasy', NULL),
	('Horror', NULL),
	('Family-Friendly', NULL),
	('Sci-fi', NULL),
	('Comedy', NULL)

INSERT INTO [Categories] VALUES
	('Animated', NULL),
	('Real', NULL),
	('Biography', NULL),
	('Documentary', NULL),
	('TV', NULL)

INSERT INTO [Movies] VALUES
	('Garfield', 1, 1991, '02:22:00', 2, 3, 9, NULL),
	('The Godfather', 2, 1932, '02:55:00', 3, 4, 8, NULL),
	('IT', 3, 1996, '03:15:00', 4, 5, 7, NULL),
	('Pulp Fiction', 4, 1994, '02:34:00', 5, 1, 6, NULL),
	('Fight Club', 5, 1999, '02:19:00', 1, 2, 5, NULL)

--14

	CREATE DATABASE [CarRental]
	USE [CarRental]

	CREATE TABLE [Categories]
	(
	[Id] INT PRIMARY KEY IDENTITY,
	[CategoryName] VARCHAR(50) NOT NULL,
	[DailyRate] DECIMAL(6, 2) NOT NULL,
	[WeeklyRate] DECIMAL(6, 2) NOT NULL,
	[MonthlyRate] DECIMAL(6, 2) NOT NULL,
	[WeekendRate] DECIMAL(6, 2) NOT NULL
)

CREATE TABLE [Cars]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[PlateNumber] VARCHAR(30) NOT NULL,
	[Manufacturer] VARCHAR(50) NOT NULL,
	[Model] VARCHAR(50) NOT NULL,
	[CarYear] INT NOT NULL,
	[CategoryId] INT FOREIGN KEY REFERENCES [Categories](Id) NOT NULL,
	[Doors] INT NOT NULL,
	[Picture] IMAGE,
	[Condition] NVARCHAR(1000) NOT NULL,
	[Available] BIT NOT NULL
)

CREATE TABLE [Employees]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] VARCHAR(30) NOT NULL,
	[LastName] VARCHAR(30) NOT NULL,
	[Title] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [Customers]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[DriverLicenceNumber] INT NOT NULL,
	[FullName] VARCHAR(50) NOT NULL,
	[Address] VARCHAR(200) NOT NULL,
	[City] VARCHAR(50) NOT NULL,
	[ZIPCode] INT NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [RentalOrders]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES [Employees](Id) NOT NULL,
	[CustomerId] INT FOREIGN KEY REFERENCES [Customers](Id) NOT NULL,
	[CarId] INT FOREIGN KEY REFERENCES [Cars](Id) NOT NULL,
	[TankLevel] INT NOT NULL,
	[KilometrageStart] INT NOT NULL,
	[KilometrageEnd] INT NOT NULL,
	[TotalKilometrage] INT NOT NULL,
	[StartDate] DATE NOT NULL,
	[EndDate] DATE NOT NULL,
	[TotalDays] INT NOT NULL,
	[RateApplied] DECIMAL(6, 2) NOT NULL,
	[TaxRate] DECIMAL(4, 2) NOT NULL,
	[OrderStatus] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(1000)
)

INSERT INTO [Categories] VALUES
	('First category name', 10.00, 50.00, 150.00, 20.00),
	('Second category name', 50.00, 250.00, 750.00, 100.00),
	('Third category name', 100.00, 500.00, 1500.00, 200.00)

INSERT INTO [Cars] VALUES
	('PLN 0001', 'Ford', 'Model A', 1994, 1, 4, NULL, 'Good', 1),
	('PLN 0002', 'Tesla', 'Model B', 2021, 2, 4, NULL, 'Great', 1),
	('PLN 0003', 'Capsule Corp', 'Model C', 2054, 3, 10, NULL, 'Best', 0)

INSERT INTO [Employees] VALUES
	('Tyler', 'Durden', 'Edward Norton`s Alter Ego', NULL),
	('Plain', 'Jane', 'some gal', NULL),
	('Average', 'Joe', 'some dude', NULL)

INSERT INTO [Customers] VALUES
	('123456', 'Jimmy Carr', 'Britain', 'London', 1000, NULL),
	('654321', 'Bill Burr', 'USA', 'Washington', 2000, NULL),
	('999999', 'Louis CK', 'Mexico', 'Mexico City', 3000, NULL)

INSERT INTO [RentalOrders] VALUES
	(1, 1, 1, 70, 90000, 100000, 10000, '1994-10-01', '1994-10-21', 20, 100.00, 14.00, 'Pending', NULL),
	(2, 2, 2, 85, 250000, 2750000, 25000, '2011-11-12', '2011-11-24', 12, 150.00, 17.50, 'Canceled', NULL),
	(3, 3, 3, 90, 0, 120000, 120000, '2025-04-05', '2025-05-02', 27, 220.00, 21.25, 'Delivered', NULL)

--15

CREATE DATABASE [Hotel]
USE [Hotel]

CREATE TABLE [Employees]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[Title] VARCHAR(50) NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [Customers]
(
	[AccountNumber] INT PRIMARY KEY IDENTITY,
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[PhoneNumber] INT NOT NULL,
	[EmergencyName] VARCHAR(50),
	[EmergencyNumber] INT,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [RoomStatus]
(
	[RoomStatus] VARCHAR(50) PRIMARY KEY NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [RoomTypes]
(
	[RoomType] VARCHAR(50) PRIMARY KEY NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [BedTypes]
(
	[BedType] VARCHAR(50) PRIMARY KEY NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [Rooms]
(
	[RoomNumber] INT PRIMARY KEY IDENTITY,
	[RoomType] VARCHAR(50) FOREIGN KEY REFERENCES [RoomTypes](RoomType) NOT NULL,
	[BedType] VARCHAR(50) FOREIGN KEY REFERENCES [BedTypes](BedType) NOT NULL,
	[Rate] DECIMAL(5,2) NOT NULL,
	[RoomStatus] VARCHAR(50) FOREIGN KEY REFERENCES [RoomStatus](RoomStatus) NOT NULL,
	[Notes] NVARCHAR(1000)
)

CREATE TABLE [Payments]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES [Employees](Id) NOT NULL,
	[PaymentDate] DATE NOT NULL,
	[AccountNumber] INT FOREIGN KEY REFERENCES [Customers](AccountNumber) NOT NULL,
	[FirstDateOccupied] DATE NOT NULL,
	[LastDateOccupied] DATE NOT NULL,
	[TotalDays] INT NOT NULL,
	[AmountCharged] DECIMAL(6, 2) NOT NULL,
	[TaxRate] DECIMAL(4, 2) NOT NULL,
	[TaxAmount] DECIMAL(6, 2) NOT NULL,
	[PaymentTotal] DECIMAL(6, 2) NOT NULL,
	[Notes] NVARCHAR(1000)

)

CREATE TABLE [Occupancies]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[EmployeeId] INT FOREIGN KEY REFERENCES [Employees](Id) NOT NULL,
	[DateOccupied] DATE NOT NULL,
	[AccountNumber] INT FOREIGN KEY REFERENCES [Customers](AccountNumber) NOT NULL,
	[RoomNumber] INT FOREIGN KEY REFERENCES [Rooms](RoomNumber) NOT NULL,
	[RateApplied] DECIMAL(4, 2) NOT NULL,
	[PhoneCharge] DECIMAL(4, 2) NOT NULL,
	[Notes] NVARCHAR(1000)
)

INSERT INTO [Employees] 
	VALUES
	('Walter', 'White', 'Cook', NULL),
	('Jesse', 'Pinkman', 'Comic_Relief', NULL),
	('Tony', 'Stark', 'Iron_Man', NULL)

INSERT INTO [Customers]
	VALUES
	('James', 'Jameson', 0884950153, 'MisterWhite',0993219593,NULL),
	('Noah', 'Noahson', 0886225474, 'MisterWhite',0993219593,NULL),
	('Gus', 'Fring', 0876312001, 'MisterWhite',0993219593,NULL)

INSERT INTO [RoomStatus]
	VALUES
	('Free', NULL),
	('Occupied', NULL),
	('In Construction', NULL)

INSERT INTO [RoomTypes]
	VALUES
	('Cheap', NULL),
	('Expensive', NULL),
	('Awful', NULL)

INSERT INTO [BedTypes]
	VALUES
	('Small', NULL),
	('Big', NULL),
	('KingSized', NULL)

INSERT INTO [Rooms] 
	VALUES
	('Cheap', 'Big', 65.00, 'Free', NULL),
	('Expensive', 'Small', 15.50, 'Occupied', NULL),
	('Awful', 'KingSized', 30.35, 'In Construction', NULL)

INSERT INTO [Payments] VALUES
	(1, '2021-03-01', 1, '2020-01-01', '2023-03-15', 1, 650.00, 10.00, 20.00, 600.00, NULL),
	(2, '2022-04-02', 2, '2020-01-02', '2023-05-25', 2, 699.30, 40.00, 39.92, 669.88, NULL),
	(3, '2020-05-03', 3, '2020-01-03', '2023-06-28', 3, 630.65, 60.00, 56.15, 346.60, NULL)
	   	
INSERT INTO [Occupancies] VALUES
	(1, '2020-01-01', 1, 1, 10.00, 10.00, NULL),
	(2, '2021-01-02', 2, 2, 20.00, 16.50, NULL),
	(3, '2022-01-03', 3, 3, 30.00, 19.40, NULL)

--16

CREATE DATABASE [SoftUni]
USE [SoftUni]

CREATE TABLE [Towns]
(
	[Id] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Addresses]
(
	[Id] INT IDENTITY NOT NULL,
	[AddressText] VARCHAR(50) NOT NULL,
	[TownId] INT NOT NULL
)

CREATE TABLE [Departments]
(
	[Id] INT IDENTITY NOT NULL,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Employees]
(
	[Id] INT IDENTITY NOT NULL,
	[FirstName] VARCHAR(50) NOT NULL,
	[MiddleName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[JobTitle] VARCHAR(50) NOT NULL,
	[DepartmentId] INT NOT NULL,
	[HireDate] DATE NOT NULL,
	[Salary] DECIMAL(7, 2) NOT NULL,
	[AddressId] INT
)

ALTER TABLE [Towns]
	ADD CONSTRAINT PK_Towns
	PRIMARY KEY(Id)

ALTER TABLE [Addresses]
	ADD CONSTRAINT PK_Addresses 
	PRIMARY KEY (Id)

ALTER TABLE [Addresses]
	ADD CONSTRAINT FK_Addresses_TownId 
	FOREIGN KEY (TownId) REFERENCES [Towns](Id)

ALTER TABLE [Departments]
	ADD CONSTRAINT PK_Departments 
	PRIMARY KEY (Id)

ALTER TABLE [Employees]
	ADD CONSTRAINT PK_Employees 
	PRIMARY KEY (Id)

ALTER TABLE [Employees]
	ADD CONSTRAINT FK_Employees_DepartmentId 
	FOREIGN KEY (DepartmentId) REFERENCES [Departments](Id)

ALTER TABLE [Employees]
	ADD CONSTRAINT FK_Employees_AddressId 
	FOREIGN KEY (AddressId) REFERENCES [Addresses](Id)

--17

USE [master]

BACKUP DATABASE [SoftUni]
	TO DISK = 'D:\my documents\SQL Server\MSSQL16.SQLEXPRESS\MSSQL/Backup\SoftUniDB.bak' 

GO

DROP DATABASE [SoftUni]

GO

RESTORE DATABASE [SoftUni] 
	FROM DISK = 'D:\my documents\SQL Server\MSSQL16.SQLEXPRESS\MSSQL/Backup\SoftUniDB.bak' 

--18

USE [SoftUni]

INSERT INTO [Towns] VALUES
	('Sofia'),
	('Plovdiv'),
	('Varna'),
	('Burgas')
	
INSERT INTO [Departments] VALUES
	('Engineering'),
	('Sales'),
	('Marketing'),
	('Software Development'),
	('Quality Assurance')

INSERT INTO [Addresses] VALUES
	('Default entry', 1)

INSERT INTO [Employees] VALUES
	('Ivan', 'Ivanov', 'Ivanov', '.NET Developer',4,'2013-02-01',3500.00,1),
	('Petar','Petrov','Petrov','Senior Engineer',1,'2004-03-02',4000.00,1),
	('Maria', 'Petrova', 'Ivanova', 'Intern',5, '2016-08-28', 525.25,1),
	('Georgi','Teziev','Ivanov','CEO',2,'2007-12-09',3000.00,1),
	('Peter','Pan','Pan','Intern',3,'2016-08-28',599.88,1)

--19

SELECT * FROM [Towns]	

SELECT * FROM [Departments]	

SELECT * FROM [Employees]

--20

SELECT * FROM [Towns]	
	ORDER BY [Name]

SELECT * FROM [Departments]	
	ORDER BY [Name]

SELECT * FROM [Employees]
	ORDER BY [Salary] DESC

--21

SELECT [Name] FROM [Towns]	
	ORDER BY [Name]

SELECT [Name] FROM [Departments]	
	ORDER BY [Name]

SELECT [FirstName], [LastName], [JobTitle], [Salary] FROM [Employees]
	ORDER BY [Salary] DESC

--22

UPDATE [Employees]
	SET [Salary] *= 1.1

SELECT [Salary] FROM [Employees]

--23

USE [Hotel]

UPDATE [Payments]
	SET [TaxRate] -= 0.3

SELECT [TaxRate] FROM [Payments]

--24

DELETE [Occupancies]

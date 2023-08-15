--1

CREATE DATABASE TableRelations

USE  [TableRelations]

CREATE TABLE [Passports]
(
	PassportID INT PRIMARY KEY IDENTITY(101,1) NOT NULL,
	PassportNumber CHAR(8) NOT NULL
)

INSERT INTO [Passports] 
	VALUES
	('N34FG21B'),
	('K65LO4R7'),
	('ZE657QP2')

CREATE TABLE [Persons]
(
	PersonID INT IDENTITY NOT NULL,
	FirstName NVARCHAR(50) NOT NULL,
	Salary DECIMAL(7,2) NOT NULL,
	PassportID INT NOT NULL
)

ALTER TABLE [Persons]
ADD CONSTRAINT PK_PersonId
PRIMARY KEY (PersonID)

ALTER TABLE [Persons]
ADD CONSTRAINT FK_Persons_Passports
FOREIGN KEY (PassportId) 
REFERENCES [Passports](PassportId)

INSERT INTO [Persons] 
	VALUES
	('Roberto', 43300.00, 102),
	('Tom', 56100.00, 103),
	('Yana', 60200.00, 101)

--2

CREATE TABLE [Manufacturers]
(
	[ManufacturerID] INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL,
	[EstablishedOn] DATE NOT NULL
)

INSERT INTO [Manufacturers] 
	VALUES
	('BMW', '1916-03-07'),
	('Tesla', '2003-01-01'),
	('Lada', '1966-05-01')

CREATE TABLE [Models]
(
	[ModelID] INT PRIMARY KEY IDENTITY(101, 1),
	[Name] VARCHAR(50) NOT NULL,
	[ManufacturerID] INT FOREIGN KEY REFERENCES [Manufacturers](ManufacturerID),
)

INSERT INTO [Models] VALUES
	('X1', 1),
	('i6', 1),
	('Model S', 2),
	('Model X', 2),
	('Model 3', 2),
	('Nova', 3)

--3

USE [TableRelations]

CREATE TABLE [Students]
(
	StudentID INT PRIMARY KEY IDENTITY,
	[Name] NVARCHAR(50) NOT NULL
)

INSERT INTO [Students] VALUES
	('Mila'),
	('Toni'),
	('Ron')

CREATE TABLE [Exams]
(
	ExamID INT PRIMARY KEY IDENTITY(101,1),
	[Name] NVARCHAR(50) NOT NULL
)

INSERT INTO [Exams] VALUES
	('SpringMVC'),
	('Neo4j'),
	('Oracle 11g')

CREATE TABLE [StudentsExams]
(
	StudentID INT NOT NULL FOREIGN KEY REFERENCES [Students](StudentID),
	ExamID INT NOT NULL FOREIGN KEY REFERENCES [Exams](ExamID),
	CONSTRAINT PK_StudentsExams 
	PRIMARY KEY (StudentID, ExamID)
)

INSERT INTO [StudentsExams] 
	VALUES
	(1, 101),
	(1, 102),
	(2, 101),
	(3, 103),
	(2, 102),
	(2, 103)

--4

CREATE TABLE [Teachers]
(
	[TeacherID] INT IDENTITY(101, 1) PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[ManagerID] INT
	FOREIGN KEY REFERENCES [Teachers](TeacherID)
)

	INSERT INTO [Teachers] VALUES
	('John', NULL),
	('Maya', 106),
	('Silvia', 106),
	('Ted', 105),
	('Mark', 101),
	('Greta', 101)

--5

CREATE DATABASE OnlineStore

USE [OnlineStore]

CREATE TABLE [ItemTypes]
(
 [ItemTypeID] INT PRIMARY KEY IDENTITY,
 [Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Items]
(
	[ItemID] INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	[ItemTypeID] INT FOREIGN KEY REFERENCES [ItemTypes](ItemTypeID) NOT NULL
)

CREATE TABLE [Cities]
(
 [CityID] INT PRIMARY KEY IDENTITY,
 [Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Customers]
(
	[CustomerID] INT PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL,
	[Birthday] DATE NOT NULL,
	[CityID] INT FOREIGN KEY REFERENCES [Cities](CityID) NOT NULL 
)

CREATE TABLE [Orders]
(
	[OrderID] INT PRIMARY KEY,
	[CustomerID] INT FOREIGN KEY REFERENCES [Customers](CustomerID) NOT NULL
)

CREATE TABLE [OrderItems]
(
	[OrderID] INT FOREIGN KEY REFERENCES [Orders](OrderID) NOT NULL,
	[ItemID] INT FOREIGN KEY REFERENCES [Items](ItemID) NOT NULL,
	CONSTRAINT PK_OrderItems
		PRIMARY KEY (OrderID, ItemID)
)

--6

CREATE TABLE [Majors]
(
	[MajorID] INT NOT NULL PRIMARY KEY,
	[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE [Subjects]
(
	[SubjectID] INT NOT NULL PRIMARY KEY,
	[SubjectName] VARCHAR(50) NOT NULL
)

CREATE TABLE [Students]
(
	[StudentID] INT NOT NULL PRIMARY KEY,
	[StudentNumber] INT NOT NULL,
	[StudentName] VARCHAR(50) NOT NULL,
	[MajorID] INT FOREIGN KEY
		REFERENCES [Majors](MajorID)
)

CREATE TABLE [Agenda]
(
	[StudentID] INT NOT NULL FOREIGN KEY
		REFERENCES [Students](StudentID),
	[SubjectID] INT NOT NULL FOREIGN KEY
		REFERENCES [Subjects](SubjectID)
	CONSTRAINT PK_Agenda 
		PRIMARY KEY (StudentID, SubjectID)
)

CREATE TABLE [Payments]
(
	[PaymentID] INT NOT NULL PRIMARY KEY,
	[PaymentDate] DATE NOT NULL,
	[PaymentAmount] DECIMAL(6, 2),
	[StudentID] INT NOT NULL FOREIGN KEY
		REFERENCES [Students](StudentID)
)

--9

USE [Geography]

SELECT m.MountainRange, p.PeakName, p.Elevation
FROM [Peaks] AS p
JOIN [Mountains] AS m
ON p.MountainId = m.Id
WHERE m.MountainRange = 'Rila'
ORDER BY p.Elevation DESC	

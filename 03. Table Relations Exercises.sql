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

	INSERT INTO [Teachers] VALUES
	('John', NULL),
	('Maya', 106),
	('Silvia', 106),
	('Ted', 105),
	('Mark', 101),
	('Greta', 101)
)
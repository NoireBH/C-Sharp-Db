CREATE DATABASE Countries
GO

USE Countries
GO

CREATE TABLE Capitals
(
	Id INT PRIMARY KEY IDENTITY
	,[Name] NVARCHAR(50)
	,[Population] INT
)

CREATE TABLE Countries
(
	Id INT PRIMARY KEY IDENTITY
	,[Name] NVARCHAR(50)
	,[Population] INT
	,CountryCode NVARCHAR(3),
	CapitalId INT  FOREIGN KEY REFERENCES Capitals(Id)
)

CREATE TABLE Cities
(
	Id INT PRIMARY KEY IDENTITY
	,[Name] NVARCHAR(50)
	,[Population] INT
	,CountryId INT FOREIGN KEY REFERENCES Countries(Id)
)

CREATE TABLE Attractions
(
	Id INT PRIMARY KEY IDENTITY
	,[Name] NVARCHAR(50)
	,Notes NVARCHAR(250)
	,CityId INT FOREIGN KEY REFERENCES Cities(Id)
)

INSERT INTO Capitals([Name],[Population]) VALUES
	('Sofia', 1200000),
	('London', 1300000)

INSERT INTO Countries([Name],[Population],CountryCode,CapitalId) VALUES
	('Bulgaria',6000000,'BG',1),
	('Britain', 6700000, 'UK',2)
-- Databases MSSQL Server Exam - 19 Feb 2023

--1

CREATE DATABASE Boardgames
GO
USE Boardgames
GO
CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY
	,[Name] VARCHAR(50) NOT NULL
)

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY
	,StreetName NVARCHAR(100) NOT NULL
	,StreetNumber INT NOT NULL
	,Town VARCHAR(30) NOT NULL
	,Country VARCHAR(50) NOT NULL
	,ZIP INT NOT NULL
)

CREATE TABLE Publishers
(
	Id INT PRIMARY KEY IDENTITY
	,Name NVARCHAR(30) UNIQUE NOT NULL
	,AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
	,Website NVARCHAR(40) 
	,Phone NVARCHAR(20) 
)

CREATE TABLE PlayersRanges
(
	Id INT PRIMARY KEY IDENTITY
	,PlayersMin INT NOT NULL
	,PlayersMax INT NOT NULL
)

CREATE TABLE Boardgames
(
	Id INT PRIMARY KEY IDENTITY
	,Name NVARCHAR(30) UNIQUE NOT NULL
	,YearPublished INT NOT NULL
	,Rating DECIMAL(20,2) NOT NULL
	,CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL
	,PublisherId INT FOREIGN KEY REFERENCES Publishers(Id) NOT NULL
	,PlayersRangeId INT FOREIGN KEY REFERENCES PlayersRanges(Id) NOT NULL
)

CREATE TABLE Creators
(
	Id INT PRIMARY KEY IDENTITY
	,FirstName NVARCHAR(30) NOT NULL
	,LastName NVARCHAR(30) NOT NULL
	,Email NVARCHAR(30) NOT NULL
)

CREATE TABLE CreatorsBoardgames
(
	CreatorId INT
	,BoardgameId INT
	,CONSTRAINT PK_CreatorsBoardgames PRIMARY KEY (CreatorId, BoardgameId)
	, CONSTRAINT FK_CreatorsBoardgames_Creators  FOREIGN KEY (CreatorId) REFERENCES Creators(Id)
	, CONSTRAINT FK_CreatorsBoardgames_Boardgames FOREIGN KEY (BoardgameId) REFERENCES Boardgames(Id)
)

--2

INSERT INTO Boardgames(Name,YearPublished,Rating,CategoryId,PublisherId,PlayersRangeId)
VALUES
	('Deep Blue', 2019, 5.67, 1, 15 ,7)
	,('Paris', 2016, 9.78, 7, 1, 5)
	,('Catan: Starfarers', 2021, 9.87, 7, 13, 6)
	,('Bleeding Kansas', 2020, 3.25, 3 ,7, 4)
	,('One Small Step', 2019, 5.75, 5, 9, 2)

INSERT INTO Publishers(Name,AddressId,Website,Phone)
VALUES
	('Agman Games', 5, 'www.agmangames.com', '+16546135542')
	,('Amethyst Games', 7, 'www.amethystgames.com', '+15558889992')
	,('BattleBooks', 13, 'www.battlebooks.com', '+12345678907')


--3

UPDATE PlayersRanges 
SET PlayersMax +=1 
WHERE Id=1 

UPDATE Boardgames 
SET [Name]=[Name] + 'V2'
WHERE YearPublished>=2020

--4

DELETE FROM CreatorsBoardgames WHERE BoardgameId IN (1,16,31,47)
DELETE FROM Boardgames WHERE PublisherId IN (1,16)
DELETE FROM Publishers WHERE AddressId IN (5)
DELETE FROM Addresses WHERE SUBSTRING(Town, 1, 1) = 'L'

--5

SELECT
	bg.Name
	,bg.Rating
FROM Boardgames AS bg
ORDER BY bg.YearPublished ASC, bg.Name DESC

--6

SELECT
	bg.Id,
	bg.Name,
	bg.YearPublished,
	c.Name AS CategoryName
FROM Boardgames AS bg
JOIN Categories AS c ON c.Id = bg.CategoryId
WHERE bg.CategoryId IN (6,8)
ORDER BY bg.YearPublished DESC

--7

SELECT
	c.Id
	,CONCAT(c.FirstName, ' ', c.LastName) AS CreatorName
	,c.Email
FROM Creators AS c
LEFT JOIN CreatorsBoardgames AS cbg ON cbg.CreatorId = c.Id
WHERE cbg.CreatorId IS NULL

--8

SELECT TOP(5)
	bg.Name
	,bg.Rating
	,c.Name AS CategoryName
FROM Boardgames AS bg
JOIN PlayersRanges AS pr ON pr.Id = bg.PlayersRangeId
JOIN Categories AS c ON c.Id = bg.CategoryId
WHERE bg.Rating > 7 AND bg.Name LIKE '%a%'
OR bg.Rating > 7.50 AND pr.PlayersMin = 2 AND pr.PlayersMax = 5
ORDER BY bg.Name ASC, bg.Rating DESC

--9

SELECT
	CONCAT(c.FirstName, ' ', c.LastName) AS FullName
	,c.Email
	,MAX(bg.Rating)
FROM Creators AS c
JOIN CreatorsBoardgames AS cbg ON c.Id = cbg.CreatorId
JOIN Boardgames AS bg ON cbg.BoardgameId = bg.Id
WHERE c.Email LIKE '%.com%'
GROUP BY CONCAT(c.FirstName, ' ', c.LastName),Email
ORDER BY FullName

--10

SELECT
	c.LastName
	,CEILING(AVG(bg.Rating)) AS AverageRating
	,p.Name
FROM Creators AS c
JOIN CreatorsBoardgames AS cbg ON c.Id = cbg.CreatorId
JOIN Boardgames AS bg ON cbg.BoardgameId = bg.Id
JOIN Publishers AS p ON bg.PublisherId = p.Id
WHERE cbg.CreatorId IS NOT NULL 
and p.Name='Stonemaier Games'
GROUP BY c.LastName,p.Name
ORDER BY AVG(bg.Rating) DESC

--11


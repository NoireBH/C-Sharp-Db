CREATE DATABASE TouristAgency
GO
USE TouristAgency

--1

CREATE TABLE Countries
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(50) NOT NULL
)

CREATE TABLE Destinations
(
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(50) NOT NULL,
	CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Rooms
(
	Id INT PRIMARY KEY IDENTITY,
	[Type] VARCHAR(40) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	BedCount INT NOT NULL CHECK (
   BedCount >= 1 AND BedCount <= 10)
)

CREATE TABLE Hotels
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(80) NOT NULL,
	DestinationId INT FOREIGN KEY REFERENCES Destinations(Id) NOT NULL
)

CREATE TABLE Tourists
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(80) NOT NULL,
	PhoneNumber VARCHAR(20) NOT NULL,
	Email VARCHAR(80),
	CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
)

CREATE TABLE Bookings
(
	Id INT PRIMARY KEY IDENTITY,
	ArrivalDate DATETIME2 NOT NULL,
	DepartureDate DATETIME2 NOT NULL,
	AdultsCount INT CHECK (
   AdultsCount >= 1 AND AdultsCount <= 10) NOT NULL,
   ChildrenCount INT CHECK (
   ChildrenCount >= 0 AND ChildrenCount <= 10) NOT NULL,
   TouristId INT FOREIGN KEY REFERENCES Tourists(Id) NOT NULL,
   HotelId INT FOREIGN KEY REFERENCES Hotels(Id) NOT NULL,
   RoomId INT FOREIGN KEY REFERENCES Rooms(Id) NOT NULL
)

CREATE TABLE HotelsRooms
(
	HotelId INT NOT NULL,
	RoomId INT NOT NULL

	CONSTRAINT PK_HotelsRooms PRIMARY KEY (HotelId, RoomId),
	CONSTRAINT FK_HotelsRooms_Hotels FOREIGN KEY (HotelId) REFERENCES Hotels(Id),
	CONSTRAINT FK_HotelsRooms_Rooms FOREIGN KEY (RoomId) REFERENCES Rooms(Id)
)

--2

INSERT INTO Tourists(Name,PhoneNumber,Email,CountryId) 
VALUES

	('John Rivers', '653-551-1555', 'john.rivers@example.com', 6),
	('Adeline Aglaé', '122-654-8726', 'adeline.aglae@example.com', 2),
	('Sergio Ramirez', '233-465-2876', 's.ramirez@example.com', 3),
	('Johan Müller', '322-876-9826', 'j.muller@example.com', 7),
	('Eden Smith', '551-874-2234', 'eden.smith@example.com', 6)

INSERT INTO Bookings(ArrivalDate,DepartureDate,AdultsCount,ChildrenCount,TouristId,HotelId,RoomId) 
VALUES

	('2024-03-01', '2024-03-11', 1, 0, 21, 3 ,5),
	('2023-12-28', '2024-01-06', 2, 1, 22, 13 ,3),
	('2023-11-15', '2023-11-20', 1, 2, 23, 19 ,7),
	('2023-12-05', '2023-12-09', 4, 0, 24, 6 ,4),
	('2024-05-01', '2024-05-07', 6, 0, 25, 14 ,6)
	

--3

UPDATE Bookings
SET DepartureDate = Dateadd(day, 1, DepartureDate)
WHERE MONTH(DepartureDate) = 12

UPDATE Tourists
SET Email = NULL
WHERE Name LIKE '%MA%'

--4

DELETE FROM Bookings WHERE TouristId IN(6,16,25)
DELETE FROM Tourists WHERE Name LIKE '%Smith%'

--5

SELECT
	FORMAT(b.ArrivalDate, 'yyyy-MM-dd'),
	b.AdultsCount,
	b.ChildrenCount
FROM Bookings as b
JOIN Rooms AS r ON r.Id = b.RoomId
ORDER BY r.Price DESC, b.ArrivalDate ASC

--6

SELECT 
	t.HotelId,
	t.Name
FROM
(
	SELECT
	BookingCount = COUNT(b.HotelId),
	BookingId = r.Id,
	[Name] = h.Name,
	HotelId = h.Id
FROM Hotels AS h
JOIN HotelsRooms AS hr ON hr.HotelId = h.Id
JOIN Rooms AS r ON r.Id = hr.RoomId
JOIN Bookings AS b on b.RoomId = r.Id
GROUP BY  b.Id, h.Name,h.Id, r.Id
) AS  t
WHERE t.BookingId = 8
GROUP BY BookingId, t.Name, t.BookingCount, t.HotelId
ORDER BY t.BookingCount DESC 



SELECT
	h.Id,
	h.Name
FROM Hotels AS h
JOIN HotelsRooms AS hr ON hr.HotelId = h.Id
JOIN Rooms AS r ON r.Id = hr.RoomId
JOIN Bookings AS b on b.RoomId = r.Id
WHERE r.Id = 8
GROUP BY h.Id, h.Name
ORDER BY COUNT(b.Id) DESC


SELECT
	b.hotelId
FROM Hotels AS h
JOIN HotelsRooms AS hr ON hr.HotelId = h.Id
JOIN Rooms AS r ON r.Id = hr.RoomId
JOIN Bookings AS b on b.RoomId = r.Id
GROUP BY b.HotelId
ORDER BY COUNT(*)  DESC

--7

SELECT
	t.Id,
	t.Name,
	t.PhoneNumber
FROM Tourists AS t
LEFT JOIN Bookings AS h ON h.TouristId = t.Id
WHERE h.TouristId IS NULL
ORDER BY t.Name

--8

SELECT
	*
FROM
(
	SELECT
	TOP(10)
	BookingId = h.Id,
	b.ArrivalDate,
	 HotelName = h.Name,
	 DestinationName = d.Name,
	 CountryName = c.Name
FROM Bookings AS b
JOIN Hotels AS h ON h.Id = b.HotelId
JOIN Tourists AS t ON t.Id = b.TouristId
JOIN Countries AS c ON c.Id = t.CountryId
JOIN Destinations AS d ON d.CountryId = c.Id
) AS t
WHERE  t.ArrivalDate < '2023-12-31' AND t.BookingId % 2 = 0
ORDER BY t.CountryName, t.ArrivalDate



SELECT
	TOP(10)
	*
FROM Bookings AS b
JOIN Hotels AS h ON h.Id = b.HotelId
WHERE  FORMAT(b.ArrivalDate, 'yyyy-MM-dd') < '2023-12-31'

--9

SELECT
	HotelName = h.Name,
	RoomPrice = r.Price
FROM Tourists AS t
JOIN Bookings AS b ON b.TouristId = t.Id
JOIN Hotels AS h ON h.Id = b.HotelId
JOIN Rooms AS r ON r.Id = b.RoomId
WHERE RIGHT(t.Name,2) NOT LIKE '%EZ%'
ORDER BY r.Price DESC

--10

SELECT
	h.Name,
	HotelRevenue = SUM(DATEDIFF(DAY, b.ArrivalDate, b.DepartureDate) * r.Price)
FROM Hotels AS h
JOIN Bookings AS b ON b.HotelId = h.Id
JOIN Rooms AS r ON r.Id = b.RoomId
GROUP BY h.Name
ORDER BY HotelRevenue DESC

--11
GO
CREATE FUNCTION udf_RoomsWithTourists(@name NVARCHAR(100))
RETURNS INT 
AS
BEGIN
DECLARE @TotalNumOfTourists INT =
	(
		SELECT 
			SUM(b.AdultsCount) + SUM(b.ChildrenCount)
		FROM Rooms AS r
		JOIN Bookings AS b ON b.RoomId = r.Id
		JOIN Tourists AS t ON t.Id = b.TouristId
		WHERE r.Type = @name

		
	)

	RETURN @TotalNumOfTourists
END

--12
GO
CREATE PROCEDURE usp_SearchByCountry(@country NVARCHAR(50)) 
AS
	SELECT 
		t.Name,
		t.PhoneNumber,
		t.Email,
		COUNT(b.TouristId)
	FROM Tourists AS t
	JOIN Bookings AS b ON b.TouristId = t.Id
	JOIN Countries AS c ON c.Id = t.CountryId
		WHERE b.TouristId IS NOT NULL AND c.Name = @country
			GROUP BY t.Name,t.PhoneNumber, t.Email
	ORDER BY t.Name, COUNT(b.TouristId) DESC

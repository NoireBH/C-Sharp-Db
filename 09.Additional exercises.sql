USE Diablo
--1

SELECT
	subquery.[Email Provider],
	COUNT([Email Provider]) AS [Number Of Users]
FROM
(
	SELECT
	SUBSTRING(Email,CHARINDEX('@',Email) + 1,LEN(Email)) AS [Email Provider]
FROM [Users]
) AS subquery
GROUP BY [Email Provider]
ORDER BY [Number Of Users] DESC, [Email Provider]

--2

SELECT
	g.[Name],
	gt.[Name],
	u.[Username],
	ug.[Level],
	ug.[Cash],
	c.[Name]
FROM [Games] AS g
JOIN [GameTypes] AS gt ON gt.Id = g.GameTypeId
JOIN [UsersGames] AS ug ON ug.GameId = g.Id
JOIN [Users] AS u ON u.Id = ug.UserId
JOIN [Characters] AS c ON c.Id = ug.CharacterId
ORDER BY ug.[Level] DESC, u.[Username], g.[Name]

--3

SELECT
	u.[Username],
	g.[Name] AS [Game],
	COUNT(ugi.[ItemId]) AS [Items Count],
	SUM(i.[Price]) AS [Items Price]
FROM [Users] AS u
JOIN [UsersGames] AS ug ON ug.UserId = u.Id
JOIN [Games] AS g ON g.Id = ug.GameId
JOIN [UserGameItems] AS ugi ON ugi.UserGameId = ug.Id
JOIN [Items] AS i ON i.Id = ugi.ItemId
GROUP BY u.[Username],g.[Name]
HAVING COUNT(ugi.[ItemId]) >= 10
ORDER BY [Items Count] DESC, [Items Price] DESC, Username ASC

--4

SELECT
	u.Username,
	g.[Name],
	MAX(c.Name) as [Character],
	SUM(sts.Strength) + MAX(st.Strength) + MAX(cs.Strength) as Strength,
	SUM(sts.Defence) + MAX(st.Defence) + MAX(cs.Defence) as Defence,
	SUM(sts.Speed) + MAX(st.Speed) + MAX(cs.Speed) as Speed,
	SUM(sts.Mind) + MAX(st.Mind) + MAX(cs.Mind) as Mind,
	SUM(sts.Luck) + MAX(st.Luck) + MAX(cs.Luck) as luck
FROM Users AS u
JOIN UsersGames AS ug ON u.Id = ug.UserId
JOIN Games AS g ON ug.GameId = g.Id
JOIN GameTypes gt ON gt.Id = g.GameTypeId
JOIN [Statistics] AS st ON st.Id = gt.BonusStatsId
JOIN Characters AS c ON ug.CharacterId = c.Id
JOIN [Statistics] cs ON cs.Id = c.StatisticId
JOIN UserGameItems AS ugi ON ugi.UserGameId = ug.Id
JOIN items AS i ON i.Id = ugi.ItemId
JOIN [Statistics] AS sts ON sts.Id = i.StatisticId
GROUP BY u.Username, g.[Name]
ORDER BY Strength DESC, Defence DESC, Speed DESC, Mind DESC, Luck DESC

--5

SELECT
	[Name],
	Price,
	MinLevel,
	st.Strength,
	st.Defence,
	st.Speed,
	st.Luck,
	st.Mind
FROM Items AS i
JOIN [Statistics] AS st ON st.Id = i.StatisticId
WHERE st.Mind > (select AVG(CAST(st.Mind as bigint)) from [Statistics] AS st ) 
and st.Speed > (select AVG(CAST(st.Speed as bigint)) from [Statistics] AS st ) 
and st.Luck > (select AVG(CAST(st.Luck as bigint)) from [Statistics] AS st) 
ORDER BY i.[Name]

--6

SELECT
	i.[Name] AS Item,
	i.Price,
	i.MinLevel,
	gt.[Name] AS [Forbidden Game Type]
FROM [Items] AS i
LEFT JOIN GameTypeForbiddenItems AS fi ON i.Id = fi.ItemId
LEFT JOIN GameTypes AS gt ON fi.GameTypeId = gt.Id
ORDER BY gt.[Name] DESC, i.[Name]

--7

DECLARE @userId INT = (SELECT Id FROM Users WHERE Username = 'Alex')
DECLARE @gameId INT = (SELECT Id FROM Games WHERE Name = 'Edinburgh')
DECLARE @userGameId INT = (SELECT Id FROM UsersGames WHERE UserId = @userId AND GameId = @gameId)

DECLARE @ItemId1 INT = (SELECT Id FROM Items WHERE Name = 'Blackguard')
DECLARE @ItemId2 INT = (SELECT Id FROM Items WHERE Name = 'Bottomless Potion of Amplification')
DECLARE @ItemId3 INT = (SELECT Id FROM Items WHERE Name = 'Eye of Etlich (Diablo III)')
DECLARE @ItemId4 INT = (SELECT Id FROM Items WHERE Name = 'Gem of Efficacious Toxin')
DECLARE @ItemId5 INT = (SELECT Id FROM Items WHERE Name = 'Golden Gorget of Leoric')
DECLARE @ItemId6 INT = (SELECT Id FROM Items WHERE Name = 'Hellfire Amulet')

DECLARE @totalCost MONEY =
(
	SELECT
		SUM(Price)
	FROM Items
	WHERE Id IN (@ItemId1, @ItemId2, @ItemId3, @ItemId4, @ItemId5, @ItemId6)
)

UPDATE UsersGames
SET Cash -= @totalCost
WHERE Id = @userGameId

INSERT INTO UserGameItems 
	VALUES
	(@ItemId1, @userGameId),
	(@ItemId2, @userGameId),
	(@ItemId3, @userGameId),
	(@ItemId4, @userGameId),
	(@ItemId5, @userGameId),
	(@ItemId6, @userGameId)

SELECT
	u.Username,
	g.[Name],
	ug.Cash,
	i.[Name] AS [Item Name]
FROM Users AS u
JOIN UsersGames AS ug ON u.Id = ug.UserId
JOIN Games AS g ON ug.GameId = g.Id
JOIN UserGameItems AS ugi ON ug.Id = ugi.UserGameId
JOIN Items AS i ON ugi.ItemId = i.Id
WHERE g.Id = @gameId
ORDER BY i.[Name]

--8
USE [Geography]
SELECT
	p.PeakName,
	m.MountainRange AS Mountain,
	p.Elevation
FROM Peaks AS p
JOIN Mountains AS m ON p.MountainId = m.Id
ORDER BY p.Elevation DESC, p.PeakName

--9

SELECT
	p.PeakName,
	m.MountainRange,
	c.CountryName,
	con.ContinentName
FROM Peaks AS p
JOIN Mountains AS m ON m.Id = p.MountainId
JOIN MountainsCountries AS mc ON mc.MountainId = m.Id
JOIN Countries AS c ON c.CountryCode = mc.CountryCode
JOIN Continents AS con ON con.ContinentCode = c.ContinentCode
ORDER BY PeakName,c.CountryName

--10

SELECT
	c.CountryName,
	co.ContinentName,
	COUNT(cr.RiverId) AS RiversCount,
	CASE
		WHEN SUM(r.Length) IS NULL THEN 0
		ELSE SUM(r.length)
	END AS TotalLength
FROM Countries AS c
LEFT JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
LEFT JOIN Rivers AS r ON r.Id = cr.RiverId
LEFT JOIN Continents AS co ON co.ContinentCode = c.ContinentCode
GROUP BY c.CountryName,co.ContinentName
ORDER BY RiversCount DESC, TotalLength DESC, CountryName

--11

SELECT
	cur.CurrencyCode,
	cur.Description AS Currency,
	COUNT(*) AS NumberOfCountries
FROM Currencies cur
LEFT JOIN Countries AS c ON c.CurrencyCode = cur.CurrencyCode
GROUP BY cur.CurrencyCode,cur.Description
ORDER BY NumberOfCountries DESC, Currency

--12

SELECT
	c.ContinentName,
	SUM(CAST(con.AreaInSqKm AS BIGINT)) AS CountriesArea,
	SUM(CAST(con.Population AS BIGINT)) AS CountriesPopulation
FROM Continents AS c
JOIN Countries AS con ON c.ContinentCode = con.ContinentCode
GROUP BY c.ContinentName
ORDER BY CountriesPopulation DESC

--13

CREATE TABLE Monasteries
(
    Id INT IDENTITY,
    Name VARCHAR(50),
    CountryCode CHAR(2) FOREIGN KEY REFERENCES Countries(CountryCode) NOT NULL
)

INSERT INTO Monasteries(Name, CountryCode) VALUES
    ('Rila Monastery “St. Ivan of Rila”', 'BG'), 
    ('Bachkovo Monastery “Virgin Mary”', 'BG'),
    ('Troyan Monastery “Holy Mother''s Assumption”', 'BG'),
    ('Kopan Monastery', 'NP'),
    ('Thrangu Tashi Yangtse Monastery', 'NP'),
    ('Shechen Tennyi Dargyeling Monastery', 'NP'),
    ('Benchen Monastery', 'NP'),
    ('Southern Shaolin Monastery', 'CN'),
    ('Dabei Monastery', 'CN'),
    ('Wa Sau Toi', 'CN'),
    ('Lhunshigyia Monastery', 'CN'),
    ('Rakya Monastery', 'CN'),
    ('Monasteries of Meteora', 'GR'),
    ('The Holy Monastery of Stavronikita', 'GR'),
    ('Taung Kalat Monastery', 'MM'),
    ('Pa-Auk Forest Monastery', 'MM'),
    ('Taktsang Palphug Monastery', 'BT'),
    ('Sümela Monastery', 'TR')

ALTER TABLE Countries
ADD IsDeleted BIT 

UPDATE Countries
SET IsDeleted = 0

UPDATE [Countries]
SET [IsDeleted] = 1
WHERE CountryCode IN
(
	SELECT
	c.CountryCode
	FROM Countries AS c
	JOIN CountriesRivers AS cr ON cr.CountryCode = c.CountryCode
	GROUP BY c.CountryCode
	HAVING COUNT(cr.RiverId) > 3
)

SELECT 
    m.[Name] AS Monastery,
    c.[CountryName] AS Country
FROM [Monasteries] AS m 
JOIN [Countries] AS c ON m.[CountryCode] = c.[CountryCode]
WHERE c.[IsDeleted] = 0
ORDER BY [Monastery] ASC

UPDATE Countries
SET CountryName = 'Myanmar'


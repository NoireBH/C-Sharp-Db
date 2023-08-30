--1

CREATE PROC usp_GetEmployeesSalaryAbove35000 
AS
SELECT [FirstName], [LastName]
FROM [Employees]
WHERE [Salary] > 35000
GO

EXEC usp_GetEmployeesSalaryAbove35000

--2

CREATE PROC usp_GetEmployeesSalaryAboveNumber
(@number DECIMAL(18,4))
AS
	SELECT [FirstName], [LastName]
	FROM [Employees]
	WHERE [Salary] >= @number	
GO

EXEC usp_GetEmployeesSalaryAboveNumber 90000.4444

--3

CREATE PROC usp_GetTownsStartingWith 
(@townName VARCHAR(50))
AS
	SELECT [Name]
	AS [Town]
	FROM [Towns]
	WHERE SUBSTRING([Name],1, LEN(@townName)) = @townName
GO

EXEC usp_GetTownsStartingWith 'S'

--4

CREATE PROC usp_GetEmployeesFromTown 
(@townName VARCHAR(50))
AS
	Select [FirstName] , [LastName]
	FROM [Employees] 
		AS e
	JOIN [Addresses] 
		AS a 
		ON e.AddressID = a.AddressID
	JOIN [Towns] 
		AS t 
		ON a.TownID = t.TownID
	WHERE t.Name = @townName
GO

EXEC usp_GetEmployeesFromTown 'Sofia'

--5

CREATE FUNCTION ufn_GetSalaryLevel
(@salary DECIMAL(18,4)) 
RETURNS VARCHAR(10)
BEGIN
	IF @salary < 30000
	RETURN 'Low'
	ELSE IF @salary <= 50000
	RETURN 'Average'
	ELSE IF @salary > 50000
	RETURN 'High'
	
	RETURN NULL
END

SELECT TOP(100)
	[Salary], dbo.ufn_GetSalaryLevel([Salary])
FROM [Employees]

--6

CREATE PROC usp_EmployeesBySalaryLevel
(@levelOfSalary VARCHAR(10))
AS
	SELECT
		[FirstName],
		[LastName]
	FROM [Employees]
	WHERE dbo.ufn_GetSalaryLevel(Salary) = @levelOfSalary
GO

EXEC usp_EmployeesBySalaryLevel 'high'

--7

CREATE OR ALTER FUNCTION ufn_IsWordComprised
(@setOfLetters VARCHAR(50), @word VARCHAR(50))
RETURNS BIT
AS
BEGIN
	DECLARE @counter INT = 1
	WHILE (@counter <= LEN(@word))
		BEGIN
			IF @setOfLetters NOT LIKE '%' + SUBSTRING(@word, @counter, 1) + '%' RETURN 0
			SET @counter += 1
		END
		RETURN 1
END

--8

CREATE PROC usp_DeleteEmployeesFromDepartment 
(@departmentId INT)
AS
BEGIN
	ALTER TABLE [Departments]
	ALTER COLUMN [ManagerID] INT NULL
	
	DELETE FROM [EmployeesProjects]	
	WHERE [EmployeeID] IN
	(
		SELECT [EmployeeID] FROM [Employees]
		WHERE [DepartmentID] = @departmentId
	)

	UPDATE [Employees]
	SET [ManagerID] = NULL
	WHERE [ManagerID] IN
	(
		SELECT [EmployeeID] FROM [Employees]
		WHERE [DepartmentID] = @departmentId
	)
	
	UPDATE [Departments]
	SET [ManagerID] = NULL
	WHERE [DepartmentID] = @departmentId
	
 	DELETE FROM [Employees]
	WHERE [DepartmentID] = @departmentId

	DELETE FROM [Departments]
	WHERE [DepartmentID] = @departmentId

	SELECT COUNT(*) FROM [Employees]
	WHERE [DepartmentID] = @departmentId
END

--9

CREATE PROC usp_GetHoldersFullName 
AS
	SELECT
		CONCAT_WS(' ', [FirstName],[LastName]) AS [Full Name]
	FROM [AccountHolders]
GO

EXEC usp_GetHoldersFullName

--10

CREATE PROC usp_GetHoldersWithBalanceHigherThan
(@numberToCompare DECIMAL(18,4))
AS
	SELECT
		[FirstName],
		[LastName]
	FROM [AccountHolders] AS ah
	JOIN 
		(
			SELECT
			[AccountHolderId],
			SUM(Balance) AS [TotalSum]
			FROM [Accounts]
			GROUP BY [AccountHolderId]
		)  AS a ON a.AccountHolderId = ah.Id 
	WHERE a.[TotalSum] > @numberToCompare
	ORDER BY ah.[FirstName], ah.[LastName]
GO

EXEC usp_GetHoldersWithBalanceHigherThan 5000

--11

CREATE FUNCTION ufn_CalculateFutureValue 
(@sum DECIMAL(18,4), @yearlyInterestRate FLOAT, @numberOfYears INT) 
RETURNS DECIMAL(18, 4)
BEGIN
	RETURN @sum * POWER(1 + @yearlyInterestRate,@numberOfYears)
END

--12

CREATE PROC usp_CalculateFutureValueForAccount
(@accountId INT, @interestRate FLOAT)
AS
	SELECT
		acc.[Id] AS [Account Id],
		ah.[FirstName] AS [First Name],
		ah.[LastName] AS [Last Name],
		acc.[Balance] AS [Current Balance],
		dbo.ufn_CalculateFutureValue(acc.[Balance], @interestRate, 5) AS [Balance in 5 years]
	FROM [Accounts] AS [acc]
		JOIN [AccountHolders] AS [ah]
		ON acc.[AccountHolderId] = ah.[Id]
	WHERE acc.[Id] = @accountId
GO

--13

CREATE FUNCTION ufn_CashInUsersGames
(@gameName VARCHAR(50))
RETURNS TABLE
AS
	RETURN SELECT
	SUM([Cash]) AS [SumCash]
	FROM
	(
		SELECT
		[GameId],
		[Cash],
		ROW_NUMBER() OVER(ORDER BY ug.[Cash] DESC) AS [RowNumber]
		FROM [UsersGames] AS ug
		JOIN [Games] AS [g] ON ug.[GameId] = g.[Id]	
		WHERE g.[Name] = @gameName
		GROUP BY ug.[GameId], ug.[Cash]
	) AS [t]
	WHERE t.[RowNumber] % 2 = 1

SELECT * FROM dbo.ufn_CashInUsersGames('Love in a mist')
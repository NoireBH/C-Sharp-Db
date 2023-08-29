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


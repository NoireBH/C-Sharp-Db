--1
USE [Bank]
CREATE TABLE [Logs]
(
	[LogId] INT IDENTITY,
	[AccountId] INT FOREIGN KEY REFERENCES Accounts(Id),
	[OldSum] DECIMAL(18, 4),
	[NewSum] DECIMAL(18, 4)
)
GO

CREATE TRIGGER tr_AddToLogsOnAccUpdate
ON [Accounts] FOR UPDATE
AS
INSERT INTO [Logs] VALUES
(
	(SELECT [Id] FROM inserted), 
	(SELECT [Balance] FROM deleted), 
	(SELECT [Balance] FROM inserted)
)
GO

--2

CREATE TABLE [NotificationEmails]
(
	[Id] INT IDENTITY,
	[Recipient] INT,
	[Subject] VARCHAR(50),
	[Body] VARCHAR(500)
)
GO

CREATE OR ALTER TRIGGER tr_CreateNewEmailOnNewLogRecordInsert
ON [Logs] FOR INSERT
AS
INSERT INTO [NotificationEmails] VALUES
(
	(SELECT [AccountId] FROM inserted),
	(SELECT 'Balance change for account: ' 
	+ CAST([AccountId] AS VARCHAR(100))  FROM inserted),
	(SELECT 'On ' + FORMAT(GETDATE(), 'MMM dd yyyy h:mmtt')
	+ ' your balance was changed from '
	+ CAST([OldSum] AS VARCHAR(100)) 
	+ ' to' + CAST([NewSum] AS VARCHAR(100))  
	+ '.'
	FROM [inserted])
)
GO

--3

CREATE PROC usp_DepositMoney
(@AccountId INT, @MoneyAmount DECIMAL(18,4))
AS
	IF (@moneyAmount < 0) THROW 50001, 'Invalid amount', 1
	UPDATE [Accounts]
	SET [Balance] += @moneyAmount
	WHERE [Id] = @accountId
GO

--4

CREATE PROC usp_WithdrawMoney 
(@accountId INT, @moneyAmount DECIMAL(18, 4))
AS
	IF @moneyAmount < 0 THROW 50001, 'Invalid amount', 1
	UPDATE [Accounts]
	SET [Balance] -= @moneyAmount
	WHERE [Id] = @accountId	
GO

EXEC usp_WithdrawMoney 1,1000
SELECT * FROM [Accounts]
WHERE [Id] = 1
GO
--5

CREATE OR ALTER PROC usp_TransferMoney
(@senderId INT, @receiverId INT, @amount DECIMAL(18, 4))
AS
	BEGIN TRANSACTION
	BEGIN TRY
		EXEC usp_DepositMoney @receiverId, @amount
		EXEC usp_WithdrawMoney @senderId, @amount
	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION
	END CATCH
	COMMIT TRANSACTION	

--6

DECLARE @userGameId INT =
(
	SELECT ug.[Id]
	FROM [UsersGames] AS [ug] 
	JOIN [Users] AS [u] ON ug.[UserId] = u.[Id]
	JOIN [Games] AS [g] ON ug.[GameId] = g.[Id]
	WHERE u.[Username] = 'Stamat' AND g.[Name] = 'Safflower'
)

DECLARE @itemCost DECIMAL(18,4)

DECLARE @minLevel INT = 11
DECLARE @maxLevel INT = 12
DECLARE @playerCash DECIMAL(18, 4) = 
(
	SELECT [Cash]
    FROM [UsersGames]
    WHERE [Id] = @userGameId
)

SET @itemCost =
(
	SELECT
		SUM(Price)
	FROM [Items]
	WHERE [MinLevel] BETWEEN @minLevel AND @maxLevel
)

IF (@playerCash >= @itemCost)
BEGIN
	BEGIN TRANSACTION
	UPDATE [UsersGames]
	SET [Cash] -= @itemCost
	WHERE [Id] = @userGameId

	INSERT INTO [UserGameItems] (ItemId, UserGameId)
	(
		SELECT
			[Id],
			@userGameId
		FROM [Items]
		WHERE [MinLevel] BETWEEN @minLevel AND @maxLevel
	)
	COMMIT
END

SET @minLevel = 19
SET @maxLevel = 21
SET @playerCash = 
(
	SELECT [Cash]
    FROM [UsersGames]
    WHERE [Id] = @userGameId
)

SET @itemCost = 
(
	SELECT SUM([Price])
    FROM [Items]
    WHERE [MinLevel] BETWEEN @minLevel AND @maxLevel
)

IF (@playerCash >= @itemCost)
BEGIN
	BEGIN TRANSACTION
    UPDATE [UsersGames]
    SET [Cash] -= @itemCost
    WHERE [Id] = @userGameId
      
    INSERT INTO [UserGameItems] (ItemId, UserGameId)
    (
		SELECT
			[Id],
			@userGameId
		FROM [Items]
		WHERE [MinLevel] BETWEEN @minLevel AND @maxLevel
	)
	COMMIT     
END

SELECT 
	i.[Name] AS [Item Name]
FROM [Items] AS i
JOIN [UserGameItems] AS ugi ON ugi.[ItemId] = i.[Id]
JOIN [UsersGames] AS ug ON ug.[Id] = ugi.[UserGameId]
JOIN [Games] AS g ON g.[Id] = ug.[GameId]
WHERE g.[Name] = 'Safflower'
ORDER BY [Item Name]

--8

GO
CREATE OR ALTER PROC usp_AssignProject
(@emloyeeId INT, @projectID INT)
AS
BEGIN TRANSACTION
	DECLARE @projectCount INT =
	(
		SELECT
			COUNT(projectID)
		FROM [EmployeesProjects] AS ep
		WHERE ep.[EmployeeID] = @emloyeeId
	)

	IF @projectCount >= 3
	BEGIN
		RAISERROR('The employee has too many projects!', 16, 1)
		ROLLBACK		
	END

	INSERT INTO [EmployeesProjects] 
	VALUES
		(@emloyeeId, @projectID)
COMMIT TRANSACTION

EXEC usp_AssignProject 3,2

--9

CREATE TABLE Deleted_Employees
(
	[EmployeeId] INT PRIMARY KEY IDENTITY, 
	[FirstName] VARCHAR(50), 
	[LastName] VARCHAR(50), 
	[MiddleName] VARCHAR(50), 
	[JobTitle] VARCHAR(50), 
	[DepartmentId] INT, 
	[Salary] DECIMAL(18, 2)
)
GO

CREATE TRIGGER tr_AddEntityToDeletedEmployeesTable
ON [Employees] FOR DELETE
AS
INSERT INTO [Deleted_Employees]	
	SELECT		
		[FirstName],
		[LastName],
		[MiddleName],
		[JobTitle],
		[DepartmentID],
		[Salary]
	FROM deleted
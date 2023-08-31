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
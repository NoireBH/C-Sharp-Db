﻿--Database Basics MS SQL Exam – 04 Apr 2023
--1

CREATE DATABASE Accounting
GO
USE Accounting
GO





CREATE TABLE Countries
(
	Id INT PRIMARY KEY IDENTITY,
	Name VARCHAR(10) NOT NULL
)

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY,
	StreetName NVARCHAR(20) NOT NULL,
	StreetNumber INT,
	PostCode INT NOT NULL,
	City VARCHAR(25) NOT NULL,
	CountryId INT FOREIGN KEY REFERENCES Countries(Id) NOT NULL
)


CREATE TABLE Vendors
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(25) NOT NULL,
	NumberVAT NVARCHAR(15) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
)

CREATE TABLE Clients
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(25) NOT NULL,
	NumberVAT NVARCHAR(15) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL
)

CREATE TABLE Categories
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(10) NOT NULL
)

CREATE TABLE Products
(
	Id INT PRIMARY KEY IDENTITY,
	Name NVARCHAR(35) NOT NULL,
	Price DECIMAL(18,2) NOT NULL,
	CategoryId INT FOREIGN KEY REFERENCES Categories(Id) NOT NULL,
	VendorId INT FOREIGN KEY REFERENCES Vendors(Id) NOT NULL,
)

CREATE TABLE Invoices
(
	Id INT PRIMARY KEY IDENTITY,
	Number INT UNIQUE NOT NULL,
	IssueDate DATETIME2 NOT NULL,
	DueDate DATETIME2 NOT NULL,
	Amount DECIMAL(18,2) NOT NULL,
	Currency VARCHAR(5) NOT NULL,
	ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
)


CREATE TABLE ProductsClients
(
	ProductId INT NOT NULL
	, ClientId INT NOT NULL
	, CONSTRAINT PK_ProductsClients PRIMARY KEY (ProductId, ClientId)
	, CONSTRAINT FK_ProductsClients_Products  FOREIGN KEY (ProductId) REFERENCES Products(Id)
	, CONSTRAINT FK_ProductsClients_Clients  FOREIGN KEY (ClientId) REFERENCES Clients(Id)
)

--2

INSERT  INTO Products([Name], Price, CategoryId, VendorId) VALUES
('SCANIA Oil Filter XD01', 78.69 , 1, 1),
('MAN Air Filter XD01', 97.38 , 1, 5),
('DAF Light Bulb 05FG87', 55.00 , 2, 13),
('ADR Shoes 47-47.5', 49.85 , 3, 5),
('Anti-slip pads S', 5.87 , 5, 7)

INSERT  INTO Invoices(Number, IssueDate, DueDate, Amount, Currency, ClientId) VALUES
(1219992181, '2023-03-01', '2023-04-30', 180.96, 'BGN', 3),
(1729252340, '2022-11-06', '2023-01-04', 158.18, 'EUR', 13),
(1950101013, '2023-02-17', '2023-04-18', 615.15, 'USD', 19)

--3

UPDATE  Invoices
SET DueDate = '2023-04-01'
WHERE Year(IssueDate) = 2022 AND Month(IssueDate) = 11

UPDATE Clients
SET AddressId = 3
WHERE [Name] LIKE '%CO%'

--4
DELETE FROM Invoices WHERE ClientId = 11
DELETE FROM ProductsClients WHERE ClientId = 11
DELETE FROM Clients WHERE SUBSTRING(NumberVat, 1, 2) = 'IT'

--5

SELECT
	Number,
	Currency
FROM Invoices
ORDER BY Amount DESC, DueDate ASC

--6

SELECT
	p.Id,
	p.Name,
	p.Price,
	c.Name
FROM Products AS P
JOIN Categories AS c ON c.Id = p.CategoryId
WHERE c.Name = 'ADR' OR c.Name = 'Others'
ORDER BY p.Price DESC

--7

SELECT
	c.Id,
	c.Name AS Client,
	CONCAT(a.StreetName, ' ', a.StreetNumber, ', ', a.City, ', ', a.PostCode, ', ', con.[Name])
FROM Clients AS c
LEFT JOIN ProductsClients AS pc ON pc.ClientId = c.Id
JOIN Addresses AS a ON a.Id = c.AddressId
JOIN Countries AS con ON con.Id = a.CountryId
WHERE pc.ProductId IS NULL
ORDER BY c.[Name]

--8

SELECT TOP(7)
	i.Number
	, i.Amount
	, c.[Name] AS Client
FROM Invoices AS i
JOIN Clients AS c ON c.Id = i.ClientId
WHERE i.IssueDate < '2023-01-01' AND i.Currency = 'EUR'
OR i.Amount > 500 AND SUBSTRING(c.NumberVAT,1,2) = 'DE'
ORDER BY i.Number ASC, i.Amount DESC

--9

SELECT
	c.[Name] AS Client
	, MAX(p.Price) AS Price
	, c.NumberVAT AS [VAT Number]
FROM ProductsClients AS pc
JOIN Clients AS c ON pc.ClientId = c.Id
JOIN Products AS p ON pc.ProductId = p.Id
WHERE RIGHT(c.Name,2) NOT IN('KG')
GROUP BY c.[Name], c.NumberVAT
ORDER BY MAX(p.Price) DESC


--10


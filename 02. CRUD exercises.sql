--2
USE [SoftUni]


SELECT * FROM [Departments]

--3

SELECT [Name] FROM [Departments]

--4

SELECT [FirstName], [LastName], [Salary] FROM [Employees]

--5

SELECT [FirstName], [MiddleName], [LastName] FROM [Employees]

--6

SELECT [FirstName] + '.' + [LastName] + '@softuni.bg' 
AS [Full Email Address]
FROM [Employees]

--7

SELECT DISTINCT [Salary] 
FROM [Employees]

--8

SELECT *
FROM [Employees]
WHERE [JobTitle] = 'Sales Representative'

--9

SELECT [FirstName], [LastName], [JobTitle]
FROM [Employees]
WHERE [Salary] >= 20000 AND [Salary] <= 30000


--10

SELECT [FirstName] + ' ' + [MiddleName] + ' ' + [LastName]
AS [Full Name]
FROM [Employees]
WHERE [Salary] IN (25000, 14000, 12500, 23600)
--1

USE [Softuni]

SELECT 
TOP (5) e.EmployeeID,
		e.JobTitle,e.AddressID,
		a.AddressText
	FROM [Employees] 
		AS e
LEFT JOIN [Addresses] 
		AS a
		ON e.AddressID = a.AddressID
	ORDER BY e.AddressID

--2

SELECT TOP(50) 
	e.[FirstName], 
	e.[LastName],
	t.[Name],
	a.[AddressText]
FROM [Employees] AS [e]
JOIN [Addresses] AS [a]
	ON e.[AddressID] = a.[AddressID]
JOIN [Towns] AS [t]
	ON a.[TownID] = t.[TownID]
ORDER BY e.[FirstName], e.[LastName]

--3

SELECT e.[EmployeeID], e.[FirstName], e.[LastName] , d.[Name]
	FROM [Employees]
	AS e
LEFT JOIN [Departments]
	AS d
	ON e.[DepartmentID] = d.[DepartmentID]
WHERE e.[DepartmentID] = 3
ORDER BY e.[EmployeeID]

--4

SELECT 
	TOP(5) 
		e.[EmployeeID],
		e.[FirstName],
		e.[Salary],
		d.[Name] AS [DepartmentName]
	FROM [Employees]
	AS e
LEFT JOIN [Departments]
	AS d
	ON e.[DepartmentID] = d.[DepartmentID]
WHERE e.[Salary] > 15000
ORDER BY e.[DepartmentID] ASC

--5

SELECT 
	TOP(3) 
		e.[EmployeeID],
		e.[FirstName]	
FROM [EmployeesProjects]
	AS ep
RIGHT JOIN [Employees]
	AS e
	ON ep.[EmployeeID] = e.[EmployeeID]
WHERE ep.[ProjectID] IS NULL
ORDER BY e.[EmployeeID]

--6

SELECT 
	e.[FirstName],
	e.[LastName],
	e.[HireDate],
	d.[Name]
FROM [Employees]
	AS [e]
LEFT JOIN [Departments]
	AS [d]
	ON e.DepartmentID = d.DepartmentID
WHERE 
	e.DepartmentID IN (3,10) 
	AND e.[HireDate] > '1999-01-01'
ORDER BY e.[HireDate]

--7

SELECT 
	TOP (5)
		e.[EmployeeID],
		e.[FirstName],
		p.[Name] AS [ProjectName]
FROM [EmployeesProjects]
	AS [ep]
RIGHT JOIN [Employees]
	AS [e]
	ON ep.[EmployeeID] = e.[EmployeeID]
INNER JOIN [Projects]
	AS [p]
	ON ep.[ProjectID] = p.[ProjectID]
WHERE ep.[ProjectID] 
	IS NOT NULL
	AND p.StartDate > '2002-08-13'
	AND p.EndDate IS NULL
ORDER BY e.[EmployeeID]

-- ===============================================================================
-- Alachisoft (R) NCache Sample Code.
-- ===============================================================================
-- Copyright © Alachisoft.  All rights reserved.
-- THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
-- OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
-- LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
-- FITNESS FOR A PARTICULAR PURPOSE.
-- ===============================================================================

-- ============================================
-- Drop tables if they exist (correct dependency order)
-- ============================================
IF OBJECT_ID('dbo.Orders', 'U') IS NOT NULL DROP TABLE dbo.Orders;
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Customers', 'U') IS NOT NULL DROP TABLE dbo.Customers;
IF OBJECT_ID('dbo.Suppliers', 'U') IS NOT NULL DROP TABLE dbo.Suppliers;

-- ============================================
-- Create Customers table
-- ============================================
CREATE TABLE dbo.Customers (
    CustomerID CHAR(6) PRIMARY KEY,
    CompanyName NVARCHAR(40) NOT NULL,
    ContactName NVARCHAR(30),
    ContactTitle NVARCHAR(30),
    Address NVARCHAR(60),
    City NVARCHAR(15),
    Region NVARCHAR(15),
    PostalCode NVARCHAR(10),
    Country NVARCHAR(15),
    Phone NVARCHAR(24),
    Fax NVARCHAR(24),
    LastModify DATETIME DEFAULT GETDATE()
);

-- ============================================
-- Create Suppliers table
-- ============================================
CREATE TABLE dbo.Suppliers (
    SupplierID BIGINT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(40) NOT NULL,
    ContactName NVARCHAR(30),
    ContactTitle NVARCHAR(30),
    Address NVARCHAR(60),
    City NVARCHAR(15),
    Region NVARCHAR(15),
    PostalCode NVARCHAR(10),
    Country NVARCHAR(15),
    Phone NVARCHAR(24),
    Fax NVARCHAR(24),
    HomePage NTEXT,
    LastModify DATETIME DEFAULT GETDATE()
);

-- ============================================
-- Create Products table
-- ============================================
CREATE TABLE dbo.Products (
    ProductID BIGINT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(40) NOT NULL,
    SupplierID BIGINT NOT NULL,
    CategoryID BIGINT NULL,
    QuantityPerUnit NVARCHAR(20),
    UnitPrice MONEY DEFAULT 0,
    UnitsInStock SMALLINT DEFAULT 0,
    UnitsOnOrder SMALLINT DEFAULT 0,
    ReorderLevel SMALLINT DEFAULT 0,
    Discontinued BIT NOT NULL DEFAULT 0,
    LastModify DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Products_Suppliers FOREIGN KEY (SupplierID) REFERENCES Suppliers(SupplierID)
);

-- ============================================
-- Create Orders table
-- ============================================
CREATE TABLE dbo.Orders (
	OrderID BIGINT IDENTITY(1,1) PRIMARY KEY,
	CustomerID CHAR(6) NOT NULL,
	EmployeeID BIGINT NULL,
	OrderDate DATETIME,
	RequiredDate DATETIME NULL,
	ShippedDate DATETIME NULL,
	ShipVia INT NULL,
	Freight MONEY DEFAULT 0,
	ShipName NVARCHAR(40),
	ShipAddress NVARCHAR(60) NULL,
	ShipCity NVARCHAR(15) NULL,
	ShipRegion NVARCHAR(15) NULL,
	ShipPostalCode NVARCHAR(10) NULL,
	ShipCountry NVARCHAR(15) NULL,
	LastModify DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);

-- ============================================
-- Populate Customers
-- ============================================
IF NOT EXISTS (SELECT 1 FROM dbo.Customers)
BEGIN
    INSERT INTO dbo.Customers (CustomerID, CompanyName, ContactName, ContactTitle, City, Country)
    VALUES
	('ALFKI','Company A','Alice','Manager','New York','USA'),
	('ANATR','Company B','Bob','Director','London','UK'),
	('ANTON','Company C','Charlie','CEO','Paris','France'),
	('AROUT','Company D','David','CFO','Berlin','Germany'),
	('BERGS','Company E','Eve','CTO','Rome','Italy'),
	('BLAUS','Company F','Frank','Manager','Madrid','Spain'),
	('BLONP','Company G','Grace','Director','Lisbon','Portugal'),
	('BOLID','Company H','Hank','CEO','Vienna','Austria'),
	('BONAP','Company I','Ivy','CFO','Prague','Czech'),
	('BOTTM','Company J','Jack','CTO','Budapest','Hungary'),
	('BSBEV','Company K','Kate','Manager','Warsaw','Poland'),
	('CACTU','Company L','Leo','Director','Athens','Greece'),
	('CENTC','Company M','Mia','CEO','Dublin','Ireland'),
	('CHOPS','Company N','Nick','CFO','Oslo','Norway'),
	('COMMI','Company O','Olivia','CTO','Stockholm','Sweden'),
	('CONSH','Company P','Paul','Manager','Helsinki','Finland'),
	('DRACD','Company Q','Quinn','Director','Copenhagen','Denmark'),
	('DUMON','Company R','Rita','CEO','Brussels','Belgium'),
	('EASTC','Company S','Sam','CFO','Amsterdam','Netherlands'),
	('ERNSH','Company T','Tina','CTO','Zurich','Switzerland');

END

-- ============================================
-- Populate Suppliers
-- ============================================
IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers)
BEGIN
    INSERT INTO dbo.Suppliers (CompanyName, ContactName, City, Country, Phone)
    VALUES
    ('Supplier A','Alice','New York','USA','+1-212-555-0101'),
    ('Supplier B','Bob','London','UK','+44-20-5555-0102'),
    ('Supplier C','Charlie','Paris','France','+33-1-5555-0103'),
    ('Supplier D','David','Berlin','Germany','+49-30-5555-0104'),
    ('Supplier E','Eve','Rome','Italy','+39-06-5555-0105'),
    ('Supplier F','Frank','Madrid','Spain','+34-91-555-0106'),
    ('Supplier G','Grace','Lisbon','Portugal','+351-21-555-0107'),
    ('Supplier H','Hank','Vienna','Austria','+43-1-5555-0108'),
    ('Supplier I','Ivy','Prague','Czech','+420-2-5555-0109'),
    ('Supplier J','Jack','Budapest','Hungary','+36-1-5555-0110'),
    ('Supplier K','Kate','Warsaw','Poland','+48-22-5555-0111'),
    ('Supplier L','Leo','Athens','Greece','+30-21-5555-0112'),
    ('Supplier M','Mia','Dublin','Ireland','+353-1-5555-0113'),
    ('Supplier N','Nick','Oslo','Norway','+47-21-5555-0114'),
    ('Supplier O','Olivia','Stockholm','Sweden','+46-8-5555-0115'),
    ('Supplier P','Paul','Helsinki','Finland','+358-9-5555-0116'),
    ('Supplier Q','Quinn','Copenhagen','Denmark','+45-33-5555-0117'),
    ('Supplier R','Rita','Brussels','Belgium','+32-2-5555-0118'),
    ('Supplier S','Sam','Amsterdam','Netherlands','+31-20-5555-0119'),
    ('Supplier T','Tina','Zurich','Switzerland','+41-44-5555-0120');
END


-- ============================================
-- Populate Products
-- ============================================
IF NOT EXISTS (SELECT 1 FROM dbo.Products)
BEGIN
    INSERT INTO dbo.Products (ProductName, SupplierID, UnitPrice, Discontinued, QuantityPerUnit)
    VALUES
    ('Product 1', 1, 10.0, 1, 5),
	('Product 2', 2, 20.0, 1, 15),
	('Product 3', 3, 30.0, 1, 2),
	('Product 4', 4, 40.0, 1, 12),
	('Product 5', 5, 50.0, 1, 45),
	('Product 6', 6, 60.0, 1, 5),
	('Product 7', 7, 70.0, 1, 6),
	('Product 8', 8, 80.0, 0, 7),
	('Product 9', 9, 90.0, 1, 2),
	('Product 10', 10, 100.0, 1, 1),
	('Product 11', 11, 110.0, 0, 7),
	('Product 12', 12, 120.0, 1, 23),
	('Product 13', 13, 130.0, 0, 52),
	('Product 14', 14, 140.0, 1, 21),
	('Product 15', 15, 150.0, 1, 42),
	('Product 16', 16, 160.0, 1, 64),
	('Product 17', 17, 170.0, 1, 16),
	('Product 18', 18, 180.0, 1, 41),
	('Product 19', 19, 190.0, 1, 21),
	('Product 20', 20, 200.0, 1, 23);
END

-- ============================================
-- Populate Orders
-- ============================================
IF NOT EXISTS (SELECT 1 FROM dbo.Orders)
BEGIN
    INSERT INTO dbo.Orders (CustomerID, EmployeeID, OrderDate, Freight, ShipName)
    VALUES
    ('ALFKI', 1, GETDATE(), 10.0, 'Ship A'),
    ('ANATR', 2, GETDATE(), 20.0, 'Ship B'),
    ('ANTON', 3, GETDATE(), 30.0, 'Ship C'),
    ('AROUT', 4, GETDATE(), 40.0, 'Ship D'),
    ('BERGS', 5, GETDATE(), 50.0, 'Ship E'),
    ('BLAUS', 6, GETDATE(), 60.0, 'Ship F'),
    ('BLONP', 7, GETDATE(), 70.0, 'Ship G'),
    ('BOLID', 8, GETDATE(), 80.0, 'Ship H'),
    ('BONAP', 9, GETDATE(), 90.0, 'Ship I'),
    ('BOTTM', 10, GETDATE(), 100.0, 'Ship J'),
    ('BSBEV', 11, GETDATE(), 110.0, 'Ship K'),
    ('CACTU', 12, GETDATE(), 120.0, 'Ship L'),
    ('CENTC', 13, GETDATE(), 130.0, 'Ship M'),
    ('CHOPS', 14, GETDATE(), 140.0, 'Ship N'),
    ('COMMI', 15, GETDATE(), 150.0, 'Ship O'),
    ('CONSH', 16, GETDATE(), 160.0, 'Ship P'),
    ('DRACD', 17, GETDATE(), 170.0, 'Ship Q'),
    ('DUMON', 18, GETDATE(), 180.0, 'Ship R'),
    ('EASTC', 19, GETDATE(), 190.0, 'Ship S'),
    ('ERNSH', 20, GETDATE(), 200.0, 'Ship T');
END

PRINT 'Database setup complete with sample data.';

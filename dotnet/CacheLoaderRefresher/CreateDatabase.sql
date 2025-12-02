-- ===============================================================================
-- Alachisoft (R) NCache Sample Code.
-- ===============================================================================
-- Copyright © Alachisoft.  All rights reserved.
-- THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
-- OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
-- LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
-- FITNESS FOR A PARTICULAR PURPOSE.
-- ===============================================================================

-- Drop existing tables if they exist
IF OBJECT_ID('dbo.Products', 'U') IS NOT NULL DROP TABLE dbo.Products;
IF OBJECT_ID('dbo.Suppliers', 'U') IS NOT NULL DROP TABLE dbo.Suppliers;
GO

-- =============================================
-- 1. Create Suppliers Table
-- =============================================
CREATE TABLE dbo.Suppliers
(
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    CompanyName NVARCHAR(100) NOT NULL,
    ContactName NVARCHAR(100),
    ContactTitle NVARCHAR(100),
    Address NVARCHAR(200),
    City NVARCHAR(50),
    Country NVARCHAR(50),
    Phone NVARCHAR(30),
    LastModify DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- =============================================
-- 2. Create Products Table
-- =============================================
CREATE TABLE dbo.Products
(
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    ProductName NVARCHAR(100) NOT NULL,
    SupplierID INT FOREIGN KEY REFERENCES dbo.Suppliers(SupplierID),
    CategoryID INT NULL,
    QuantityPerUnit NVARCHAR(50),
    UnitPrice MONEY,
    UnitsInStock SMALLINT,
    UnitsOnOrder SMALLINT,
    ReorderLevel SMALLINT,
    Discontinued BIT DEFAULT 0,
    LastModify DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- =============================================
-- 3. Populate Suppliers with sample data
-- =============================================
INSERT INTO dbo.Suppliers (CompanyName, ContactName, ContactTitle, Address, City, Country, Phone)
VALUES
('Exotic Liquids', 'Charlotte Cooper', 'Purchasing Manager', '49 Gilbert St.', 'London', 'UK', '(171) 555-2222'),
('New Orleans Cajun Delights', 'Shelley Burke', 'Order Administrator', 'P.O. Box 78934', 'New Orleans', 'USA', '(100) 555-4822'),
('Grandma Kelly''s Homestead', 'Regina Murphy', 'Sales Representative', '707 Oxford Rd.', 'Ann Arbor', 'USA', '(313) 555-5735'),
('Tokyo Traders', 'Yoshi Nagase', 'Marketing Manager', '9-8 Sekimai Musashino-shi', 'Tokyo', 'Japan', '(03) 3555-5011'),
('Cooperativa de Quesos', 'Antonio del Valle Saavedra', 'Export Administrator', 'Calle del Rosal 4', 'Oviedo', 'Spain', '(98) 598 76 54'),
('Mayumi''s', 'Mayumi Ohno', 'Marketing Representative', '92 Setsuko Chuo-ku', 'Osaka', 'Japan', '(06) 431-7877'),
('Pavlova Ltd.', 'Ian Devling', 'Marketing Manager', '74 Rose St. Moonie Ponds', 'Melbourne', 'Australia', '(03) 444-2343'),
('Specialty Biscuits', 'Peter Wilson', 'Sales Representative', '29 King''s Way', 'Manchester', 'UK', '(161) 555-4448'),
('PB Knäckebröd AB', 'Lars Peterson', 'Sales Agent', 'Kaloadagatan 13', 'Göteborg', 'Sweden', '(031) 345-6789'),
('Refrescos Americanas LTDA', 'Carlos Diaz', 'Marketing Manager', 'Av. das Americanas 12.890', 'São Paulo', 'Brazil', '(11) 555-4640');
GO

-- =============================================
-- 4. Populate Products with sample data
-- =============================================
INSERT INTO dbo.Products (ProductName, SupplierID, CategoryID, QuantityPerUnit, UnitPrice, UnitsInStock, UnitsOnOrder, ReorderLevel, Discontinued)
VALUES
('Chai', 1, 1, '10 boxes x 20 bags', 18.00, 39, 0, 10, 0),
('Chang', 1, 1, '24 - 12 oz bottles', 19.00, 17, 40, 25, 0),
('Aniseed Syrup', 1, 2, '12 - 550 ml bottles', 10.00, 13, 70, 25, 0),
('Chef Anton''s Cajun Seasoning', 2, 2, '48 - 6 oz jars', 22.00, 53, 0, 0, 0),
('Chef Anton''s Gumbo Mix', 2, 2, '36 boxes', 21.35, 0, 0, 0, 1),
('Grandma''s Boysenberry Spread', 3, 2, '12 - 8 oz jars', 25.00, 120, 0, 25, 0),
('Uncle Bob''s Organic Dried Pears', 3, 7, '12 - 1 lb pkgs.', 30.00, 15, 0, 10, 0),
('Northwoods Cranberry Sauce', 3, 2, '12 - 12 oz jars', 40.00, 6, 0, 0, 0),
('Mishi Kobe Niku', 4, 6, '18 - 500 g pkgs.', 97.00, 29, 0, 0, 1),
('Ikura', 4, 8, '12 - 200 ml jars', 31.00, 31, 0, 0, 0),
('Queso Cabrales', 5, 4, '1 kg pkg.', 21.00, 22, 30, 30, 0),
('Queso Manchego La Pastora', 5, 4, '10 - 500 g pkgs.', 38.00, 86, 0, 0, 0),
('Tofu', 6, 7, '40 - 100 g pkgs.', 23.25, 35, 0, 0, 0),
('Genen Shouyu', 6, 2, '24 - 250 ml bottles', 15.50, 39, 0, 5, 0),
('Pavlova', 7, 3, '32 - 500 g boxes', 17.45, 29, 0, 10, 0),
('Outback Lager', 7, 1, '24 - 355 ml bottles', 15.00, 15, 10, 30, 0),
('Flotemysost', 8, 4, '10 - 500 g pkgs.', 21.50, 26, 0, 0, 0),
('Mozzarella di Giovanni', 8, 4, '24 - 200 g pkgs.', 34.80, 14, 0, 0, 0),
('Gnocchi di nonna Alice', 9, 5, '24 - 250 g pkgs.', 38.00, 21, 10, 30, 0),
('Ravioli Angelo', 9, 5, '24 - 250 g pkgs.', 19.50, 36, 0, 0, 0),
('Guaraná Fantástica', 10, 1, '12 - 355 ml cans', 4.50, 20, 0, 0, 1);
GO

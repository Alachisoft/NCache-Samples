-- ===============================================================================
-- Alachisoft (R) NCache Sample Code.
-- ===============================================================================
-- Copyright © Alachisoft.  All rights reserved.
-- THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
-- OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
-- LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
-- FITNESS FOR A PARTICULAR PURPOSE.
-- ===============================================================================

UPDATE dbo.Products
SET 
    UnitPrice = UnitPrice + 10,
	UnitsInStock = UnitsInStock + 1,
    LastModify = GETDATE();
GO

UPDATE dbo.Suppliers
SET 
	ContactName = ContactName + ' [New]',
    LastModify = GETDATE();
GO

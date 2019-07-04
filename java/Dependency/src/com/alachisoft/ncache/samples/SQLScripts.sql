-- ===============================================================================
-- Alachisoft (R) NCache Sample Code.
-- NCache Oracle Dependency Script
-- ===============================================================================
-- Copyright © Alachisoft.  All rights reserved.
-- THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
-- OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
-- LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
-- FITNESS FOR A PARTICULAR PURPOSE.
-- ===============================================================================

--Create table'TayzGrid_db_sync'
CREATE TABLE TAYZGRID_DB_SYNC (
CACHE_KEY VARCHAR2 ( 256 ) NOT NULL ENABLE ,
CACHE_ID VARCHAR2 ( 256 ) NOT NULL ENABLE ,
MODIFIED NUMBER ( 2 , 1 ) DEFAULT 0 NOT NULL ENABLE ,
WORK_IN_PROGRESS NUMBER ( 2 , 1 ) DEFAULT 0 NOT NULL ENABLE ,
PRIMARY KEY ( CACHE_KEY , CACHE_ID ) ENABLE
);

--create trigger
Create or replace trigger MYTRIGGER
AFTER
update or delete on PRODUCTS
REFERENCING OLD AS oldRow
for each row
BEGIN
UPDATE TAYZGRID_DB_SYNC
SET modified = 1
WHERE cache_key = (: oldRow . ProductID || ':dbo.Products' );
END TRIGGER ;

--Update Table for DBDepenedency Sample
--This query is supposed to run when execution is on hold whil Thread.Sleep in DBDependency.java.
UPDATE Products SET ProductName = 'Modified Product' WHERE ProductID = 6


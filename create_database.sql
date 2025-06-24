-- Script to create khachsan database
USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'khachsan')
BEGIN
    DROP DATABASE khachsan;
END
GO

-- Create database
CREATE DATABASE khachsan;
GO

-- Use the new database
USE khachsan;
GO

PRINT 'Database khachsan created successfully!';

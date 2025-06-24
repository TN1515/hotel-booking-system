-- Fix NormalizedName for roles in AspNetRoles table
UPDATE AspNetRoles 
SET NormalizedName = 'ADMIN' 
WHERE Name = 'Admin';

UPDATE AspNetRoles 
SET NormalizedName = 'CUSTOMER' 
WHERE Name = 'Customer';

UPDATE AspNetRoles 
SET NormalizedName = 'STAFF' 
WHERE Name = 'Staff';

-- Insert roles if they don't exist
IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
    INSERT INTO AspNetRoles (Name, NormalizedName, RoleID, RoleName, Description, IsActive, CreatedBy, CreatedDate)
    VALUES ('Admin', 'ADMIN', 1, 'Admin', 'Administrator', 1, 'System', GETDATE());
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Customer')
BEGIN
    INSERT INTO AspNetRoles (Name, NormalizedName, RoleID, RoleName, Description, IsActive, CreatedBy, CreatedDate)
    VALUES ('Customer', 'CUSTOMER', 2, 'Customer', 'Customer', 1, 'System', GETDATE());
END

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Staff')
BEGIN
    INSERT INTO AspNetRoles (Name, NormalizedName, RoleID, RoleName, Description, IsActive, CreatedBy, CreatedDate)
    VALUES ('Staff', 'STAFF', 3, 'Staff', 'Hotel Staff', 1, 'System', GETDATE());
END

-- Verify the roles
SELECT Id, Name, NormalizedName, RoleID, RoleName, Description, IsActive 
FROM AspNetRoles 
ORDER BY RoleID;

CREATE TABLE [dbo].[Felhasznalo]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY,
	[PrivilegeLevel] INT DEFAULT 0,
	[Username] NVARCHAR(50) NOT NULL UNIQUE,
	[Password] NVARCHAR(50) NOT NULL,
	[Name] NVARCHAR(100),
	[Email] NVARCHAR(100),
	[PhoneNumber] NVARCHAR(12),
	[DeliveryAddress] NVARCHAR(200)
);
GO
INSERT INTO Felhasznalo (PrivilegeLevel,Username,Password) VALUES(1,'admin','admin');
SELECT * FROM Felhasznalo;
DROP TABLE Felhasznalo;
DELETE FROM Felhasznalo;

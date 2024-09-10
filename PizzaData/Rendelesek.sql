CREATE TABLE [dbo].[Rendelesek]
(
	[OrderId] INT NOT NULL IDENTITY PRIMARY KEY,
    [Nev] NVARCHAR(100) NOT NULL,
    [Cim] NVARCHAR(100) NOT NULL,
    [Email] NVARCHAR(100) NOT NULL,
    [Telefonszam] NVARCHAR(12) NOT NULL,
    [PizzaID] INT NOT NULL,
    [PizzaDB] INT NOT NULL,
    [Reszosszeg] INT NOT NULL,
    [Allapot] NVARCHAR(50) NOT NULL,
    [OrderDate] DATE DEFAULT GETDATE()
);
SELECT * FROM Rendelesek;
DELETE FROM Rendelesek;
DROP TABLE [dbo].[Rendelesek];

INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Csiga János', 'Falu utca 11', 'csiga.jani@gmail.com', '+36303345155', 1, 2, 1000, 'Felvett', '2024-04-17');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Rakodó Béla', 'Város tér 5', 'rakodos123@ghotmail.com', '+36303325195', 1, 2, 1000, 'Meghiúsult', '2024-04-16');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Cipő Adél', 'Vidék utca 13', 'destroyer69@freemail.com', '+36404932588', 1, 2, 1000, 'Kiszállítva', '2024-04-18');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Faló Péter', 'Majlék út 45/B', 'falo.peti@gmail.com', '+36202346617', 1, 2, 1000, 'Felvett', '2024-04-21');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Faló Péter', 'Majlék út 45/B', 'falo.peti@gmail.com', '+36202346617', 1, 2, 1000, 'Felvett', '2024-04-24');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Faló Péter', 'Majlék út 45/B', 'falo.peti@gmail.com', '+36202346617', 1, 2, 1000, 'Felvett', '2024-04-24');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Faló Péter', 'Majlék út 45/B', 'falo.peti@gmail.com', '+36202346617', 1, 2, 1000, 'Felvett', '2024-04-24');
INSERT INTO Rendelesek (Nev, Cim, Email, Telefonszam, PizzaID, PizzaDB, Reszosszeg, Allapot, OrderDate) 
VALUES ('Faló Péter', 'Majlék út 45/B', 'falo.peti@gmail.com', '+36202346617', 1, 2, 1000, 'Felvett', '2024-04-24');
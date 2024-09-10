CREATE TABLE [dbo].[Pizza]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Neve] NCHAR(50) NOT NULL, 
    [Ar] INT NOT NULL
);
GO
INSERT INTO Pizza (Id,Neve,Ar) VALUES (1,'Margarinás pizza',1800);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (2,'Sonku pizza',2100);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (3,'Songo pizza',2200);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (4,'Songoku pizza',2400);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (5,'Paraszt pizza',2600);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (6,'Ultra Paraszt pizza',2900);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (7,'Tenger Gyümi pizza',3000);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (8,'Husi pizza',2900);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (9,'Tonyás pizza',2500);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (10,'Vega pizza',2300);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (11,'Négysajtos pizza',2400);
INSERT INTO Pizza (Id,Neve,Ar) VALUES (12,'Hawaii pizza',6000);
DELETE FROM Pizza;
SELECT * FROM Pizza;
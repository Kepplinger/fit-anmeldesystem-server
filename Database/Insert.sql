Insert into Addresses (Addition, City, StreetNumber, ZipCode, Street) 
VALUES ('Adresszusatz', 'Leonding', 12, 4050, 'Limesstraße')

INSERT INTO Contacts (FirstName,LastName, Email, PhoneNumber)
VALUES ('ExampleFirstName', 'ExampleLastName', 'andi.sakal15@gmail.com', '+4369917209297')

INSERT INTO Companies (FK_Address, FK_Contact, Name, Homepage, LogoUrl, PhoneNumber, Email, EstablishmentsAut, EstablishmentsCountAut) 
VALUES (1,1,'HTL Leonding', 'www.htl-leonding.at', 'www.htl-leonding.at', '+4369917202927', 'andi.sakal15@gmail.com', 'Leonding', 1)

INSERT INTO Packages (Name, Discriminator, Price, Description) 
VALUES ('Grundpaket', 1, 200, 'Das Grundpaket bietet Ihnen einen Standplatz am FIT')

INSERT INTO Packages (Name, Discriminator, Price, Description) 
VALUES ('Sponsorpaket', 2, 400, 'Beim Sponsorpaket zusätzlich enthalten ist noch anbringung Ihres Firmenlogos auf Werbematerialien des FITs')

INSERT INTO Packages (Name, Discriminator, Price, Description) 
VALUES ('Vortragspaket', 3, 600, 'Beim Vortragspaket zuästzlich zu den restlichen Paketen darf man einen Vortrag halten')

INSERT INTO Resources (Name, [Description])
VALUES ('Wlan','Es wird Ihnen ein Wlan Modul zur Verfügung gestellt')

INSERT INTO Resources (Name, [Description])
VALUES ('Stehtisch','Ein Stehtisch')

INSERT INTO Resources (Name, [Description])
VALUES ('Sessel','Ein Sessel')

INSERT INTO Resources (Name, [Description])
VALUES ('Strom','Strom')

INSERT INTO Areas (Designation, GraphicURL) 
VALUES ('Erdgeschoss', 'www.erdgeschossurl.com')

INSERT INTO Areas (Designation, GraphicURL) 
VALUES ('1 Stock', 'www.stock1.com')

INSERT INTO Areas (Designation, GraphicURL) 
VALUES ('2 Stkock', 'www.stock2.com')

INSERT INTO Events (EventDate, RegistrationStart, RegistrationEnd, IsLocked, FK_Areas)
VALUES ('2017-11-30 00:00:00', '2017-11-01 00:00:00','2018-11-30 00:00:00', 1, 1);

INSERT INTO Branches (Name)
VALUES ('Informatik/Medientechnik');

INSERT INTO Branches (Name)
VALUES ('Elektronik/techn. Informatik');

INSERT INTO Branches (Name)
VALUES ('Biomedizin & Gesundheitstechnik');

INSERT INTO Locations (FK_Area, Category, Number, XCoordinate, YCoordinate)
VALUES (1, 'Iagenda Category', 100, 50, 50)

INSERT Into Presentations ([Description],FileURL, FK_Branch, IsAccepted, RoomNumber, Title)
VALUES ('Presentationsbeschreibung', 'www.fileurl.com', 1, 0, '252', 'Presentations Title')

INSERT Into Bookings (FK_Branches, FK_Company, FK_Event, FK_Location, FK_FitPackage, FK_Presentation, AdditionalInfo, CreationDate, isAccepted, ProvidesSummerJob, ProvidesThesis, Remarks)
VALUES (1,1,1,1,1,1,'additional info', '2017-11-05 00:00:00', 0, 1, 1,'Bemerkungen')

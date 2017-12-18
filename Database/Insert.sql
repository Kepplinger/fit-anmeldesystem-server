Insert into Addresses (Addition, City, StreetNumber, ZipCode, Street) 
VALUES ('Something Additional', 'Linz', 11, 4020, 'Thomas-Bernhard Weg')

INSERT INTO Contacts (FirstName,LastName, Email, PhoneNumber)
VALUES ('Andrej', 'Sakal', 'andi.sakal15@gmail.com', '+4369917209297')

INSERT INTO Contacts (FirstName,LastName, Email, PhoneNumber)
VALUES ('Felix', 'Hofmann', 'andi.sakal15@gmail.com', '+4369917209297')

INSERT INTO Companies (FK_Address, FK_Contact, Name, Homepage, LogoUrl, PhoneNumber, Email, EstablishmentsAut, EstablishmentsCountAut) 
VALUES (1,1,'Sakal IT', 'www.sakal-it.at', 'www.sakal-it.at/logo', '+4369917202927', 'andi.sakal15@gmail.com', 'Linz', 1)

INSERT INTO Packages (Name, Discriminator, Price) 
VALUES ('Grundpaket', 1, 200)

INSERT INTO Packages (Name, Discriminator, Price) 
VALUES ('Sponsorpaket', 2, 400)

INSERT INTO Packages (Name, Discriminator, Price) 
VALUES ('Vortragspaket', 3, 600)

INSERT INTO Resources (Name, [Description])
VALUES ('Wlan','Es wird Ihnen ein Wlan Modul zur Verfügung gestellt')

INSERT INTO Areas (Designation, GraphicURL) 
VALUES ('Erdgeschoss', 'www.linktourlofstandplatzwahl')

INSERT INTO Areas (Designation, GraphicURL) 
VALUES ('1 Stk.', 'www.linktourlofstandplatzwahl')

INSERT INTO Areas (Designation, GraphicURL) 
VALUES ('2 Stk.', 'www.linktourlofstandplatzwahl')

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

INSERT Into Bookings (FK_Branches, FK_Company, FK_Event, FK_Location, FK_Package, FK_Presentation, AdditionalInfo, CompanyDescription, CreationDate, isAccepted, ProvidesSummerJob, ProvidesThesis, Remarks)
VALUES (1,1,1,1,1,1,'Additional Infos need sth','Sakal Company', '2017-11-05 00:00:00', 0, 1, 1,'bemerkungen')

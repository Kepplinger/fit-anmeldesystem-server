Insert into Addresses (AddressAdditional, City, StreetNumber, PostalCode, Street) 
VALUES ('Something Additional', 'Linz', 11, 4020, 'Thomas-Bernhard Weg')

INSERT INTO Contacts (FirstName,LastName, Email, Phone)
VALUES ('Andrej', 'Sakal', 'andi.sakal15@gmail.com', '+4369917209297')

INSERT INTO Contacts (FirstName,LastName, Email, Phone)
VALUES ('Felix', 'Hofmann', 'andi.sakal15@gmail.com', '+4369917209297')

INSERT INTO Companies (FK_Address, FK_Contact, Name, Homepage, LogoUrl, PhoneNumber, Email, SubjectAreas)
VALUES (1,1,'Sakal IT', 'www.sakal-it.at', 'www.sakal-it.at/logo', '+4369917202927', 'andi.sakal15@gmail.com', 'Informatik')

INSERT INTO Packages (Name, Number, Tag) 
VALUES ('Premium Package', 1, 'PP')

INSERT INTO Events (EventDate, RegistrationStart, RegistrationEnd, IsLocked)
VALUES ('2017-11-30 00:00:00', '2017-11-01 00:00:00','2018-11-30 00:00:00', 1);

INSERT INTO Branches (Name)
VALUES ('IT');

INSERT INTO Areas (Designation, GraphicURL, FK_Event) 
VALUES ('Erdgeschoss', 'www.linktourlofstandplatzwahl', 1)

INSERT INTO Locations (FK_Area, Category, Number, XCoordinate, YCoordinate)
VALUES (1, 'Iagenda Category', 100, 50, 50)


INSERT Into Presentations ([Description],FileURL, FK_Branch, IsAccepted, RoomNumber, Title)
VALUES ('Presentationsbeschreibung', 'www.fileurl.com', 1, 0, '252', 'Presentations Title')

INSERT Into Bookings (FK_Branches, FK_Company, FK_Event, FK_Location, FK_Package, FK_Presentation, AdditionalInfo, CompanyDescription, CreationDate, isAccepted, ProvidesSummerJob, ProvidesThesis, Remarks)
VALUES (1,1,1,1,1,1,'Additional Infos need sth','Sakal Company', '2017-11-05 00:00:00', 0, 1, 1,'bemerkungen')

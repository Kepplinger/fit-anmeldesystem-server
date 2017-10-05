insert into dbo.Events(Date,RegistrationStart,RegistrationEnd,IsLocked) values(27-11-2015,1-11-2015,20-11-2015,'true');
insert into dbo.Events(Date,RegistrationStart,RegistrationEnd,IsLocked) values(27-11-2016,1-10-2016,20-11-2016,'true');
insert into dbo.Events(Date,RegistrationStart,RegistrationEnd,IsLocked) values(27-11-2017,1-09-2017,20-11-2017,'true');
insert into dbo.Events(Date,RegistrationStart,RegistrationEnd,IsLocked) values(27-11-2018,1-08-2018,20-11-2018,'true');

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select 'Erdgeschoss',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2015;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select '1.Stock',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2015;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select '2.Stock',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2015;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select 'Turnsaal',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2015;

	insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select 'Erdgeschoss',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2016;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select '1.Stock',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2016;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select '2.Stock',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2016;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select 'Turnsaal',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2016;

	insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select 'Erdgeschoss',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2017;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select '1.Stock',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2017;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select '2.Stock',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2017;

insert into dbo.Areas(Designation,Graphic,FK_Event) 
	select 'Turnsaal',2132130210301203,e.id 
	from dbo.Events e 
	where e.Date = 27-11-2017;

insert into dbo.Locations(Number,FK_Area,XCoordinate,YCoordinate)
	select 1,a.id,200,200
	from dbo.Areas a, dbo.Events e
	where a.Designation = '1.Stock' and e.Date=27-11-2017;

insert into dbo.Locations(Number,FK_Area,XCoordinate,YCoordinate)
	select 10,a.id,250,250
	from dbo.Areas a, dbo.Events e
	where a.Designation = 'Erdgeschoss' and e.Date=27-11-2016;

insert into dbo.Locations(Number,FK_Area,XCoordinate,YCoordinate)
	select 15,a.id,100,50
	from dbo.Areas a, dbo.Events e
	where a.Designation = '2.Stock' and e.Date=27-11-2015;

insert into dbo.Locations(Number,FK_Area,XCoordinate,YCoordinate)
	select 101,a.id,400,400
	from dbo.Areas a, dbo.Events e
	where a.Designation = '2.Stock' and e.Date=27-11-2017;

insert into dbo.Categories(name,price,Description,Fk_Location)
select 'basic',200,'Nur einen Stand', l.Id
from dbo.Locations l
where l.Number = 101;

insert into dbo.Categories(name,price,Description,Fk_Location) 
select 'sponsor',400,'Stand + Vortrag', l.id
from dbo.Locations l
where l.Number = 10;

insert into dbo.Categories(name,price,Description,Fk_Location) 
select 'premium sponsor',600,'Stand + Vortrag + Sponsoreitrag',l.id
from dbo.Locations l
where l.Number = 15;

insert into dbo.Presentations(RoomNumber,Title,Description,IsAccepted) values(101,'C#','Was ist C?#','false');
insert into dbo.Presentations(RoomNumber,Title,Description,IsAccepted) values(102,'Java','Was ist Java?','false');
insert into dbo.Presentations(RoomNumber,Title,Description,IsAccepted) values(103,'C','Was ist C?','false');
insert into dbo.Presentations(RoomNumber,Title,Description,IsAccepted) values(104,'Python','Was ist Python?','false');
insert into dbo.Presentations(RoomNumber,Title,Description,IsAccepted) values(105,'Sql','Was ist Sql?','false');

insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Patrick','Mistlberger','002349919239',200012030492349,'patrick.m@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Felix','Hofmann','0012323222',92394,'f.hofmann@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Phillip','Panzenböck','09923411',203432940,'p.panzi@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Lukas','Riedl','06991888292',2321323,'lukas.riedliscool@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Robin','Mair','123123213123',000033222,'r.mair@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Tobias','Schicker','08992323',0123123213,'t.schicker@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Max','Mustermann','123123',02030230,'m.mustermann@gmail.com');
insert into dbo.People(FirstName,LastName,PhoneNumber,Picture,Email) values('Jonas','Freund','098889291',021300230,'j.freund@gmail.com');

insert into dbo.Contacts(FK_Person) select id from dbo.People where LastName = 'Mistlberger';
insert into dbo.Contacts(FK_Person) select id from dbo.People where LastName = 'Panzenböck';
insert into dbo.Contacts(FK_Person) select id from dbo.People where LastName = 'Mair';
insert into dbo.Contacts(FK_Person) select id from dbo.People where LastName = 'Mustermann';

insert into dbo.Addresses(Street,Number,City,PostalCode) values('Teststraße', 1, 'Teststadt',4040);
insert into dbo.Addresses(Street,Number,City,PostalCode) values('Teststraße', 2, 'Teststadt',4040);
insert into dbo.Addresses(Street,Number,City,PostalCode) values('Teststraße', 3, 'Teststadt',4040);
insert into dbo.Addresses(Street,Number,City,PostalCode) values('Teststraße', 4, 'Teststadt',4040);

insert into Companies(Name,Email,Homepage,PhoneNumber,ShortDescription,CompanySign,SubjectAreas,FK_Address,FK_Contact) 
	select 'TestFirma 1', 'testfirma1@gmail.com', 'testfirma1.com', 12309089891230, 'Wir sind eine Testfirma', 92394342932949234,'Informatik',a.id, c.id 
	from Addresses a, Contacts c 
	where a.Number=1 and c.FK_Person = (select id from People where LastName = 'Mistlberger');

insert into Companies(Name,Email,Homepage,PhoneNumber,ShortDescription,CompanySign,SubjectAreas,FK_Address,FK_Contact) 
	select 'TestFirma 2', 'testfirma2@gmail.com', 'testfirma2.com', 12309089891230, 'Wir sind eine Testfirma', 92394342932949234,'Medientechnik',a.id,c.id
	from Addresses a, Contacts c 
	where a.Number=2 and c.FK_Person = (select id from People where LastName = 'Panzenböck');

insert into Companies(Name,Email,Homepage,PhoneNumber,ShortDescription,CompanySign,SubjectAreas,FK_Address,FK_Contact) 
	select 'TestFirma 3', 'testfirma3@gmail.com', 'testfirma3.com', 12309089891230, 'Wir sind eine Testfirma', 92394342932949234,'Elektronik',a.id,c.id
	from Addresses a, Contacts c 
	where a.Number=3 and c.FK_Person = (select id from People where LastName = 'Mair');

insert into Companies(Name,Email,Homepage,PhoneNumber,ShortDescription,CompanySign,SubjectAreas,FK_Address,FK_Contact) 
	select 'TestFirma 4', 'testfirma4@gmail.com', 'testfirma4.com', 12309089891230, 'Wir sind eine Testfirma', 92394342932949234,'Biomedizin',a.id,c.id
	from Addresses a, Contacts c 
	where a.Number=4 and c.FK_Person = (select id from People where LastName = 'Mustermann');


insert into dbo.Bookings(FK_Event,FK_Company,FK_Location,FK_Presentation,FK_Category,isAccepted) 
	select e.id,c.id,l.id,p.id,cat.id,'true'
	from dbo.Events e, dbo.Companies c, dbo.Locations l, dbo.Presentations p, dbo.Categories cat
	where e.Date = 27-11-2017 and c.Name = 'TestFirma 1' and l.Number = 10 and p.RoomNumber=101 and cat.Name='sponsor';

insert into dbo.Bookings(FK_Event,FK_Company,FK_Location,FK_Presentation,FK_Category,isAccepted) 
	select e.id,c.id,l.id,p.id,cat.id,'false'
	from dbo.Events e, dbo.Companies c, dbo.Locations l, dbo.Presentations p, dbo.Categories cat
	where e.Date = 27-11-2016 and c.Name = 'TestFirma 2' and l.Number = 15 and p.RoomNumber=102 and cat.Name='premium sponsor';

insert into dbo.Bookings(FK_Event,FK_Company,FK_Location,FK_Presentation,FK_Category,isAccepted) 
	select e.id,c.id,l.id,p.id,cat.id,'false'
	from dbo.Events e, dbo.Companies c, dbo.Locations l, dbo.Presentations p, dbo.Categories cat
	where e.Date = 27-11-2017 and c.Name = 'TestFirma 3' and l.Number = 101 and p.RoomNumber=102 and cat.Name='sponsor';

insert into dbo.Resources(name,description) values('Wlan', 'Wir brauchen Internet'); 
insert into dbo.Resources(name,description) values('Tisch', 'Wir brauchen Tische');
insert into dbo.Resources(name,description) values('Sessel', 'Wir brauchen Sessel');
insert into dbo.Resources(name,description) values('Wasser', 'Wir brauchen Wasser');
insert into dbo.Resources(name,Description)values('Kaffee', 'Wir brauchen Kaffee');
insert into dbo.ResourceBookings(fk_resource,fk_booking,amount) 
	select r.id,b.id,1
    from dbo.Resources r, dbo.Bookings b
	where r.Name='Wlan' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 1');
insert into dbo.ResourceBookings(fk_resource,fk_booking,amount) 
	select r.id,b.id,2
    from dbo.Resources r, dbo.Bookings b
	where r.Name='Tisch' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 1');
insert into dbo.ResourceBookings(fk_resource,fk_booking,amount) 
	select r.id,b.id,4
    from dbo.Resources r, dbo.Bookings b
	where r.Name='Sessel' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 1');
insert into dbo.ResourceBookings(fk_resource,fk_booking,amount) 
	select r.id,b.id,1
    from dbo.Resources r, dbo.Bookings b
	where r.Name='Wlan' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 2');
insert into dbo.ResourceBookings(fk_resource,fk_booking,amount) 
	select r.id,b.id,2
    from dbo.Resources r, dbo.Bookings b
	where r.Name='Sessel' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 3');
insert into dbo.ResourceBookings(fk_resource,fk_booking,amount) 
	select r.id,b.id,2
    from dbo.Resources r, dbo.Bookings b
	where r.Name='Kaffee' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 4');

insert into dbo.Lecturers(FK_Person,FK_Presentation)
	 select p.id,pres.id 
	 from dbo.People p, dbo.Presentations pres
	 where p.LastName = 'Mistlberger' and pres.Title='C#';

insert into dbo.Lecturers(FK_Person,FK_Presentation)
	 select p.id,pres.id 
	 from dbo.People p, dbo.Presentations pres
	 where p.LastName = 'Hofmann' and pres.Title='Java';

insert into dbo.Lecturers(FK_Person,FK_Presentation)
	 select p.id,pres.id 
	 from dbo.People p, dbo.Presentations pres
	 where p.LastName = 'Panzenböck' and pres.Title='Phyton';

insert into dbo.Lecturers(FK_Person,FK_Presentation)
	 select p.id,pres.id 
	 from dbo.People p, dbo.Presentations pres
	 where p.LastName = 'Mair' and pres.Title='Sql';

insert into dbo.Lecturers(FK_Person,FK_Presentation)
	 select p.id,pres.id 
	 from dbo.People p, dbo.Presentations pres
	 where p.LastName = 'Schicker' and pres.Title='C';



insert into dbo.Representatives(FK_Booking,FK_Person)
	select b.id,p.id
	from dbo.Bookings b, dbo.People p
	where p.LastName = 'Freund' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 1');

insert into dbo.Representatives(FK_Booking,FK_Person)
	select b.id,p.id
	from dbo.Bookings b, dbo.People p
	where p.LastName = 'Mustermann' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 1');

insert into dbo.Representatives(FK_Booking,FK_Person)
	select b.id,p.id
	from dbo.Bookings b, dbo.People p
	where p.LastName = 'Hofmann' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 2');

insert into dbo.Representatives(FK_Booking,FK_Person)
	select b.id,p.id
	from dbo.Bookings b, dbo.People p
	where p.LastName = 'Mair' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 3');

insert into dbo.Representatives(FK_Booking,FK_Person)
	select b.id,p.id
	from dbo.Bookings b, dbo.People p
	where p.LastName = 'Riedl' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 4');

insert into dbo.Details(Description) values('Plakatständer');
insert into dbo.DetailAllocations(FK_Booking,FK_Detail,Text)
	select b.id,d.id,'Plakat aufhängen'
	from dbo.Bookings b, dbo.Details d
	where d.Description = 'Plakatständer' and b.FK_Company=(select id from dbo.Companies where Name='TestFirma 1');
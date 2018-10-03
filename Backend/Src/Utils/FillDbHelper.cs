using Backend.Core;
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Backend.Src.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Utils {
    public static class FillDbHelper {

        public async static Task createTestData(ApplicationDbContext _context, UserManager<FitUser> userManager) {

            // Admin
            FitUser fitUser = new FitUser();
            fitUser.Email = "simon.kepplinger@gmail.com";
            fitUser.UserName = fitUser.Email;
            fitUser.Role = "FitAdmin";

            string hashedPassword = GenerateSHA256String("test123");

            await userManager.CreateAsync(fitUser, hashedPassword);

            SmtpConfig smtpConfig = new SmtpConfig();
            smtpConfig.Host = "smtp.gmail.com";
            smtpConfig.Port = 587;
            smtpConfig.MailAddress = "andi.sakal@gmail.com";
            smtpConfig.Password = "sombor123";
            _context.SmtpConfigs.Add(smtpConfig);

            Console.WriteLine("Search for Companies who want to join FIT ...");

            MemberStatus memberStatus = new MemberStatus();
            memberStatus.DefaultPrice = 0;
            memberStatus.Name = "keine Mitgliedschaft";

            MemberStatus memberStatus2 = new MemberStatus();
            memberStatus2.DefaultPrice = 0;
            memberStatus2.Name = "interessiert";

            MemberStatus memberStatus3 = new MemberStatus();
            memberStatus3.DefaultPrice = 200;
            memberStatus3.Name = "kleine Mitgliedschaft";

            MemberStatus memberStatus4 = new MemberStatus();
            memberStatus4.DefaultPrice = 400;
            memberStatus4.Name = "große Mitgliedschaft";

            _context.MemberStati.Add(memberStatus);
            _context.MemberStati.Add(memberStatus2);
            _context.MemberStati.Add(memberStatus3);
            _context.MemberStati.Add(memberStatus4);
            _context.SaveChanges();

            // Set up Company
            Company company = new Company();
            company.Name = "Kepplinger IT";
            company.IsAccepted = 1;
            company.RegistrationToken = "FirmenToken1";
            company.MemberStatus = memberStatus3;

            // Set up Address
            Address address = new Address();
            address.Addition = "Additional Address Info";
            address.City = "Linz";
            address.StreetNumber = "14";
            address.Street = "some Street";
            address.ZipCode = "4020";

            _context.Addresses.Add(address);
            _context.SaveChanges();

            // Set Up Contact
            Contact contact = new Contact();
            contact.FirstName = "Andrej";
            contact.LastName = "Sakal";
            contact.Gender = "M";
            contact.PhoneNumber = "+4369917209297";
            contact.Email = "simon.kepplinger@gmail.com";

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            // Set up Company
            company.Contact = contact;
            company.Address = address;

            FitUser companyUser = new FitUser();
            companyUser.UserName = company.RegistrationToken;
            companyUser.Role = "Member";

            await userManager.CreateAsync(companyUser, company.RegistrationToken);

            company.fk_FitUser = companyUser.Id;

            _context.Companies.Add(company);
            _context.SaveChanges();

            Console.WriteLine("Search for Resources in the HTL Leonding ...");
            //Set up Ressources
            Resource resource = new Resource();
            resource.Name = "Stuhl";
            _context.Resources.Add(resource);
            Resource resource2 = new Resource();
            resource2.Name = "Fernseher";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "Stehtisch";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "WLAN";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "Strom";
            _context.Resources.Add(resource2);
            _context.SaveChanges();

            FitPackage package = new FitPackage();
            package.Name = "Basispaket";
            package.Discriminator = 1;
            package.Description = "Das Grundpaket bietet Ihnen einen Standplatz am FIT";
            package.Price = 200;

            _context.Packages.Add(package);
            _context.SaveChanges();

            FitPackage package2 = new FitPackage();
            package2.Name = "Sponsorpaket";
            package2.Discriminator = 2;
            package2.Description = "Beim Sponsorpaket zusätzlich enthalten ist noch anbringung Ihres Firmenlogos auf Werbematerialien des FITs";
            package2.Price = 400;

            _context.Packages.Add(package2);
            _context.SaveChanges();

            FitPackage package3 = new FitPackage();
            package3.Name = "Vortragspaket";
            package3.Discriminator = 3;
            package3.Description = "Beim Vortragspaket zuästzlich zu den restlichen Paketen darf man einen Vortrag halten";
            package3.Price = 600;

            _context.Packages.Add(package3);
            _context.SaveChanges();

            Branch it = new Branch();
            it.Name = "Informatik/Medientechnik";

            _context.Branches.Add(it);
            _context.SaveChanges();

            Branch elektr = new Branch();
            elektr.Name = "Elektronik/techn. Informatik";

            _context.Branches.Add(elektr);
            _context.SaveChanges();

            Branch bio = new Branch();
            bio.Name = "Biomedizin & Gesundheitstechnik";

            _context.Branches.Add(bio);
            _context.SaveChanges();

            Location l = new Location();
            l.Category = "A";
            l.Number = "31";
            l.XCoordinate = 1.0;
            l.XCoordinate = 1.0;

            _context.Locations.Add(l);
            _context.SaveChanges();

            // AREA
            Area a = new Area();
            a.Designation = "Erdgeschoss";
            a.Locations = new List<Location>();
            a.Locations.Add(l);

            Event e = new Event();
            e.EventDate = DateTime.Now;
            e.PresentationsLocked = false;
            e.RegistrationEnd = DateTime.Now.AddMonths(2);
            e.RegistrationStart = DateTime.Now.AddMonths(-2);
            e.RegistrationState = new RegistrationState();
            e.RegistrationState.IsLocked = false;
            e.RegistrationState.IsCurrent = true;
            e.Areas = new List<Area>();
            e.Areas.Add(a);

            _context.Events.Add(e);
            _context.SaveChanges();

            Console.WriteLine("Set up some students in the database ...");

            Graduate g = new Graduate();
            g.LastName = "Kepplinger";
            g.FirstName = "Simon";
            g.Gender = "M";
            g.Email = "simon.kepplinger@gmail.com";
            g.PhoneNumber = "seiFlammenTelNr";
            g.GraduationYear = 2018;
            g.RegistrationToken = "GraduateToken1";

            Address address1 = new Address();
            address1.Street = "Dr. Karl Rennerstraße";
            address1.StreetNumber = "17a";
            address1.ZipCode = "4061";
            address1.City = "Pasching";
            address1.Addition = "A Haus hoid";

            _context.Addresses.Add(address1);
            _context.SaveChanges();

            g.Address = address1;

            FitUser graduateUser = new FitUser();
            graduateUser.UserName = g.RegistrationToken;
            graduateUser.Role = "Member";

            await userManager.CreateAsync(graduateUser, g.RegistrationToken);

            g.fk_FitUser = graduateUser.Id;

            _context.Graduates.Add(g);
            _context.SaveChanges();

            Graduate g2 = new Graduate();
            g2.LastName = "Sakal";
            g2.FirstName = "Andrea";
            g2.Gender = "F";
            g2.Email = "andra.sakal@gmail.com";
            g2.PhoneNumber = "000000007";
            g2.GraduationYear = 2017;
            g2.RegistrationToken = "GraduateToken2";

            Address address2 = new Address();
            address2.Street = "Asphaltstraße";
            address2.StreetNumber = "32";
            address2.ZipCode = "4040";
            address2.City = "Linz";
            address2.Addition = "";

            _context.Addresses.Add(address2);
            _context.SaveChanges();

            g2.Address = address2;

            FitUser graduateUser2 = new FitUser();
            graduateUser2.UserName = g2.RegistrationToken;
            graduateUser2.Role = "Member";

            await userManager.CreateAsync(graduateUser2, g2.RegistrationToken);
            g2.fk_FitUser = graduateUser2.Id;

            _context.Graduates.Add(g2);
            _context.SaveChanges();

            for (int i = 0; i < 1; i++) {
                //Representatives
                List<Representative> repre = new List<Representative>();
                Representative repr = new Representative();
                repr.Email = "andi.sakal15@gmail.com";
                repr.Image = null;
                repr.Name = "Andrej Sakal";

                _context.Representatives.Add(repr);
                _context.SaveChanges();
                repre.Add(repr);

                Presentation p = new Presentation();
                p.Branches = new List<PresentationBranch>();
                p.Description = "Hallo ist da jemand?";
                p.IsAccepted = 0;
                p.Title = "Coole Präsi";
                p.File = null;

                _context.Presentations.Add(p);
                _context.SaveChanges();

                // Set up Booking
                Booking booking = new Booking();
                booking.AdditionalInfo = "Here is some Additional Info";
                booking.CompanyDescription = "This is the company description";
                booking.isAccepted = 0;
                booking.ProvidesSummerJob = true;
                booking.ProvidesThesis = false;
                booking.Remarks = "Remark";
                booking.CreationDate = DateTime.Now;
                booking.fk_FitPackage = package3.Id;
                booking.Event = e;
                booking.Representatives = repre;
                booking.fk_Company = company.Id;
                booking.fk_Contact = contact.Id;
                booking.Email = "officemail@gmail.com";
                booking.Branch = "Firmen Branche";
                booking.EstablishmentsAut = "Linz";
                booking.EstablishmentsCountAut = 1;
                booking.EstablishmentsCountInt = 0;
                booking.EstablishmentsInt = "";
                booking.Homepage = "www.fit.com";
                booking.Logo = null;
                booking.PhoneNumber = "firmenphonenr";
                booking.Presentation = p;

                _context.Bookings.Add(booking);
                _context.SaveChanges();


                // ressourceBookingCreaten
                booking.Resources = new List<ResourceBooking>();
                ResourceBooking rb = new ResourceBooking();
                rb.Booking = booking;

                rb.Resource = resource;
                _context.ResourceBookings.Add(rb);
                _context.SaveChanges();

                booking.Resources.Add(rb);
                _context.Bookings.Update(booking);
                _context.SaveChanges();
            }
        }

        public static void createEmails(IUnitOfWork uow) {
            #region Mails intialize
            Email presentationRejected = new Email("PR", "Vortrag abgelehnt (Firma)",
                              "Diese Email geht an die Firma und gilt als Ablehnung den Vortrag.",
                              "<!DOCTYPE html>" +
                                           "<html>" +
                                           "<head>" +
                                           "</head>" +
                                           "<body>" +
                                           "<p>Ihr Vortrag wurde abeglehnt!</p>" +
                                           "</body>" +
                                           "</html>",
                              "Ihr Vortrag wurde abeglehnt - ABSLEO HTL Leonding FIT");

            Email presentationAccepted = new Email("PA", "Vortrag bestätigt (Firma)",
                  "Diese Email geht an die Firma und gilt als Bestätigung den Vortrag.",
                  "<!DOCTYPE html>" +
                               "<html>" +
                               "<head>" +
                               "</head>" +
                               "<body>" +
                               "<p>Ihr Vortrag wurde bestätigt!</p>" +
                               "</body>" +
                               "</html>",
                  "Ihr Vortrag wurde bestätigt - ABSLEO HTL Leonding FIT");


            Email bookingRejected = new Email("BR", "Anmeldung abgelehnt (Firma)",
                              "Diese Email geht an die Firma und gilt als Ablehnung der Anmeldung.",
                              "<!DOCTYPE html>" +
                                           "<html>" +
                                           "<head>" +
                                           "</head>" +
                                           "<body>" +
                                           "<p>Ihre Anmeldung wurde abeglehnt!</p>" +
                                           "</body>" +
                                           "</html>",
                              "Ihre Anmeldung wurde abeglehnt - ABSLEO HTL Leonding FIT");

            Email bookingAccepted = new Email("BA", "Anmeldung bestätigt (Firma)",
                               "Diese Email geht an die Firma und gilt als Bestätigungs-Email der Anmeldung.",
                               "<!DOCTYPE html>" +
                                            "<html>" +
                                            "<head>" +
                                            "</head>" +
                                            "<body>" +
                                            "<p>Ihre Anmeldung wurde bestätigt!</p>" +
                                            "</body>" +
                                            "</html>",
                               "Ihre Anmeldung wurde bestätigt - ABSLEO HTL Leonding FIT");

            Email bookingDataRequired = new Email("DR", "FIT-Anmeldungs-Daten ausstehend (Firma)",
                               "Diese Email weist die Firma auf fehlende Daten hin.",
                               "<!DOCTYPE html>" +
                                            "<html>" +
                                            "<head>" +
                                            "</head>" +
                                            "<body>" +
                                            "<p>Folgende Daten fehlen: {{ REQUIRED_DATA }}</p>" +
                                            "</body>" +
                                            "</html>",
                               "Ausstehende Daten - ABSLEO HTL Leonding FIT");

            Email isPendingGottenCompany = new Email("PGC", "Antrag eingereicht (Firma)",
                                "Diese Email geht an die Firma und gilt als Bestätigungsemail eines erfolgreichen Firmenantrags",
                                "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                             "<p>Ihr Antrag wurde empfangen und wird so schnell wie möglich verarbeitet!</p>" +
                                             "</body>" +
                                             "</html>",
                                "Ihr Firmenantrag wurde eingereicht - ABSLEO HTL Leonding FIT");

            Email isPendingGottenAdmin = new Email("PGA", "Antrag eingereicht (Admin)",
                                "Diese Email wird dem Admin bei einer neuen Firmenanmeldung versendet",
                                                   "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                                   "<p>Es wurde ein neuer Antrag eingereicht von der Firma: {{Company.Name}}" +
                                             "</p></body>" +
                                             "</html>",
                               "Ein neuer Firmenantrag wurde eingereich");

            Email IsPendingAcceptedCompany = new Email("PAC", "Firma akzeptiert (Firma)",
                                "Diese Email wird versendet wenn ein Firmenantrag akzeptiert wurde",
                                                       "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                             "<p>Ihr Firmenantrag wurde akzeptiert!" +
                                                       "<br>Hier ist der Zugangstoken: {{Company.RegistrationToken}}" +
                                                       "</p></body>" +
                                             "</html>",
                               "Ihr Firmenantrag wurde akzeptiert - ABSLEO HTL Leonding");

            Email IsPendingDeniedCompany = new Email("PDC", "Firma abgelehnt (Firma)",
                                "Diese Email wird versendet wenn ein Firmenantrag abgelehnt wurde",
                                                     "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                             "<p>Ihr Firmenantrag wurde abgelehnt!" +
                                             "</p></body>" +
                                             "</html>",
                               "Ihr Firmenantrag wurde abgelehnt - ABSLEO HTL Leonding");

            Email CompanyAssigned = new Email("CA", "Firma einer bestehenden zugewiesen (Firma)",
                      "Diese Email wird an die Firma verschickt wenn es die Firma bereits gibt und Sie dieser zugewiesen wurde",
                                  "<!DOCTYPE html>" +
                                   "<html>" +
                                   "<head>" +
                                   "</head>" +
                                   "<body>" +
                                   "<p>Firma bereits vorhanden wurde {{Company.RegistrationToken}}" +
                                   "</body>" +
                                   "</html>",
                     "Firma bereits vorhanden - ABSLEO HTL Leonding");

            Email SendBookingAcceptedMail = new Email("SBA", "FIT-Anmeldung eingereicht (Firma)",
                                "Diese Email wird an die Firma versendet wenn die FIT-Buchung akzeptiert wurde",
                                                      "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                             "<div>" +
                                             "<img src=\"https://www.htl-leonding.at/uploads/pics/HTL_Abteilungsicons_mit_Textbalken_rgb_01.jpg\" align=\"left\" alt=\"Abteilung Logo\" height=\"90px\" width=\"360px\"/>" +
                                             "<img src=\"http://www.htl-leonding.at/fileadmin/config/main/img/htlleondinglogo.png\" align=\"right\" alt=\"HTL Leonding Logo\"/></center>" +
                                             "<br><br><br><br><br><br><br><br>" +
                                             "</div>" +
                                             "<div>" +
                                                      "<p>Sehr geehrte(r) Frau/Herr {{Booking.Company.Contact.LastName}}, " +
                                             "<br><br>" +
                                             "Ihre Anmeldung ist bei uns eingetroffen! Wir freuen uns sehr, dass Sie sich dazu entschieden " +
                                             "haben beim diesjährigen <b>Firmeninformationstag</b> in der <b><a href=\"http://www.htl-leonding.at/\">HTL Leonding</a></b> Ihr Unternehmen vorzustellen!<br>" +
                                             "</p>" +
                                             "<p>Bei Fragen oder Anregungen können Sie uns jederzeit unter der Nr.: <a href=\"tel:+439999999\">+43123456789</a> oder per E-Mail <a href=\"mailto:andi.sakal15@gmail.com\">andi.sakal15@gmail.com</a> erreichen" +
                                             "<br><br>" +
                                             "<p> Mit freundlichen Grüßen <br><br> FirstName LastName - Ihr Ansprechpartner" +
                                             "<br><br></div><img src=\"http://www.absleo.at/typo3temp/processed/csm_absleo_logo_ohne_Rahmen_ba0c412e5a.png\" alt=\"ABSLEO Logo\"/>" +
                                             "</body>" +
                                             "</html>",
                               "FIT-Buchung wurde eingereicht - ABSLEO HTL Leonding FIT");

            Email SendForgottenCompany = new Email("SFC", "Firma-Code vergessen (Firma)",
                                "Dies ist eine FirmenCode vergessen Email",
                                            "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                            "<p>Ihr Token: {{ Company.RegistrationToken }}" +
                                             "</body>" +
                                             "</html>",
                               "ABSLEO HTL Leonding");

            Email SendForgottenGraduate = new Email("SFG", "Firma-Code vergessen (Absolvent)",
                                "Dies ist eine FirmenCode vergessen Email",
                                            "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                            "<p>Ihr Token: {{ Graduate.RegistrationToken }}" +
                                             "</body>" +
                                             "</html>",
                               "ABSLEO HTL Leonding");
            #endregion

            // generate email variables
            List<EmailVariable> variables = new List<EmailVariable> {
                
                // GRADUATE
                new EmailVariable("Absolvent-Vorname", concatFieldPath(nameof(Graduate.FirstName)), nameof(Graduate)),
                new EmailVariable("Absolvent-Nachname", concatFieldPath(nameof(Graduate.LastName)), nameof(Graduate)),
                new EmailVariable("Absolvent-Anrede", concatFieldPath(nameof(Graduate.Gender)), nameof(Graduate)),
                new EmailVariable("geehrte/r Absolvent-Anrede",  "DEAR_" + concatFieldPath(nameof(Graduate.Gender)), nameof(Graduate)),
                new EmailVariable("Absolvent-Abschlussjahr", concatFieldPath(nameof(Graduate.GraduationYear)), nameof(Graduate)),
                new EmailVariable("Absolvent-Telefon", concatFieldPath(nameof(Graduate.PhoneNumber)), nameof(Graduate)),
                new EmailVariable("Login-Token", concatFieldPath(nameof(Graduate.RegistrationToken)), nameof(Graduate)),
                new EmailVariable("Absolvent-Adresse-Straße", concatFieldPath(nameof(Graduate.Address), nameof(Address.Street)), nameof(Graduate)),
                new EmailVariable("Absolvent-Adresse-Hausnummer", concatFieldPath(nameof(Graduate.Address), nameof(Address.StreetNumber)), nameof(Graduate)),
                new EmailVariable("Absolvent-Adresse-Ort", concatFieldPath(nameof(Graduate.Address), nameof(Address.City)), nameof(Graduate)),
                new EmailVariable("Absolvent-Adresse-Postleitzahl", concatFieldPath(nameof(Graduate.Address), nameof(Address.ZipCode)), nameof(Graduate)),
                new EmailVariable("Absolvent-Adresse-Zusatz", concatFieldPath(nameof(Graduate.Address), nameof(Address.Addition)), nameof(Graduate)),

                // COMPANY
                new EmailVariable("Firma-Name", concatFieldPath(nameof(Company.Name)), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Anrede",  concatFieldPath(nameof(Company.Contact), nameof(Contact.Gender)), nameof(Company)),
                new EmailVariable("geehrte/r Firma-Kontakt-Anrede",  "DEAR_" + concatFieldPath(nameof(Company.Contact), nameof(Contact.Gender)), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Vorname", concatFieldPath(nameof(Company.Contact), nameof(Contact.FirstName)), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Nachname", concatFieldPath(nameof(Company.Contact), nameof(Contact.LastName)), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Email", concatFieldPath(nameof(Company.Contact), nameof(Contact.Email)), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Telefon", concatFieldPath(nameof(Company.Contact), nameof(Contact.PhoneNumber)), nameof(Company)),
                new EmailVariable("Firma-Adresse-Straße", concatFieldPath(nameof(Company.Address), nameof(Address.Street)), nameof(Company)),
                new EmailVariable("Firma-Adresse-Hausnummer", concatFieldPath(nameof(Company.Address), nameof(Address.StreetNumber)), nameof(Company)),
                new EmailVariable("Firma-Adresse-Ort", concatFieldPath(nameof(Company.Address), nameof(Address.City)), nameof(Company)),
                new EmailVariable("Firma-Adresse-Postleitzahl", concatFieldPath(nameof(Company.Address), nameof(Address.ZipCode)), nameof(Company)),
                new EmailVariable("Firma-Adresse-Zusatz", concatFieldPath(nameof(Company.Address), nameof(Address.Addition)), nameof(Company)),
                new EmailVariable("Login-Token", concatFieldPath(nameof(Company.RegistrationToken)), nameof(Company)),

                // BOOKING
                new EmailVariable("Firma-Name",  concatFieldPath(nameof(Booking.Company), nameof(Company.Name)), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Anrede",  concatFieldPath(nameof(Booking.Company), nameof(Company.Contact), nameof(Contact.Gender)), nameof(Booking)),
                new EmailVariable("geehrte/r Firma-Kontakt-Anrede",  "DEAR_" + concatFieldPath(nameof(Booking.Company), nameof(Company.Contact), nameof(Contact.Gender)), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Vorname",  concatFieldPath(nameof(Booking.Company), nameof(Company.Contact), nameof(Contact.FirstName)), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Nachname",  concatFieldPath(nameof(Booking.Company), nameof(Company.Contact), nameof(Contact.LastName)), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Email",  concatFieldPath(nameof(Booking.Company), nameof(Company.Contact), nameof(Contact.Email)), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Telefon",  concatFieldPath(nameof(Booking.Company), nameof(Company.Contact), nameof(Contact.PhoneNumber)), nameof(Booking)),
                new EmailVariable("Firma-Adresse-Straße",  concatFieldPath(nameof(Booking.Company), nameof(Company.Address), nameof(Address.Street)), nameof(Booking)),
                new EmailVariable("Firma-Adresse-Hausnummer",  concatFieldPath(nameof(Booking.Company), nameof(Company.Address), nameof(Address.StreetNumber)), nameof(Booking)),
                new EmailVariable("Firma-Adresse-Ort",  concatFieldPath(nameof(Booking.Company), nameof(Company.Address), nameof(Address.City)), nameof(Booking)),
                new EmailVariable("Firma-Adresse-Postleitzahl",  concatFieldPath(nameof(Booking.Company), nameof(Company.Address), nameof(Address.ZipCode)), nameof(Booking)),
                new EmailVariable("Firma-Adresse-Zusatz",  concatFieldPath(nameof(Booking.Company), nameof(Company.Address), nameof(Address.Addition)), nameof(Booking)),

                new EmailVariable("Login-Token",  concatFieldPath(nameof(Booking.Company), nameof(Company.RegistrationToken)), nameof(Booking)),
                new EmailVariable("Email",  concatFieldPath(nameof(Booking.Email)), nameof(Booking)),
                new EmailVariable("Telefon",  concatFieldPath(nameof(Booking.PhoneNumber)), nameof(Booking)),
                new EmailVariable("Homepage",  concatFieldPath(nameof(Booking.Homepage)), nameof(Booking)),
                new EmailVariable("Buchungszeitpunkt",  concatFieldPath(nameof(Booking.CreationDate)), nameof(Booking)),

                new EmailVariable("Stand-Platznummer",  concatFieldPath(nameof(Booking.Location.Number)), nameof(Booking)),
                new EmailVariable("Stand-Kategorie",  concatFieldPath(nameof(Booking.Location.Category)), nameof(Booking)),
                new EmailVariable("Paket-Name",  concatFieldPath(nameof(Booking.FitPackage), nameof(FitPackage.Name)), nameof(Booking)),
                new EmailVariable("Paket-Preis",  concatFieldPath(nameof(Booking.FitPackage), nameof(FitPackage.Price)), nameof(Booking)),

                new EmailVariable("Präsentation-Name",  concatFieldPath(nameof(Booking.Presentation), nameof(Presentation.Title)), nameof(Booking)),
                new EmailVariable("Präsentation-Raum",  concatFieldPath(nameof(Booking.Presentation), nameof(Presentation.RoomNumber)), nameof(Booking)),
                new EmailVariable("Präsentation-Datei",  concatFieldPath(nameof(Booking.Presentation), nameof(Presentation.File), nameof(DataFile.Name)), nameof(Booking)),

                new EmailVariable("Kontakt-Anrede",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.Gender)), nameof(Booking)),
                new EmailVariable("geehrte/r Kontakt-Anrede",  "DEAR_" + concatFieldPath(nameof(Booking.Contact), nameof(Contact.Gender)), nameof(Booking)),
                new EmailVariable("Kontakt-Vorname",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.FirstName)), nameof(Booking)),
                new EmailVariable("Kontakt-Nachname",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.LastName)), nameof(Booking)),
                new EmailVariable("Kontakt-Email",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.Email)), nameof(Booking)),
                new EmailVariable("Kontakt-Telefon",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.PhoneNumber)), nameof(Booking)),
                new EmailVariable("Ausstehende Daten (Liste)", "REQUIRED_DATA", nameof(Booking))
            };

            uow.EmailVariableRepository.InsertMany(variables);
            uow.Save();

            List<EmailVariable> gradauteVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Graduate)).ToList();
            List<EmailVariable> companyVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Company)).ToList();
            List<EmailVariable> bookingVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Booking)).ToList();

            // mapping the variables to the resolve-entity EmailVariableUsage
            CompanyAssigned.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(CompanyAssigned, v)).ToList();
            IsPendingAcceptedCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(IsPendingAcceptedCompany, v)).ToList();
            IsPendingDeniedCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(IsPendingDeniedCompany, v)).ToList();
            isPendingGottenAdmin.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(isPendingGottenAdmin, v)).ToList();
            isPendingGottenCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(isPendingGottenCompany, v)).ToList();
            SendForgottenCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(SendForgottenCompany, v)).ToList();
            SendForgottenGraduate.AvailableVariables = gradauteVariables.Select(v => new EmailVariableUsage(SendForgottenGraduate, v)).ToList();
            SendBookingAcceptedMail.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(SendBookingAcceptedMail, v)).ToList();
            bookingAccepted.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingAccepted, v)).ToList();
            bookingRejected.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingAccepted, v)).ToList();
            bookingDataRequired.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingDataRequired, v)).ToList();
            presentationAccepted.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingDataRequired, v)).ToList();
            presentationRejected.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingDataRequired, v)).ToList();

            // persist emails
            uow.EmailRepository.Insert(CompanyAssigned);
            uow.EmailRepository.Insert(IsPendingAcceptedCompany);
            uow.EmailRepository.Insert(IsPendingDeniedCompany);
            uow.EmailRepository.Insert(isPendingGottenAdmin);
            uow.EmailRepository.Insert(isPendingGottenCompany);
            uow.EmailRepository.Insert(SendForgottenCompany);
            uow.EmailRepository.Insert(SendForgottenGraduate);
            uow.EmailRepository.Insert(SendBookingAcceptedMail);
            uow.EmailRepository.Insert(bookingAccepted);
            uow.EmailRepository.Insert(bookingRejected);
            uow.EmailRepository.Insert(bookingDataRequired);
            uow.EmailRepository.Insert(presentationAccepted);
            uow.EmailRepository.Insert(presentationRejected);
            uow.Save();
        }

        /// <summary>
        /// Use to concat the field name paths of string (e.g. Company.Contact.FirstName).
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private static string concatFieldPath(params string[] args) {
            return String.Join('.', args);
        }

        private static string GenerateSHA256String(string inputString) {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash) {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}

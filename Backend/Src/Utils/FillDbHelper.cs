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
using System.Threading.Tasks;

namespace Backend.Utils {
    public static class FillDbHelper {

        public static void createTestData(ApplicationDbContext _context, UserManager<FitUser> userManager) {

            FitUser fitUser = new FitUser();
            fitUser.Email = "simon.kepplinger@gmail.com";
            fitUser.UserName = fitUser.Email;
            fitUser.Role = "FitAdmin";

            userManager.CreateAsync(fitUser, "test123");

            SmtpConfig smtpConfig = new SmtpConfig();
            smtpConfig.Host = "smtp.gmail.com";
            smtpConfig.Port = 587;
            smtpConfig.MailAddress = "andi.sakal@gmail.com";
            smtpConfig.Password = "sombor123";
            _context.SmtpConfigs.Add(smtpConfig);

            Console.WriteLine("Search for Companies who want to join FIT ...");
            // Set up Company
            Company company = new Company();
            company.Name = "Kepplinger IT";
            company.IsAccepted = 1;
            company.RegistrationToken = "FirmenToken1";

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
            contact.Email = "andi.sakal15@gmail.com";

            _context.Contacts.Add(contact);
            _context.SaveChanges();

            // Set up Company
            company.Contact = contact;
            company.Address = address;

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

            Address address1 = new Address();
            address1.Street = "Dr. Karl Rennerstraße";
            address1.StreetNumber = "17a";
            address1.ZipCode = "4061";
            address1.City = "Pasching";
            address1.Addition = "A Haus hoid";

            Graduate g = new Graduate();
            g.LastName = "Kepplinger";
            g.FirstName = "Simon";
            g.Gender = "M";
            g.Email = "simon.kepplinger@gmail.com";
            g.PhoneNumber = "seiFlammenTelNr";
            g.RegistrationToken = "GraduateToken1";
            g.Address = address1;

            _context.Graduates.Add(g);

            Address address2 = new Address();
            address1.Street = "Asphaltstraße";
            address1.StreetNumber = "32";
            address1.ZipCode = "4040";
            address1.City = "Linz";
            address1.Addition = "";

            Graduate g2 = new Graduate();
            g2.LastName = "Sakal";
            g2.FirstName = "Andrea";
            g2.Gender = "F";
            g2.Email = "andra.sakal@gmail.com";
            g2.PhoneNumber = "000000007";
            g2.RegistrationToken = "GraduateToken2";
            g2.Address = address1;

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
                p.RoomNumber = "E27";
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
                booking.fk_FitPackage = package.Id;
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
                               "Ihr Firmenantrag wurde akzeptiert - ABSLEO HTL Leonding FIT");

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
                               "Ihr Firmenantrag wurde abgelehnt - ABSLEO HTL Leonding FIT");

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
                     "Firma bereits vorhande - ABSLEO HTL Leonding FIT");

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
                               "FIT-Buchung wurde akzeptiert - ABSLEO HTL Leonding FIT");

            Email SendForgotten = new Email("SF", "Firma-Code vergessen (Firma)",
                                "Dies ist eine FirmenCode vergessen Email",
                                            "<!DOCTYPE html>" +
                                             "<html>" +
                                             "<head>" +
                                             "</head>" +
                                             "<body>" +
                                            "<p>Ihr Token: {{Company.RegistrationToken}}" +
                                             "</body>" +
                                             "</html>",
                               "FIT-Anmeldung - ABSLEO HTL Leonding FIT");
            #endregion

            // generate email variables
            List<EmailVariable> variables = new List<EmailVariable> {
                
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
            };

            uow.EmailVariableRepository.InsertMany(variables);
            uow.Save();

            List<EmailVariable> companyVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Company)).ToList();
            List<EmailVariable> bookingVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Booking)).ToList();

            // mapping the variables to the resolve-entity EmailVariableUsage
            CompanyAssigned.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(CompanyAssigned, v)).ToList();
            IsPendingAcceptedCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(IsPendingAcceptedCompany, v)).ToList();
            IsPendingDeniedCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(IsPendingDeniedCompany, v)).ToList();
            isPendingGottenAdmin.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(isPendingGottenAdmin, v)).ToList();
            isPendingGottenCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(isPendingGottenCompany, v)).ToList();
            SendBookingAcceptedMail.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(SendBookingAcceptedMail, v)).ToList();
            SendForgotten.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(SendForgotten, v)).ToList();

            // persist emails
            uow.EmailRepository.Insert(CompanyAssigned);
            uow.EmailRepository.Insert(IsPendingAcceptedCompany);
            uow.EmailRepository.Insert(IsPendingDeniedCompany);
            uow.EmailRepository.Insert(isPendingGottenAdmin);
            uow.EmailRepository.Insert(isPendingGottenCompany);
            uow.EmailRepository.Insert(SendBookingAcceptedMail);
            uow.EmailRepository.Insert(SendForgotten);
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
    }
}

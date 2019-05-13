using Backend.Core;
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Backend.Src.Core.Entities;
using Bogus;
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

        public static int NUMBER_GRADUTE = 20;
        public static int NUMBER_MEMBERSHIP = 7;
        public static int NUMBER_COMPANY = 10;
        public static int NUMBER_PACKAGES = 3;
        public static int NUMBER_RESOURCE = 15;
        public static int NUMBER_BRANCH = 10;
        public enum Gender
        {
            m,
            f
        }

        public async static Task createTestData(ApplicationDbContext _context, UserManager<FitUser> userManager) {
            List<MemberStatus> memberList = new List<MemberStatus>();
            List<Resource> resourceList = new List<Resource>();
            #region admin
            // Admin
            Console.WriteLine("Inserting  Admin-User...");
            FitUser fitUser = new FitUser();
            fitUser.Email = "fit.website.testing.l@gmail.com";
            fitUser.UserName = fitUser.Email;
            fitUser.Role = "FitAdmin";

            string hashedPassword = GenerateSHA256String("test123");

            await userManager.CreateAsync(fitUser, hashedPassword);
            #endregion
            #region LockPage
            Console.WriteLine("Creating Lockpages...");
            LockPage lockPage = new LockPage();
            lockPage.Expired = @"<div>
                                    <p> Leider ist der Anmeldungszeitruam zum FIT leider schon vorüber. Die Ameldung kann nur in einen bestimmten Zeitraum
                                    durchgeführt werden, und auch nur solange noch Plätze frei sind.</ p >
  
                                    <p>Bei Fragen oder anderen Anliegen, können Sie sich gerne mit uns in Kontakt setzen: </p>

                                </div>";
            lockPage.Incoming = @"<div>
                                    <div class=""alert alert-info"" role=""alert"">
                                    Die Anmeldung zum FIT ist erst<span class=""text-bold"">ab dem Versand der Einladungen</span>(Anfang Oktober)
                                    möglich!</div>

                                    <p>Bei Fragen oder anderen Anliegen, können Sie sich gerne mit uns in Kontakt setzen: </p>

                                </div>";
            
            _context.LockPages.Add(lockPage);
            #endregion
            #region smtpConfig
            Console.WriteLine("create smtpConfig...");
            SmtpConfig smtpConfig = new SmtpConfig();
            smtpConfig.Host = "smtp.gmail.com";
            smtpConfig.Port = 587;
            smtpConfig.MailAddress = "andi.sakal@gmail.com";
            smtpConfig.Password = "sombor123";
            _context.SmtpConfigs.Add(smtpConfig);
            #endregion
            #region MemberStatuses
            Console.WriteLine("insert Member Statuse...");
            var member = new Faker<MemberStatus>()
                .RuleFor(m => m.DefaultPrice, f => f.Random.Number(1, 1000))
                .RuleFor(m => m.Name, f => f.Commerce.Product())
                ;//.FinishWith((f, m) => Console.WriteLine(m.Name));
            
            for (int i = 0; i < NUMBER_MEMBERSHIP; i++)
            {
                MemberStatus m = member.Generate();
                _context.MemberStati.Add(m);
                memberList.Add(m);
            }
            _context.SaveChanges();
            #endregion
            #region Companies
            Console.WriteLine("Search for Companies which want to join the FIT ...");
            var companyGen = new Faker<Company>()
                .RuleFor(c => c.Name, f => f.Company.CompanyName())
                .RuleFor(c => c.IsAccepted, f => f.Random.Number(0, 1))
                .RuleFor(c => c.RegistrationToken, f => f.Random.String2(4) + '-' + f.Random.String2(4) + '-' + f.Random.String2(4))
                .RuleFor(c => c.MemberStatus, f => memberList.ElementAt(f.Random.Number(0, NUMBER_MEMBERSHIP - 1)))
                .RuleFor(c => c.MemberPaymentAmount, (f, c) => c.MemberStatus.DefaultPrice);
                ;//.FinishWith((f,c) => Console.WriteLine(c.CompanyName));
            var addressGen = new Faker<Address>()
                .RuleFor(adr => adr.Street, f => f.Address.StreetName())
                .RuleFor(adr => adr.StreetNumber, f => f.Address.StreetAddress())
                .RuleFor(adr => adr.ZipCode, f => f.Address.ZipCode())
                .RuleFor(ard => ard.City, f => f.Address.City())
                .RuleFor(ard => ard.Addition, f => f.Address.SecondaryAddress());
                ;//.FinishWith((f,ard) => Console.WriteLine(ard.Street));
            var contactGen = new Faker<Contact>()
                .RuleFor(c => c.Gender, f => f.PickRandom<Gender>().ToString())
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Email, f => f.Internet.ExampleEmail())
                ;//.FinishWith((f, c) => Console.WriteLine(c.LastName));

           
            for (int i = 0; i < NUMBER_COMPANY; i++)
            {
                    var comp = companyGen.Generate();
                    comp.Name = comp.Name.Substring(0, Math.Min(comp.Name.Length, 30));
                    
                    var add = addressGen.Generate();
                    
                    var cont = contactGen.Generate();
                    

                    _context.Addresses.Add(add);
                    _context.Contacts.Add(cont);
                    _context.SaveChanges();

                    comp.Contact = cont;
                    comp.Address = add;

                    FitUser companyUser = new FitUser();
                    companyUser.UserName = comp.RegistrationToken;
                    companyUser.Role = "Member";

                    await userManager.CreateAsync(companyUser, comp.RegistrationToken);

                    comp.fk_FitUser = companyUser.Id;

                    _context.Companies.Add(comp);
                    _context.SaveChanges();
            }
            #endregion
            #region Resources
            Console.WriteLine("Look for useable Resources for the FIT ...");
            _context.SaveChanges();
            var ress = new Faker<Resource>()
                .RuleFor(r => r.Name, f => f.Hacker.Noun())
                ;//.FinishWith((f, r) => Console.WriteLine(r.Name));
            for (int i = 0; i < NUMBER_GRADUTE; i++)
            {
                Resource res = ress.Generate();
                _context.Resources.Add(res);
                resourceList.Add(res);
            }
            _context.SaveChanges();
            #endregion
            #region Packages
            Console.WriteLine("work out Packages...");
            var packageGen = new Faker<FitPackage>()
                .RuleFor(p => p.Name, f => "Package-" + f.Name.FirstName())
                .RuleFor(p => p.Description, f => f.Rant.Review())
                .RuleFor(p => p.Price, f => f.Random.Number(0, 1000));

            for (int i = 0; i < NUMBER_RESOURCE; i++)
            {
                FitPackage pack = packageGen.Generate();
                _context.Packages.Add(pack);
                _context.SaveChanges();
            }
            #endregion
            #region Branches
            Console.WriteLine("insert Branches...");
            var branchGen = new Faker<Branch>()
                .RuleFor(b => b.Name, f => "Branch -" + f.Name.FirstName());

            for (int i = 0; i < NUMBER_BRANCH; i++)
            {
                Branch b = branchGen.Generate();
                _context.Branches.Add(b);
                _context.SaveChanges();
            }
            #endregion
            #region Event + Area + Locations
            Console.WriteLine("create Events with ther Locations... ");
            Event e = new Event();
            e.EventDate = DateTime.Now;
            e.PresentationsLocked = false;
            e.RegistrationEnd = DateTime.Now.AddMonths(2);
            e.RegistrationStart = DateTime.Now.AddMonths(-2);
            e.RegistrationState = new RegistrationState();
            e.RegistrationState.IsLocked = false;
            e.RegistrationState.IsCurrent = true;
            e.Areas = new List<Area>();
            _context.Events.Add(e);
            _context.SaveChanges();

            var locationGen = new Faker<Location>()
                .RuleFor(lo => lo.Category, f => f.Random.Char().ToString())
                .RuleFor(lo => lo.Number, f => f.Random.Number(1, 100).ToString())
                .RuleFor(lo => lo.XCoordinate, f => f.Random.Double(1,20))
                .RuleFor(lo => lo.YCoordinate, f => f.Random.Double(1,20));
            var areaGen = new Faker<Area>()
                .RuleFor(ar => ar.Designation, f => f.Name.FullName())
                .RuleFor(ar => ar.Locations, f => new List<Location>());
            for (int i = 0; i < 3; i++)
            {
                Area area = areaGen.Generate();
                for (int j = 0; j < 3; j++)
                {
                    Location loc = locationGen.Generate();
                    _context.Locations.Add(loc);
                    _context.SaveChanges();

                    area.Locations.Add(loc);
                }
                e.Areas.Add(area);
            }

            Console.WriteLine("Set up some students in the database ...");
            #endregion
            #region Graduates
            Console.WriteLine("Insert finished Students...");
            var graduateGen = new Faker<Graduate>()
                .RuleFor(gr => gr.Gender, f => f.PickRandom<Gender>().ToString())
                .RuleFor(gr => gr.FirstName, f => f.Name.FirstName())
                .RuleFor(gr => gr.LastName, f => f.Name.LastName())
                .RuleFor(gr => gr.Email, (f, u) => f.Internet.ExampleEmail(u.FirstName, u.LastName))
                .RuleFor(gr => gr.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(gr => gr.GraduationYear, f => f.Random.Number(2000, 2019))
                .RuleFor(gr => gr.RegistrationToken, f => f.Random.String2(12, 12))
                ;//.FinishWith((f,gr) => Console.WriteLine(gr.LastName));

            for (int i = 0; i < NUMBER_GRADUTE; i++)
            {
                var graduate = graduateGen.Generate();
                var adr = addressGen.Generate();

                _context.Addresses.Add(adr);
                _context.SaveChanges();

                graduate.Address = adr;
                FitUser graduateUser = new FitUser();
                graduateUser.UserName = graduate.RegistrationToken;
                graduateUser.Role = "Member";

                await userManager.CreateAsync(graduateUser, graduate.RegistrationToken);

                graduate.fk_FitUser = graduateUser.Id;

                _context.Graduates.Add(graduate);
                _context.SaveChanges();
            }
            #endregion
            #region Bookings
            Console.WriteLine("create bookings for each company...");
            var representativeGen = new Faker<Representative>()
                .RuleFor(r => r.Email, f => f.Internet.ExampleEmail())
                .RuleFor(r => r.Image, f => null)
                .RuleFor(r => r.Name, f => f.Name.FullName());
            var bookingGen = new Faker<Booking>()
                    .RuleFor(b => b.AdditionalInfo, f => "Additional Information: " + f.Random.String2(10))
                    .RuleFor(b => b.CompanyDescription, f => "Company Description: " + f.Random.String2(10))
                    .RuleFor(b => b.isAccepted, f => f.Random.Number(0, 1))
                    .RuleFor(b => b.ProvidesSummerJob, f => f.Random.Bool())
                    .RuleFor(b => b.ProvidesThesis, f => f.Random.Bool())
                    .RuleFor(b => b.Remarks, f => "Remarks")
                    .RuleFor(b => b.CreationDate, f => f.Date.Past())
                    .RuleFor(b => b.fk_FitPackage, f => f.Random.Number(1, NUMBER_PACKAGES ))
                    .RuleFor(b => b.Event, f => e)
                    //.RuleFor(b => b.Representatives, f => repr)
                    //.RuleFor(b => b.fk_Company, f => f.Random.Number(0, NUMBER_COMPANY - 1))
                    //.RuleFor(b => b.fk_Contact, f => f.Random.Number(0, NUMBER_COMPANY - 1))
                    .RuleFor(b => b.Email, f => f.Internet.ExampleEmail())
                    .RuleFor(b => b.Branch, f => "Branche " + f.Random.String2(10))
                    .RuleFor(b => b.EstablishmentsAut, f => f.Address.City())
                    .RuleFor(b => b.EstablishmentsCountAut, f => 1)
                    .RuleFor(b => b.EstablishmentsCountInt, f => 0)
                    .RuleFor(b => b.EstablishmentsInt, f => "")
                    .RuleFor(b => b.Homepage, f => f.Internet.Url())
                    .RuleFor(b => b.Logo, f => null)
                    .RuleFor(b => b.PhoneNumber, f => f.Phone.PhoneNumber())
                    ;//.RuleFor(b => b.Presentation, f => p);
            var pressentationGen = new Faker<Presentation>()
                .RuleFor(p => p.Branches, f => new List<PresentationBranch>())
                .RuleFor(p => p.Description, f => f.Random.String2(20))
                .RuleFor(p => p.IsAccepted, f => f.Random.Number(0, 1))
                .RuleFor(p => p.Title, f => "Präsi - " + f.Name.LastName())
                .RuleFor(p => p.File, f => null);
            var resourseBookingGen = new Faker<ResourceBooking>()
                .RuleFor(rb => rb.Resource, f => resourceList.ElementAt(f.Random.Number(0, resourceList.Count -1)));
            Booking book;
            for (int i = 0; i < 10; i++) {
                //Representatives
                List<Representative> repre = new List<Representative>();
                Representative repr = representativeGen.Generate();

                _context.Representatives.Add(repr);
                _context.SaveChanges();
                repre.Add(repr);

                Presentation p = pressentationGen.Generate();

                _context.Presentations.Add(p);
                _context.SaveChanges();

                // Set up Booking
                Booking booking = bookingGen.Generate();
                booking.Representatives = repre;
                booking.Presentation = p;
                booking.fk_Company = i +1;
                booking.fk_Contact = i +1;
                book = booking;

                _context.Bookings.Add(booking);
                _context.SaveChanges();

                booking.Resources = new List<ResourceBooking>();
                for (int j = 0; j < 4; j++)
                {
                    ResourceBooking rb = resourseBookingGen.Generate();
                    _context.ResourceBookings.Add(rb);
                    _context.SaveChanges();

                    booking.Resources.Add(rb);
                    _context.Bookings.Update(booking);
                    _context.SaveChanges();
                }
                // ressourceBookingCreaten

            }
            #endregion
        }

        public static void createEmails(IUnitOfWork uow) {
            #region Mails intialize
            Email fitInvation = new Email("FI", "FIT Einladung (Firma)",
                  "Einladung für den derzeitigen FIT",
                  "<!DOCTYPE html>" +
                               "<html>" +
                               "<head>" +
                               "</head>" +
                               "<body>" +
                               "<p>Sie sind herzlich eingeladen!</p>" +
                               "</body>" +
                               "</html>",
                  "Einladung für den FIT - ABSLEO HTL Leonding FIT");

            Email presentationRejected = new Email("PR", "Vortrag abgelehnt (Firma)",
                              "Ablehnung des Vortrags der jeweiligen Firma.",
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
                  "Bestätigung des Vortrags der jeweiligen Firma.",
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
                                            "<p>Folgende Daten fehlen: {{ Booking.REQUIRED_DATA }}</p>" +
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

                new EmailVariable("Kontakt-Anrede",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.Gender)), nameof(Booking)),
                new EmailVariable("geehrte/r Kontakt-Anrede",  "DEAR_" + concatFieldPath(nameof(Booking.Contact), nameof(Contact.Gender)), nameof(Booking)),
                new EmailVariable("Kontakt-Vorname",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.FirstName)), nameof(Booking)),
                new EmailVariable("Kontakt-Nachname",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.LastName)), nameof(Booking)),
                new EmailVariable("Kontakt-Email",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.Email)), nameof(Booking)),
                new EmailVariable("Kontakt-Telefon",  concatFieldPath(nameof(Booking.Contact), nameof(Contact.PhoneNumber)), nameof(Booking)),
                new EmailVariable("Ausstehende Daten (Liste)", "REQUIRED_DATA", nameof(Booking)),

                // EVENT
                new EmailVariable("FIT-Datum",  concatFieldPath(nameof(Booking.Event), nameof(Event.EventDate)), nameof(Booking)),
                new EmailVariable("FIT-Anmelde-Anfang",  concatFieldPath(nameof(Booking.Event), nameof(Event.RegistrationStart)), nameof(Booking)),
                new EmailVariable("FIT-Anmelde-Ende",  concatFieldPath(nameof(Booking.Event), nameof(Event.RegistrationEnd)), nameof(Booking)),

                // PRESENTATION
                new EmailVariable("Präsentation-Name",  concatFieldPath(nameof(Booking.Presentation), nameof(Presentation.Title)), nameof(Presentation)),
                new EmailVariable("Präsentation-Raum",  concatFieldPath(nameof(Booking.Presentation), nameof(Presentation.RoomNumber)), nameof(Presentation)),
                new EmailVariable("Präsentation-Datei",  concatFieldPath(nameof(Booking.Presentation), nameof(Presentation.File), nameof(DataFile.Name)), nameof(Presentation)),
            };

            List<EmailVariable> invitaionDates = new List<EmailVariable> {
                new EmailVariable("FIT-Datum",  "FIT_DATE", nameof(Company)),
                new EmailVariable("FIT-Anmelde-Anfang",  "FIT_REG_START", nameof(Company)),
                new EmailVariable("FIT-Anmelde-Ende",  "FIT_REG_END", nameof(Company)),
            };

            uow.EmailVariableRepository.InsertMany(variables);
            uow.Save();

            List<EmailVariable> gradauteVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Graduate)).ToList();
            List<EmailVariable> companyVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Company)).ToList();
            List<EmailVariable> bookingVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Booking)).ToList();
            List<EmailVariable> presentationVariables = uow.EmailVariableRepository.Get(filter: v => v.Entity == nameof(Presentation)).ToList();

            // mapping the variables to the resolve-entity EmailVariableUsage
            CompanyAssigned.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(CompanyAssigned, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            IsPendingAcceptedCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(IsPendingAcceptedCompany, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            IsPendingDeniedCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(IsPendingDeniedCompany, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            isPendingGottenAdmin.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(isPendingGottenAdmin, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            isPendingGottenCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(isPendingGottenCompany, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            SendForgottenCompany.AvailableVariables = companyVariables.Select(v => new EmailVariableUsage(SendForgottenCompany, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            SendForgottenGraduate.AvailableVariables = gradauteVariables.Select(v => new EmailVariableUsage(SendForgottenGraduate, v)).OrderBy(v => v.EmailVariable.Name).ToList();

            fitInvation.AvailableVariables = companyVariables.Concat(invitaionDates).Select(v => new EmailVariableUsage(fitInvation, v)).OrderBy(v => v.EmailVariable.Name).ToList();

            SendBookingAcceptedMail.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(SendBookingAcceptedMail, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            bookingAccepted.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingAccepted, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            bookingRejected.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingRejected, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            bookingDataRequired.AvailableVariables = bookingVariables.Select(v => new EmailVariableUsage(bookingDataRequired, v)).OrderBy(v => v.EmailVariable.Name).ToList();

            presentationAccepted.AvailableVariables = bookingVariables.Concat(presentationVariables)
                .Select(v => new EmailVariableUsage(presentationAccepted, v)).OrderBy(v => v.EmailVariable.Name).ToList();
            presentationRejected.AvailableVariables = bookingVariables.Concat(presentationVariables)
                .Select(v => new EmailVariableUsage(presentationRejected, v)).OrderBy(v => v.EmailVariable.Name).ToList();

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
            uow.EmailRepository.Insert(fitInvation);
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

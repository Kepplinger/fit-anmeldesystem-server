﻿using Backend.Core;
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Persistence;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Utils
{
    public static class FillDbHelper
    {
        public static void createTestData(ApplicationDbContext _context)
        {
            Console.WriteLine("Search for Companies who want to join FIT ...");
            // Set up Company
            Company company = new Company();
            company.Name = "Kepplinger IT";
            company.IsPending = false;
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
            resource.Description = "Braucht die Firma einen Stuhl";
            _context.Resources.Add(resource);
            Resource resource2 = new Resource();
            resource2.Name = "Fernseher";
            resource2.Description = "Die Firma braucht einen Fernseher";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "Stehtisch";
            resource2.Description = "Die Firma braucht einen Stehtisch";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "WLAN";
            resource2.Description = "Die Firma braucht WLAN";
            _context.Resources.Add(resource2);
            resource2 = new Resource();
            resource2.Name = "Strom";
            resource2.Description = "Die Firma braucht Strom";
            _context.Resources.Add(resource2);
            _context.SaveChanges();

            //Representatives
            List<Representative> repre = new List<Representative>();
            Representative repr = new Representative();
            repr.Email = "andi.sakal15@gmail.com";
            repr.Image = null;
            repr.Name = "Andrej Sakal";

            _context.Rerpresentatives.Add(repr);
            _context.SaveChanges();
            repre.Add(repr);

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

            /*Presentation p = new Presentation();
            p.Branches = new List<PresentationBranches>();

            p.Branches.Add(it);
            p.Description = "zad mi nimma";
            p.IsAccepted = false;
            p.RoomNumber = "nrofroom";
            p.Title = "title";
            p.FileURL = "http://";

            _context.Presentations.Add(p);*/
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

            _context.Areas.Add(a);
            _context.SaveChanges();

            Event e = new Event();
            e.EventDate = DateTime.Now;
            e.RegistrationEnd = DateTime.Now.AddMonths(2);
            e.RegistrationStart = DateTime.Now.AddMonths(-2);
            e.IsLocked = false;
            e.IsCurrent = true;
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
            g.RegistrationToken = "GraduateToken1";
            g.Address = address;

            _context.Graduates.Add(g);
            _context.SaveChanges();

            for (int i = 0; i < 11; i++)
            {

                // Set up Booking
                Booking booking = new Booking();
                booking.AdditionalInfo = "Here is some Additional Info";
                booking.CompanyDescription = "This is the company description";
                booking.isAccepted = true;
                booking.ProvidesSummerJob = true;
                booking.ProvidesThesis = false;
                booking.Remarks = "Remark";
                booking.CreationDate = DateTime.Now;
                booking.fk_FitPackage = package.Id;
                booking.Event = e;
                booking.Representatives = repre;
                booking.fk_Company = company.Id;
                booking.Email = "officemail@gmail.com";
                booking.Branch = "Firmen Branche";
                booking.EstablishmentsAut = "Linz";
                booking.EstablishmentsCountAut = 1;
                booking.EstablishmentsCountInt = 0;
                booking.EstablishmentsInt = "";
                booking.Homepage = "www.fit.com";
                booking.Logo = null;
                booking.PhoneNumber = "firmenphonenr";

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

        public static void createEmails(IUnitOfWork uow)
        {
            #region Mails intialize
            Email isPendingGottenCompany = new Email("isPendingGottenCompany",
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

            Email isPendingGottenAdmin = new Email("isPendingGottenAdmin",
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

            Email IsPendingAcceptedCompany = new Email("IsPendingAcceptedCompany",
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

            Email IsPendingDeniedCompany = new Email("IsPendingDeniedCompany",
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

            Email CompanyAssigned = new Email("CompanyAssigned",
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

            Email SendBookingAcceptedMail = new Email("SendBookingAcceptedMail",
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

            Email SendForgotten = new Email("SendForgotten",
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

            List<EmailVariable> variables = new List<EmailVariable> {
                new EmailVariable("Firma-Name", nameof(Company.Name), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Anrede", nameof(Company.Contact.Gender), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Vorname", nameof(Company.Contact.FirstName), nameof(Company)),
                new EmailVariable("Firma-Kontakt-Nachname", nameof(Company.Contact.LastName), nameof(Company)),
                new EmailVariable("Login-Token", nameof(Company.RegistrationToken), nameof(Company)),
                new EmailVariable("Mitglied seit", nameof(Company.MemberSince), nameof(Company)),
                new EmailVariable("Firma-Name", nameof(Booking.Company.Name), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Anrede", nameof(Booking.Company.Contact.Gender), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Vorname", nameof(Booking.Company.Contact.FirstName), nameof(Booking)),
                new EmailVariable("Firma-Kontakt-Nachname", nameof(Booking.Company.Contact.LastName), nameof(Booking)),
                new EmailVariable("Login-Token", nameof(Booking.Company.RegistrationToken), nameof(Booking)),
                new EmailVariable("Mitglied seit", nameof(Booking.Company.MemberSince), nameof(Booking)),
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

            uow.EmailRepository.Insert(CompanyAssigned);
            uow.EmailRepository.Insert(IsPendingAcceptedCompany);
            uow.EmailRepository.Insert(IsPendingDeniedCompany);
            uow.EmailRepository.Insert(isPendingGottenAdmin);
            uow.EmailRepository.Insert(isPendingGottenCompany);
            uow.EmailRepository.Insert(SendBookingAcceptedMail);
            uow.EmailRepository.Insert(SendForgotten);
            uow.Save();
        }
    }
}

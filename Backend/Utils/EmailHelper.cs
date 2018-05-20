﻿using System.Net;
using System.Net.Mail;
using Backend.Core.Entities;
using System;
using Backend.Core.Contracts;
using StoreService.Persistence;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using Backend.Core;

namespace Backend.Utils
{
    public static class EmailHelper
    {
        // isPendingGottenCompany
        // isPendingGottenAdmin
        // IsPendingAcceptedCompany
        // IsPendingDeniedCompany
        // CompanyAssigned
        // SendBookingAcceptedMail
        // SendForgotten

        public static void SendMail(Email mail, object param, string reciever)
        {
            //client config 
            SmtpClient client = EmailHelper.GetSmtpClient();

            //message config 
            MailMessage objeto_mail = new MailMessage();
            objeto_mail.Subject = mail.Subject;
            objeto_mail.From = new MailAddress("andi.sakal15@gmail.com");
            objeto_mail.To.Add(new MailAddress(reciever));
            objeto_mail.IsBodyHtml = true;
            client.SendMailAsync(objeto_mail);
            objeto_mail.Body = mail.Template;
        }

        public static bool SendMailByName(String mailName, object param, string reciever)
        {
            Email mail;

            using (IUnitOfWork uow = new UnitOfWork())
            {
                mail = uow.EmailRepository.Get(m => m.Name.ToLower().Equals(mailName.ToLower())).FirstOrDefault();
            }

            if (mail != null)
            {
                // Client config
                SmtpClient client = EmailHelper.GetSmtpClient();

                // Message config 
                MailMessage objeto_mail = new MailMessage();
                objeto_mail.Subject = mail.Subject;
                objeto_mail.From = new MailAddress("andi.sakal15@gmail.com");
                objeto_mail.To.Add(new MailAddress(reciever));
                objeto_mail.IsBodyHtml = true;

                // Add PDF-Attachment for Booking-Registrations
                if (mailName.Equals("SendBookingAcceptedMail") && param is Booking)
                {
                    EmailHelper.attachRegistrationPdfToMail(objeto_mail, param as Booking);
                }

                objeto_mail.Body = mail.Template; //replaceParamsWithValues(new Company(), mail.Template);
                client.SendMailAsync(objeto_mail);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Searches mail for {{ variableName }} occurrences an replaces them with the corresponding value.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static string replaceParamsWithValues(object param, string template)
        {
            for (int i = 0; i < template.Length - 2; i++)
            {
                string paramName = "";
                string temp = "";

                if (i == 569)
                {
                    Console.WriteLine();
                }

                string checker = template.Substring(i, 2);
                if (checker.Equals("{{") == true)
                {
                    // von {{ bis ende kürzen
                    temp = template.Substring(i + 2);
                    //paramName = temp.Substring(i + 2, );

                    // per reflection value von dem param holen

                    if (param.GetType().Name.Equals(nameof(Company)))
                    {
                        Company c = new Company();
                        paramName = paramName.ToLower().Replace("company.", "");

                        //var variable = GetPropValue(param, paramName);
                    }

                }
                checker = "";
            }
            return "";
        }

        public static Object GetPropValue(this Object obj, String propName)
        {
            string[] nameParts = propName.Split('.');
            if (nameParts.Length == 1)
            {
                return obj.GetType().GetProperty(propName).GetValue(obj, null);
            }

            foreach (String part in nameParts)
            {
                if (obj == null) { return null; }

                Type type = obj.GetType();
                PropertyInfo info = type.GetProperty(part);
                if (info == null) { return null; }

                obj = info.GetValue(obj, null);
            }
            return obj;
        }

        public static void InitializeEmails()
        {
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

            using (IUnitOfWork uow = new UnitOfWork())
            {
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

        private static void attachRegistrationPdfToMail(MailMessage objeto_mail, Booking booking)
        {
            string file;

            using (IUnitOfWork uow = new UnitOfWork())
            {
                file = uow.BookingRepository.GetById(booking.Id).PdfFilePath;
            }

            byte[] bytes = System.IO.File.ReadAllBytes(file);
            objeto_mail.Attachments.Add(new Attachment(file));
        }

        private static SmtpClient GetSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Timeout = 10000;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("andi.sakal@gmail.com", "sombor123");

            return client;
        }
    }
}
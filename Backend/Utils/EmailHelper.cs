using System.Net;
using System.Net.Mail;
using Backend.Core.Entities;
using System;
using Backend.Core.Contracts;
using StoreService.Persistence;
using System.Linq;
using System.Reflection;
using System.IO;

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
            MailMessage objeto_mail = new MailMessage();

            //client config 
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Timeout = 10000;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("andi.sakal@gmail.com", "sombor123");

            //message config 
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
            if (mail == null)
            {
                return false;
            }
            else
            {
                MailMessage objeto_mail = new MailMessage();

                // Client config
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.Timeout = 10000;
                client.UseDefaultCredentials = false;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential("andi.sakal@gmail.com", "sombor123");

                // Message config 
                objeto_mail.Subject = mail.Subject;
                objeto_mail.From = new MailAddress("andi.sakal15@gmail.com");
                objeto_mail.To.Add(new MailAddress(reciever));
                objeto_mail.IsBodyHtml = true;
                if (mailName.Equals("SendBookingAcceptedMail"))
                {
                    byte[] bytes = System.IO.File.ReadAllBytes("<pdfFile>");
                    objeto_mail.Attachments.Add(new Attachment(new MemoryStream(bytes), ""));
                }
                client.SendMailAsync(objeto_mail);
                objeto_mail.Body = mail.Template;
                return true;
            }
        }

        public static string replaceParamsWithValues(object param, string template)
        {
            for (int i = 0; i < template.Length - 2; i++)
            {
                String paramName = "";
                String temp = "";
                if (template.Substring(i, i + 2).Equals("{{") == true)
                {
                    // von {{ bis ende kürzen
                    temp = template.Substring(i + 2, template.Length);
                    paramName = temp.Substring(i + 2, temp.IndexOf("}}"));

                    // per reflection value von dem param holen

                    if (param.GetType().Name.Equals(nameof(Company)))
                    {
                        Company c = new Company();
                        paramName = paramName.ToLower().Replace("company.","");

                        var variable = GetPropValue(param, paramName);
                    }

                }
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

            using (IUnitOfWork uow = new UnitOfWork())
            {
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
}
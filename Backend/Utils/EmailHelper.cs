using System.Net;
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
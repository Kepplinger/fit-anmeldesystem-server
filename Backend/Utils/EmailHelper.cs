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
using System.Text.RegularExpressions;

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

                replaceParamsWithValues(mail, param);
                objeto_mail.Body = mail.Template;
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
        public static void replaceParamsWithValues(Email email, object param)
        {
            // replace every variable within double curly braces
            email.Template = Regex.Replace(
                email.Template,
                "{{[^}}]*}}",
                (Match m) => {

                    // remove all whitespaces and double curly braces
                    string variable = Regex.Replace(m.Value, "(\\s+|{{|}})", string.Empty);

                    return param.GetPropValue(variable).ToString();
                }
            );
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
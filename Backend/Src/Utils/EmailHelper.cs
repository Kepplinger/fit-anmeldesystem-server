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
using System.Text.RegularExpressions;
using Backend.Src.Core.Entities;

namespace Backend.Utils {
    public static class EmailHelper {
        // isPendingGottenCompany
        // isPendingGottenAdmin
        // IsPendingAcceptedCompany
        // IsPendingDeniedCompany
        // CompanyAssigned
        // SendBookingAcceptedMail
        // SendForgotten

        public static bool SendMail(Email mail, object param, string reciever, IUnitOfWork unitOfWork) {
            // Client config
            SmtpConfig smtpConfig = unitOfWork.SmtpConfigRepository.Get().FirstOrDefault();

            if (smtpConfig != null) {

                SmtpClient client = EmailHelper.GetSmtpClient(smtpConfig);

                // Message config 
                MailMessage objeto_mail = new MailMessage();
                objeto_mail.Subject = mail.Subject;
                objeto_mail.From = new MailAddress(smtpConfig.MailAddress);
                objeto_mail.To.Add(new MailAddress(reciever));
                objeto_mail.IsBodyHtml = true;

                // Add PDF-Attachment for Booking-Registrations
                if (mail.Identifier.Equals("SBA") && param is Booking) {
                    EmailHelper.attachRegistrationPdfToMail(objeto_mail, param as Booking);
                }

                replaceParamsWithValues(mail, param);
                objeto_mail.Body = mail.Template;
                client.SendMailAsync(objeto_mail);
                return true;

            } else {
                return false;
            }
        }

        public static bool SendMailByIdentifier(String mailName, object param, string reciever, IUnitOfWork unitOfWork) {
            Email mail;

            using (IUnitOfWork uow = new UnitOfWork()) {
                mail = uow.EmailRepository.Get(m => m.Identifier.ToLower().Equals(mailName.ToLower())).FirstOrDefault();
            }

            if (mail != null) {
                return SendMail(mail, param, reciever, unitOfWork);
            } else {
                return false;
            }
        }

        /// <summary>
        /// Searches mail for {{ variableName }} occurrences an replaces them with the corresponding value.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static void replaceParamsWithValues(Email email, object param) {

            if (param != null) {

                email.Template = Regex.Replace(
                email.Template,
                "{{ GENDER_TITLE }}",
                (Match m) => {

                    if ((param as Company).Contact.Gender == "M") {
                        return "geehrter";
                    } else {
                        return "geehrte";
                    }
                });

                // replace every variable within double curly braces
                email.Template = Regex.Replace(
                    email.Template,
                    "{{[^}}]*}}",
                    (Match m) => {

                        // remove all whitespaces and double curly braces
                        string variable = Regex.Replace(m.Value, "(\\s+|{{|}})", string.Empty);
                        string value = "";

                        try {
                            if (variable.Contains("DEAR_")) {
                                variable = Regex.Replace(m.Value, "DEAR_", string.Empty);
                                value = param.GetPropValue(variable).ToString();

                                switch (value) {
                                    case "M":
                                        return "geehrter Herr";
                                        break;
                                    case "F":
                                        return "geehrte Frau";
                                }

                            } else {
                                value = param.GetPropValue(variable).ToString();

                                if (variable.ToLower().Contains("gender")) {
                                    switch (value) {
                                        case "M":
                                            return "Herr";
                                        case "F":
                                            return "Frau";
                                    }
                                }
                            }
                        } catch (Exception) { }

                        return value;
                    }
                );
            }
        }

        private static void attachRegistrationPdfToMail(MailMessage objeto_mail, Booking booking) {
            string file;

            using (IUnitOfWork uow = new UnitOfWork()) {
                file = uow.BookingRepository.GetById(booking.Id).PdfFilePath;
            }

            byte[] bytes = System.IO.File.ReadAllBytes(file);
            objeto_mail.Attachments.Add(new Attachment(file));
        }

        private static SmtpClient GetSmtpClient(SmtpConfig smtpConfig) {
            SmtpClient client = new SmtpClient();
            client.Host = smtpConfig.Host;
            client.Port = smtpConfig.Port;
            client.Timeout = 10000;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(smtpConfig.MailAddress, smtpConfig.Password);

            return client;
        }
    }
}
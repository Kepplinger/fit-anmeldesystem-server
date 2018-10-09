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
using Microsoft.AspNetCore.Identity;
using Backend.Core.Entities.UserManagement;
using System.Threading.Tasks;
using System.Threading;

namespace Backend.Utils {
    public static class EmailHelper {

        public static bool SendMail(Email mail, string reciever, SmtpConfig smtpConfig, object param = null, IUnitOfWork unitOfWork = null) {
            if (smtpConfig != null) {

                SmtpClient client = EmailHelper.GetSmtpClient(smtpConfig);
                string filePath = null;

                // Message config 
                MailMessage objeto_mail = new MailMessage();
                objeto_mail.Subject = mail.Subject;
                objeto_mail.From = new MailAddress(smtpConfig.MailAddress);
                objeto_mail.To.Add(new MailAddress(reciever));
                objeto_mail.IsBodyHtml = true;

                if (param != null) {
                    // Add PDF-Attachment for Booking-Registrations
                    if (mail.Identifier.Equals("SBA") && param is Booking) {
                        filePath = EmailHelper.AttachRegistrationPdfToMail(objeto_mail, param as Booking);
                    }

                    ReplaceParamsWithValues(mail, param, unitOfWork);
                }

                objeto_mail.Body = mail.Template;
                Task mailTask = client.SendMailAsync(objeto_mail);

                return true;

            } else {
                return false;
            }
        }

        public static bool SendMail(Email mail, object param, string reciever, IUnitOfWork unitOfWork) {
            // Client config
            SmtpConfig smtpConfig = unitOfWork.SmtpConfigRepository.Get().FirstOrDefault();
            return SendMail(mail, reciever, smtpConfig, param, unitOfWork);
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

        public static void SendMailToAllFitAdmins(String mailName, object param, IUnitOfWork unitOfWork, UserManager<FitUser> userManager) {
            List<string> adminMails = userManager.Users.Where(u => u.Role == "FitAdmin").Select(u => u.Email).ToList();

            foreach (string mail in adminMails) {
                SendMailByIdentifier(mailName, param, mail, unitOfWork);
            }
        }

        /// <summary>
        /// Searches mail for {{ variableName }} occurrences an replaces them with the corresponding value.
        /// </summary>
        /// <param name="param"></param>
        /// <param name="template"></param>
        /// <returns></returns>
        public static void ReplaceParamsWithValues(Email email, object param, IUnitOfWork unitOfWork) {

            Event currentEvent = null;

            if (email.Identifier == "FI" || email.Identifier == "C") {
                currentEvent = unitOfWork.EventRepository.Get(e => e.RegistrationState.IsCurrent).FirstOrDefault();
            }

            if (param != null) {

                // replace every variable within double curly braces
                email.Template = Regex.Replace(
                    email.Template,
                    "{{[^}}]*}}",
                    (Match m) => {

                        // remove all whitespaces and double curly braces
                        string variable = Regex.Replace(m.Value, "(\\s+|{{|}})", string.Empty);
                        string value = "";

                        try {
                            if (variable == "Booking.REQUIRED_DATA") {
                                return GetListOfRequiredData(param as Booking);
                            }

                            if (variable == "Company.FIT_DATE") {
                                return currentEvent.EventDate.ToString("dd.MM.yyyy");
                            }

                            if (variable == "Company.FIT_REG_START") {
                                return currentEvent.RegistrationStart.ToString("dd.MM.yyyy");
                            }

                            if (variable == "Company.FIT_REG_END") {
                                return currentEvent.RegistrationEnd.ToString("dd.MM.yyyy");
                            }

                            if (variable.Contains("DEAR_")) {
                                variable = Regex.Replace(variable, "DEAR_", string.Empty);
                                value = param.GetPropValue(variable).ToString();

                                switch (value) {
                                    case "M":
                                        return "geehrter";
                                    case "F":
                                        return "geehrte";
                                }
                            } else {
                                object propValue = param.GetPropValue(variable);

                                if (propValue is DateTime) {
                                    value = ((DateTime)propValue).ToString("dd.MM.yyyy");
                                } else {
                                    value = propValue.ToString();
                                }

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

        public static bool HasPendingData(Booking booking) {

            bool presentationPending = false;

            if (booking.FitPackage.Discriminator == 3 && booking.Presentation != null) {
                presentationPending = booking.Presentation.File == null;
            }

            return booking.Logo == null
                || booking.Representatives.Any(r => r.Image == null)
                || booking.Location == null
                || presentationPending;
        }

        private static string GetListOfRequiredData(Booking booking) {

            string list = String.Empty;

            if (booking.Logo == null) {
                list += "<li>Firmen-Logo</li>";
            }

            if (booking.Representatives.Any(r => r.Image == null)) {
                list += "<li>Vertreter-Fotos</li>";
            }

            if (booking.Location == null) {
                list += "<li>Standplatz</li>";
            }

            if (booking.FitPackage.Discriminator == 3 && booking.Presentation != null && booking.Presentation.File == null) {
                list += "<li>Vortrags-Datei</li>";
            }

            if (list == String.Empty) {
                list = "Alle nötigen Daten sind angegeben";
            } else {
                list = "<ul>" + list + "</ul>";
            }

            return list;
        }

        private static string AttachRegistrationPdfToMail(MailMessage objeto_mail, Booking booking) {
            string file;

            using (IUnitOfWork uow = new UnitOfWork()) {
                file = uow.BookingRepository.GetById(booking.Id).PdfFilePath;
            }

            objeto_mail.Attachments.Add(new Attachment(file));
            return file;
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
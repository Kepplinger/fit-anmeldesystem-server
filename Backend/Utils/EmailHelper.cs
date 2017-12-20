﻿using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using Backend.Core.Entities;
using RazorLight;

namespace Backend.Utils
{
    public static class EmailHelper
    {

        public static void SendMail(Booking succBooking)
        {
            Booking succBooking2 = succBooking;
            MailMessage objeto_mail = new MailMessage();

            //client config
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Timeout = 10000;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("andi.sakal15@gmail.com", "robert&andrej2015");

            //message config
            objeto_mail.Subject = "Bestätigung Ihrer Buchung - ABSLEO HTL Leonding FITs";
            objeto_mail.From = new MailAddress("andi.sakal15@gmail.com");
            objeto_mail.To.Add(new MailAddress(succBooking.Company.Email));
            objeto_mail.IsBodyHtml = true;

            string templatePath = $@"{Directory.GetCurrentDirectory()}/EmailTemplates";

            EngineFactory ef = new EngineFactory();

            IRazorLightEngine engine = ef.ForFileSystem(templatePath);

            var model = succBooking;

            string result = engine.CompileRenderAsync("AcceptedBooking.cshtml", model).Result;
            objeto_mail.Body = result;

            client.SendMailAsync(objeto_mail);

        }
    }
}

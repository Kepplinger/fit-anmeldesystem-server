using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Backend.Src.Core.Entities;
using Backend.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Src.Utils.TestData.Generators
{
    public class StaticTestDataGenerator
    {
        public UserManager<FitUser> userManager;
        public ApplicationDbContext context;

        public StaticTestDataGenerator(ApplicationDbContext context, UserManager<FitUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public async Task GenerateAdmin()
        {
            Console.WriteLine("Inserting  Admin-User...");
            FitUser fitUser = new FitUser();
            fitUser.Email = "fit.website.testing.l@gmail.com";
            fitUser.UserName = fitUser.Email;
            fitUser.Role = "FitAdmin";

            string hashedPassword = GenerateSHA256String("test123");

            await userManager.CreateAsync(fitUser, hashedPassword);
        }

        public async Task GenerateSmtp()
        {
            Console.WriteLine("create smtpConfig...");
            SmtpConfig smtpConfig = new SmtpConfig();
            smtpConfig.Host = "smtp.gmail.com";
            smtpConfig.Port = 587;
            smtpConfig.MailAddress = "andi.sakal@gmail.com";
            smtpConfig.Password = "sombor123";
            context.SmtpConfigs.Add(smtpConfig);
        }

        public async Task GenerateMemberStati()
        {
            MemberStatus memberStatus = new MemberStatus();
            memberStatus.DefaultPrice = 0;
            memberStatus.Name = "keinen";
            FillDbHelper.memberList.Add(memberStatus);

            MemberStatus memberStatus2 = new MemberStatus();
            memberStatus2.DefaultPrice = 0;
            memberStatus2.Name = "interessiert";
            FillDbHelper.memberList.Add(memberStatus2);

            MemberStatus memberStatus3 = new MemberStatus();
            memberStatus3.DefaultPrice = 200;
            memberStatus3.Name = "kleine Mitgliedschaft";
            FillDbHelper.memberList.Add(memberStatus3);

            MemberStatus memberStatus4 = new MemberStatus();
            memberStatus4.DefaultPrice = 400;
            memberStatus4.Name = "große Mitgliedschaft";
            FillDbHelper.memberList.Add(memberStatus4);

            context.MemberStati.Add(memberStatus);
            context.MemberStati.Add(memberStatus2);
            context.MemberStati.Add(memberStatus3);
            context.MemberStati.Add(memberStatus4);
            context.SaveChanges();
        }

        public async Task GeneratePackages()
        {
            FitPackage package = new FitPackage();
            package.Name = "Basispaket";
            package.Discriminator = 1;
            package.Description = "Das Grundpaket bietet Ihnen einen Standplatz am FIT";
            package.Price = 200;

            context.Packages.Add(package);
            context.SaveChanges();

            FitPackage package2 = new FitPackage();
            package2.Name = "Sponsorpaket";
            package2.Discriminator = 2;
            package2.Description = "Beim Sponsorpaket zusätzlich enthalten ist noch anbringung Ihres Firmenlogos auf Werbematerialien des FITs";
            package2.Price = 400;

            context.Packages.Add(package2);
            context.SaveChanges();

            FitPackage package3 = new FitPackage();
            package3.Name = "Vortragspaket";
            package3.Discriminator = 3;
            package3.Description = "Beim Vortragspaket zuästzlich zu den restlichen Paketen darf man einen Vortrag halten";
            package3.Price = 600;

            context.Packages.Add(package3);
            context.SaveChanges();
        }

        public async Task GenerateBranches()
        {
            Branch it = new Branch();
            it.Name = "Informatik/Medientechnik";

            context.Branches.Add(it);
            context.SaveChanges();

            Branch elektr = new Branch();
            elektr.Name = "Elektronik/techn. Informatik";

            context.Branches.Add(elektr);
            context.SaveChanges();

            Branch bio = new Branch();
            bio.Name = "Biomedizin & Gesundheitstechnik";

            FillDbHelper.branches.Add(it);
            FillDbHelper.branches.Add(elektr);
            FillDbHelper.branches.Add(bio);
            context.Branches.Add(bio);
            context.SaveChanges();
        }

        public async Task GenerateLockPage()
        {
            Console.WriteLine("Creating Lockpages...");
            LockPage lockPage = new LockPage();
            lockPage.Expired = @"<div>
                                    <p>Der Anmeldungszeitraum zum FIT ist leider schon vorüber. Die Ameldung kann nur in einen bestimmten Zeitraum
                                    durchgeführt werden, und auch nur solange noch Plätze frei sind.</ p >
  
                                    <p>Bei Fragen oder anderen Anliegen, können Sie sich gerne mit uns in Kontakt setzen: </p>

                                </div>";
            lockPage.Incoming = @"<div>
                                    <div class=""alert alert-info"" role=""alert"">
                                    Die Anmeldung zum FIT ist erst <span class=""text-bold""> ab dem Versand der Einladungen</span>
                                    möglich!</div>

                                    <p>Bei Fragen oder anderen Anliegen, können Sie sich gerne mit uns in Kontakt setzen: </p>

                                </div>";

            context.LockPages.Add(lockPage);
        }

        private static string GenerateSHA256String(string inputString)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        private static string GetStringFromHash(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
    }
}

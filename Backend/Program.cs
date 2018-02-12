using System.IO;
using Backend.Core.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Backend.Core.Contracts;

namespace Backend
{
    public class Program
    {
        private static string bindurl = string.Empty;

        public static void Main(string[] args)
        {
            using (IUnitOfWork uow = new StoreService.Persistence.UnitOfWork())
            {
                uow.FillDb();
            }
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
<<<<<<< HEAD
            bindurl = configuration["Urls:DefaultUrl"];
            System.Console.WriteLine("!!!You are at reachable at: " + bindurl + "");
=======
            bindurl = configuration["Urls:ServerUrl"];
            System.Console.WriteLine("!!!You are at" + bindurl + "Mode!!!");
>>>>>>> master
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args)
                   .UseUrls(bindurl)
                    .UseStartup<Startup>()
                     .Build();
        }
    }
}

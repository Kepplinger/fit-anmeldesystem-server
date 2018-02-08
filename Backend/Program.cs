using System.IO;
using Backend.Core.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Backend
{
    public class Program
    {

        public static string bindurl = "";

        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            bindurl = configuration["Urls:ServerUrl "];
            System.Console.WriteLine("!!!You are at" + bindurl + "Mode!!!");
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

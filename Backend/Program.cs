using System.IO;
using Backend.Core.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var Configuration = builder.Build();
            var bookingJson = JsonConvert.SerializeObject(new Booking());
            var booking = JsonConvert.DeserializeObject<Booking>(bookingJson);                                      
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .UseUrls("http://localhost:8080")
            .UseStartup<Startup>()
            .Build();
    }
}

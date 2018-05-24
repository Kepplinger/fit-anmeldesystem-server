using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class MediaController
    {
        private IUnitOfWork _unitOfWork;
        string images = @"wwwroot/images/";
        string temp = @"wwwroot/tempImages/";

        public MediaController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [Produces("application/zip")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        public FileContentResult ZipArchive()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string url = configuration["Urls:ServerUrl"];

            string[] files = System.IO.Directory.GetFiles(images);

            // Copy the files and overwrite destination files if they already exist.
            foreach (string s in files)
            {
                // Use static Path methods to extract only the file name from the path.
                if (!s.Contains("zip"))
                {
                    string fileName = System.IO.Path.GetFileName(s);
                    string destFile = System.IO.Path.Combine(temp, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }
            if (System.IO.File.Exists("wwwroot/images/images.zip"))
            {
                System.IO.File.Delete("wwwroot/images/images.zip");
            }
            ZipFile.CreateFromDirectory("wwwroot/tempImages/", "wwwroot/images/images.zip");

            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes("wwwroot/images/images.zip"), "application/zip")
            {
                FileDownloadName = "images.zip"
            };
            return result;
        }
    }
}

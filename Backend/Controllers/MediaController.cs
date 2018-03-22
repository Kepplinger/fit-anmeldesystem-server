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

        public MediaController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult ZipArchive()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string url = configuration["Urls:DefaultUrl"];
            Directory.CreateDirectory("wwwroot/tempImages/");
            File.Copy("wwwroot/images/", "wwwroot/tempImages/");
            ZipFile.CreateFromDirectory("wwwroot/tempImages/", "wwwroot/images/images.zip");
            return new OkObjectResult("wwwroot/images/images.zip");
        }
    }
}

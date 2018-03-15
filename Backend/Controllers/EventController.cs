using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Backend.Core.Entities;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
using StoreService.Persistence;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EventController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public EventController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllEvents()
        {
            using(IUnitOfWork uow = new UnitOfWork())
            {
                List<Event> events = uow.EventRepository.Get().ToList();
                if (events.Count > 0)
                {
                    return new OkObjectResult(events);
                }
                return new NoContentResult();
            }
        }

        /// <summary>
        /// Creates a Event Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        public IActionResult CreateEventWithAreasAndLocations([FromBody] Event jsonEvent)
        {
            try
            {
                if (jsonEvent.Id > 0)
                {
                    Event eventToUpdate = _unitOfWork.EventRepository.Get(p => p.Id == jsonEvent.Id, includeProperties: "Areas").FirstOrDefault();
                    if (eventToUpdate != null)
                    {
                        foreach (Area area in jsonEvent.Areas)
                        {
                            /*if (area.GraphicURL.Contains("base64,"))
                            {
                                string filename = this.ImageParsing(area);
                                area.GraphicURL = filename;
                                if (area.Id > 0)
                                {
                                    _unitOfWork.AreaRepository.Update(area);
                                }
                                else
                                {
                                    _unitOfWork.AreaRepository.Insert(area);
                                }
                                _unitOfWork.Save();
                            }*/
                            _unitOfWork.EventRepository.Update(jsonEvent);
                            _unitOfWork.Save();
                        }
                        return new OkObjectResult(jsonEvent);
                    }
                }
                else
                {

                    jsonEvent.IsCurrent = true;
                    //  && _unitOfWork.EventRepository.Get(filter: p => p.IsLocked == false).FirstOrDefault() == null sollte nur ein mögliches Event geben TESTZWECK
                    if (jsonEvent != null)
                    {
                        // Saving Areas and Locations for the Event
                        foreach (Area area in jsonEvent.Areas)
                        {
                            /*string filename = this.ImageParsing(area);
                            area.GraphicURL = filename;*/

                            foreach (Location l in area.Locations)
                            {
                                _unitOfWork.LocationRepository.Insert(l);
                            }

                            _unitOfWork.Save();
                            _unitOfWork.AreaRepository.Insert(area);
                        }
                        _unitOfWork.EventRepository.Insert(jsonEvent);
                        _unitOfWork.Save();
                        return new OkObjectResult(jsonEvent);
                    }
                }

                return new BadRequestObjectResult(jsonEvent);
            }
            catch (DbUpdateException ex)
            {
                String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                System.Console.WriteLine(error);

                return new BadRequestObjectResult(error);
            }
        }

        /// <response code="200">Return current Event</response>
        /// <summary>
        /// Getting all Events from Database
        /// </summary>
        /// wi warads wann amoi duachgschaut wiad wos fia methoden das es scho gibt und east dann programmiert wida btw die methode steht 1 drunta 

            //----------------------DIE METHODE WÜRDE ÜBRIGENS GEHEN WENN MAN NCIHT .toList() SONDERN .firs() MACHT 
        //[HttpGet("current")]
        //[ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        //public IActionResult GetCurrentEvent()
        //{
        //    List<Event> events = _unitOfWork
        //                                .EventRepository
        //                                .Get().Where(p => p.EventDate.Year.Equals(DateTime.Now.Year) == true)
        //                                .Where(f => _unitOfWork.EventRepository.Get()
        //                                .Any(d => f.EventDate.Subtract(DateTime.Now) < d.EventDate
        //                                .Subtract(DateTime.Now)))
        //                                .ToList();
        //                                //.Select(p => p.EventDate.Subtract(DateTime.Now)).ToList();
        //    return new OkObjectResult(events);
        //}

        [HttpGet("latest")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetLatestEvent()
        {
            Event e = _unitOfWork.EventRepository.Get(orderBy: c => c.OrderByDescending(t => t.EventDate)).First();
            return new OkObjectResult(e);
        }


        /// <summary>
        /// My utils
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public string ImageParsing(Area area)
        {
            // parse from 64 String all image infos
            string dataFormat = String.Empty;
            int indexof = area.GraphicURL.IndexOf("base64,");
            string start = area.GraphicURL.Substring(0, indexof);
            string baseString = area.GraphicURL.Substring(indexof + 7);

            // Check image file format
            if (start.ToLower().Contains("png"))
                dataFormat = ".png";
            else if (start.ToLower().Contains("jpg"))
                dataFormat = ".jpg";
            else if (start.ToLower().Contains("jpeg"))
                dataFormat = ".jpeg";

            //Read filepath from appsetting.json
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            var configuration = builder.Build();
            string filepath = configuration["ImageFilePaths:SakalWindows"];

            //set filepath name
            string filename = this.GetHashString(area.Designation) + dataFormat;
            filepath = filepath + filename;

            // filepath
            //string filepath = @"C:\Users\andis\Desktop\Projects\fit-anmeldesystem-server\Backend\bin\Debug\netcoreapp2.0\images\" + area.Designation + dataFormat;

            // Save image to disk
            this.Base64ToImage(baseString, filepath);

            return filename;
        }
        public object Base64ToImage(string basestr, string filepath)
        {
            byte[] imageBytes = Convert.FromBase64String(basestr);
            //MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            //ms.Write(imageBytes, 0, imageBytes.Length);
            using (var imageFile = new FileStream(filepath, FileMode.Create))
            {
                imageFile.Write(imageBytes, 0, imageBytes.Length);
                imageFile.Flush();
                return imageFile;
                //images/name.jpg
            }
        }
        public byte[] GetHash(string inputString)
        {
            HashAlgorithm algorithm = SHA256.Create();  //SHA256
            return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in this.GetHash(inputString))
                sb.Append(b.ToString("X2"));
            return sb.ToString();
        }


    }
}

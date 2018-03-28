using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Backend.Core.Entities;
using System.Collections.Generic;
using StoreService.Persistence;
using Backend.Utils;

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
                List<Event> events = uow.EventRepository.Get(includeProperties: "Areas").ToList();
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
                    jsonEvent.IsCurrent = eventToUpdate.IsCurrent;
                    if (eventToUpdate != null)
                    {
                        foreach (Area area in jsonEvent.Areas)
                        {
                            if (area.GraphicURL.Contains("base64,"))
                            {
                                string filename = new ImageHelper().ImageParsing(area);
                                area.GraphicURL = filename;

                                foreach (Location l in area.Locations)
                                {
                                    if (l.Id > 0)
                                    {
                                        _unitOfWork.LocationRepository.Update(l);
                                    }
                                    else
                                    {
                                        _unitOfWork.LocationRepository.Insert(l);
                                    }
                                }

                                if (area.Id > 0)
                                {
                                    _unitOfWork.AreaRepository.Update(area);
                                }
                                else
                                {
                                    _unitOfWork.AreaRepository.Insert(area);
                                }
                                _unitOfWork.Save();
                            }
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
                            string filename = new ImageHelper().ImageParsing(area);
                            area.GraphicURL = filename;

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
    }
}

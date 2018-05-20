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
            using (IUnitOfWork uow = new UnitOfWork())
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
                    if (eventToUpdate != null)
                    {
                        int cnt = jsonEvent.Areas.Count;
                        for (int i = 0; i < cnt; i++)
                        {
                            Area area = jsonEvent.Areas.ElementAt(i);
                            if (area.Graphic.DataUrl.Contains("base64,"))
                            {
                                string filename = ImageHelper.ImageParsing(area);
                                area.Graphic.DataUrl = filename;

                                foreach (Location l in area.Locations)
                                {
                                    if (l.Id > 0)
                                    {
                                        _unitOfWork.LocationRepository.Update(l);
                                        _unitOfWork.Save();
                                    }
                                    else
                                    {
                                        _unitOfWork.LocationRepository.Insert(l);
                                        _unitOfWork.Save();
                                    }
                                }
                                if (area.Id > 0)
                                {
                                    _unitOfWork.AreaRepository.Update(area);
                                    _unitOfWork.Save();
                                }
                                else
                                {
                                    area.fk_Event = jsonEvent.Id;
                                    _unitOfWork.AreaRepository.Insert(area);
                                    _unitOfWork.Save();
                                }
                                _unitOfWork.Save();
                            }
                        }
                        //_unitOfWork.EventRepository.Update(jsonEvent);
                        //_unitOfWork.Save();
                        //jsonEvent.IsCurrent = eventToUpdate.IsCurrent;
                        //_unitOfWork.EventRepository.Update(jsonEvent);
                        for (int i = 0; i < jsonEvent.Areas.Count; i++)
                        {
                            jsonEvent.Areas.ElementAt(i).fk_Event = jsonEvent.Id;
                            _unitOfWork.EventRepository.Update(jsonEvent);
                            _unitOfWork.Save();
                        }
                        _unitOfWork.Save();
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
                            string filepath = ImageHelper.ImageParsing(area);
                            area.Graphic.DataUrl = filepath;

                            foreach (Location l in area.Locations)
                            {
                                _unitOfWork.LocationRepository.Insert(l);
                                _unitOfWork.Save();
                            }

                            _unitOfWork.AreaRepository.Insert(area);
                            _unitOfWork.Save();
                        }
                        _unitOfWork.EventRepository.Insert(jsonEvent);
                        _unitOfWork.Save();
                        this.GetCurrentEventLogic();
                        return new OkObjectResult(jsonEvent);
                    }
                    else
                    {
                        var error = new
                        {
                            errorMessage = "Es sind keine Areas und Locations vorhanden!"
                        };
                        return new BadRequestObjectResult(error);
                    }
                }

                return new BadRequestObjectResult(jsonEvent);
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException != null)
                {
                    String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                    System.Console.WriteLine(error);
                    return new BadRequestObjectResult(error);
                }
                else
                {
                    String error = "*********************\n\nDbUpdateException Message: " + ex.Message;
                    System.Console.WriteLine(error);
                    return new BadRequestObjectResult(error);
                }
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

        [HttpGet("current")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetLatestEvent()
        {
            Event e;
            if ((e = this.GetCurrentEventLogic()) != null)
            {
                return new OkObjectResult(e);
            }
            else
            {
                return new NoContentResult();
            }
        }

        public Event GetCurrentEventLogic()
        {
            List<Event> allEvents = _unitOfWork.EventRepository.Get().ToList();

            if (allEvents != null && allEvents.Count > 0)
            {
                Event curEvent;

                // look if future events available (differ for 2 algo)
                List<Event> futureEvents = _unitOfWork.EventRepository
                                       .Get().Where(p => p.EventDate.Year.Equals(DateTime.Now.Year) == true)
                                       .Where(f => _unitOfWork.EventRepository.Get()
                                       .Any(d => f.EventDate.Subtract(DateTime.Now) > TimeSpan.Zero)).ToList();

                if (futureEvents != null && futureEvents.Count > 0)
                {
                    curEvent = _unitOfWork
                                       .EventRepository
                                       .Get().Where(p => p.EventDate.Year.Equals(DateTime.Now.Year) == true)
                                       .Where(f => _unitOfWork.EventRepository.Get()
                                       .Any(d => f.EventDate.Subtract(DateTime.Now) > TimeSpan.Zero && d.EventDate.Subtract(DateTime.Now) <= f.EventDate
                                       .Subtract(DateTime.Now))).OrderBy(q => q.EventDate.Subtract(DateTime.Now)).FirstOrDefault();
                }
                else
                {
                    TimeSpan s;
                    TimeSpan s1;
                    DateTime mynow = DateTime.Now;
                    curEvent = _unitOfWork
                                       .EventRepository
                                       .Get().Where(p => p.EventDate.Year.Equals(DateTime.Now.Year) == true)
                                       .Where(f => _unitOfWork.EventRepository.Get()
                                       .Any(d => (s = d.EventDate.Subtract(mynow)) <= (s1 = f.EventDate
                                       .Subtract(mynow))))
                                       .OrderByDescending(q => q.EventDate.Subtract(mynow))
                                       .FirstOrDefault();
                }

                // if curr event is available set it to isCurrent true and set al other to false
                if (curEvent != null)
                {
                    List<Event> events = _unitOfWork.EventRepository.Get(p => p.IsCurrent == true).ToList();

                    if (events != null && events.Count > 0)
                    {
                        for (int i = 0; i < events.Count; i++)
                        {
                            events.ElementAt(i).IsCurrent = false;
                            _unitOfWork.EventRepository.Update(events.ElementAt(i));
                            _unitOfWork.Save();
                        }
                    }

                    curEvent.IsCurrent = true;
                    _unitOfWork.EventRepository.Update(curEvent);
                    _unitOfWork.Save();
                    return curEvent;
                }
            }
            else
            {
                return null;
            }
            return null;
        }
    }
}
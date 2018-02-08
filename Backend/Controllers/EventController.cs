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

        /// <summary>
        /// Creates a Event Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status400BadRequest)]
        public IActionResult CreateEventWithAreasAndLocations([FromBody] Event jsonEvent)
        {
            List<Event> active = _unitOfWork.EventRepository.Get(p => p.IsCurrent == true).ToList();
            for (int i = 0; i < active.Count; i++)
            {
                active.ElementAt(i).IsCurrent = false;
                _unitOfWork.EventRepository.Update(active.ElementAt(i));
            }
            _unitOfWork.Save();
            try
            {
                jsonEvent.IsCurrent = true;
                //  && _unitOfWork.EventRepository.Get(filter: p => p.IsLocked == false).FirstOrDefault() == null sollte nur ein mögliches Event geben TESTZWECK
                if (jsonEvent != null)
                {
                    // Saving Areas and Locations for the Event
                    foreach (Area area in jsonEvent.Areas)
                    {
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
        [HttpGet("current")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetCurrentEvent()
        {
            return new OkObjectResult(_unitOfWork.EventRepository.Get(p => p.IsCurrent == true, includeProperties: "Areas").FirstOrDefault());
        }

        /*// <response code="200">Return current Event</response>
        /// <summary>
        /// Getting all Events from Database
        /// </summary>
        [HttpGet("next")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetNextEvent()
        {
            return new OkObjectResult(_unitOfWork.EventRepository.Get(p => p.EventDate.C, includeProperties: "Area").FirstOrDefault());
        }*/


        [HttpGet("latest")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetLatestEvent()
        {
            Event e = _unitOfWork.EventRepository.Get(orderBy: c => c.OrderByDescending(t => t.EventDate)).First();
            return new OkObjectResult(e);
        }

    }
}

using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Entities;

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
        public IActionResult Create([FromBody] Core.Entities.Event temp)
        {
            System.Console.WriteLine(temp.EventDate);
            try
            {
                if (temp != null)
                {
                    _unitOfWork.EventRepository.Insert(temp);
                    _unitOfWork.Save();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        /// <response code="200">Return current Event</response>
        /// <summary>
        /// Getting all Events from Database
        /// </summary>
        [HttpGet("current")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetCurrentEvent()
        {
            Event e = new Event();
            e.IsLocked = false;
            e.RegistrationEnd = DateTime.Now.AddDays(30);
            e.RegistrationStart = DateTime.Now.AddDays(-1);
            e.Areas.AddRange(_unitOfWork.AreaRepository.Get().ToList());
            return new ObjectResult(e);
        }
    }
}

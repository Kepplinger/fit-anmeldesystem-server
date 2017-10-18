using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]

    public class EventController
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
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Core.Entities.Event), 200)]
        [ProducesResponseType(typeof(void), 101)]
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

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        /// <response code="200">Returns all available Events</response>
        /// <summary>
        /// Getting all Events from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Core.Entities.Event), 200)]
        public IActionResult GetAll()
        {
            var events = _unitOfWork.EventRepository.Get();
            return new ObjectResult(events);
        }
    }
}

using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
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
        public IActionResult Create([FromBody] Event temp)
        {
            try
            {
                if (temp != null && _unitOfWork.EventRepository.Get(filter: p => p.IsLocked == false).FirstOrDefault()==null)
                {
                    _unitOfWork.EventRepository.Insert(temp);
                    _unitOfWork.Save();
                    return new OkObjectResult(temp);
                }
                return new BadRequestObjectResult(temp);
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
            /*
              return ner OkObjectResult(_unitOfWork.EventRepository.Get(p => p.IsLocked == false).FirstOrDefault());
            */

            Event e = new Event();
            e.IsLocked = false;
            e.RegistrationEnd = DateTime.Now.AddDays(30);
            e.RegistrationStart = DateTime.Now.AddDays(-1);
            e.Areas.AddRange(_unitOfWork.AreaRepository.Get().ToList());
            return new OkObjectResult(e);
        }


        [HttpGet("latest")]
        [ProducesResponseType(typeof(StatusCodes), StatusCodes.Status200OK)]
        public IActionResult GetLatestEvent()
        {
            Event e = _unitOfWork.EventRepository.Get(orderBy: c => c.OrderByDescending( t => t.EventDate)).First();
            return new OkObjectResult(e);
        }

    }
}

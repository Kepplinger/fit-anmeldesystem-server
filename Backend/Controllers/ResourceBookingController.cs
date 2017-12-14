using Backend.Core.Contracts;
using Backend.Core.Entities;
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
    [Produces("application/json", "application/xml")]
    public class ResourceBookingController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ResourceBookingController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a ResourceBooking Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(ResourceBooking), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] ResourceBooking temp)
        {
            System.Console.WriteLine(temp.Resource.Description);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.ResourceBookingRepository.Insert(temp);
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

        /// <response code="200">Returns all available ResourceBookings</response>
        /// <summary>
        /// Getting all ResourceBookings from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResourceBooking), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var resourceBookings = _unitOfWork.ResourceBookingRepository.Get();
            return new OkObjectResult(resourceBookings);
        }
    }
}

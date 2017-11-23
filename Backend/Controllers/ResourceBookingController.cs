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

    public class ResourceBookingController
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
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(ResourceBooking), 200)]
        [ProducesResponseType(typeof(void), 101)]
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
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        /// <response code="200">Returns all available ResourceBookings</response>
        /// <summary>
        /// Getting all ResourceBookings from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(ResourceBooking), 200)]
        public IActionResult GetAll()
        {
            var resourceBookings = _unitOfWork.ResourceBookingRepository.Get();
            return new ObjectResult(resourceBookings);
        }
    }
}

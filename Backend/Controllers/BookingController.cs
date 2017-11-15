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

    public class BookingController
    {
        private IUnitOfWork _unitOfWork;

        public BookingController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Booking Object
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Booking), 200)]
        public IActionResult Create([FromBody] Booking temp)
        {
            System.Console.WriteLine(temp.Company);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.BookingRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

       /// <summary>
        /// Returns all saved Bookings
        /// </summary>
        /// <response code="200">Returns all available Bookings</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IActionResult), 200)]

        public IActionResult GetAll()
        {
            var bookings = _unitOfWork.BookingRepository.Get();
            return new ObjectResult(bookings);
        }
    }
}

using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create([FromBody] Booking temp)
        {
            try
            {
                if (temp != null && temp.Company.Id != 0)
                {
                    _unitOfWork.CompanyRepository.Get();
                    /*if (toUpdate.FK_Address != 0 && toUpdate.Address != null)
                    {
                        _unitOfWork.AddressRepository.Update(toUpdate.Address);
                        _unitOfWork.Save();
                    }

                    else if (toUpdate.FK_Address == 0) 
                    {
                        _unitOfWork.AddressRepository.Insert(toUpdate.Address);
                        _unitOfWork.Save();
                        toUpdate.FK_Address = toUpdate.Address.Id;
                    }

                    if (toUpdate.FK_Contact != 0 && toUpdate.Contact != null)
                    {
                        _unitOfWork.ContactRepository.Update(toUpdate.Contact);
                        _unitOfWork.Save();
                    }
                    else if (toUpdate.FK_Contact == 0)
                    {
                        _unitOfWork.ContactRepository.Insert(toUpdate.Contact);
                        _unitOfWork.Save();
                        toUpdate.FK_Contact = toUpdate.Contact.Id;
                    }*/
                    _unitOfWork.CompanyRepository.Update(temp.Company);
                    _unitOfWork.Save();
                }
                else if(temp != null && temp.Company.Id == 0) {
                    
                }
                return new StatusCodeResult(StatusCodes.Status200OK);
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
        [HttpGet("")]
        [ProducesResponseType(typeof(IActionResult), 200)]
        public IActionResult GetAll()
        {
            var bookings = _unitOfWork.BookingRepository.Get(includeProperties: "Event,Branches,Company,Package,Location,Presentation",includeLevelTwoProps: "Company:Address");
            return new ObjectResult(bookings);
        }

        /// <response code="200">Returns the available bookings with the </response>
        /// <summary>
        /// Getting all bookings from Database
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Booking), 200)]
        public IActionResult GetById(int id)
        {
            var bookings = _unitOfWork.BookingRepository.GetById(id);
            return new ObjectResult(bookings);
        }
    }
}

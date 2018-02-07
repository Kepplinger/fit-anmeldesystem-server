using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;
using Backend.Utils;
using System.Net.Mail;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class BookingController : Controller
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
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [Consumes("application/json")]
        public IActionResult Create([FromBody] Booking jsonBooking)
        {

            if (jsonBooking != null && jsonBooking.Company.Id != 0)
                Update(jsonBooking);
            else if (jsonBooking != null && jsonBooking.Company.Id == 0)
                return Insert(jsonBooking);

            Console.WriteLine("Bad Request 400: Possible Problem Json Serialization: " + jsonBooking.ToString());
            return new BadRequestObjectResult(jsonBooking);
        }

        [NonAction]
        public IActionResult Update(Booking jsonBooking) {

            using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    // Update already persistent Entities ------------------
                    Company toUpdate = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == jsonBooking.Company.Id).FirstOrDefault();

                    if (toUpdate.Address != null && toUpdate.FK_Address != 0)
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
                    }

                    _unitOfWork.CompanyRepository.Update(jsonBooking.Company);
                    _unitOfWork.Save();

                    transaction.Commit();

                    return new OkObjectResult(jsonBooking);
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();

                    String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                    System.Console.WriteLine(error);

                    return new BadRequestObjectResult(error);
                }
            }
        }

        [NonAction]
        public IActionResult Insert(Booking jsonBooking) {
            using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    // Insert new Company and Booking ----------------------
                    _unitOfWork.AddressRepository.Insert(jsonBooking.Company.Address);
                    _unitOfWork.Save();

                    _unitOfWork.ContactRepository.Insert(jsonBooking.Company.Contact);
                    _unitOfWork.Save();

                    Company c = jsonBooking.Company;
                    c.RegistrationToken = Guid.NewGuid().ToString();
                    _unitOfWork.CompanyRepository.Insert(c);    
                    _unitOfWork.Save();

                    _unitOfWork.RepresentativeRepository.InsertMany(jsonBooking.Representatives);
                    _unitOfWork.Save();

                    // Get the entity from the DB and give reference to it
                   // jsonBooking.Location.Area = _unitOfWork.AreaRepository.Get(filter: p => p.Id == jsonBooking.Location.Area.Id).FirstOrDefault();
                    _unitOfWork.LocationRepository.Insert(jsonBooking.Location);
                    _unitOfWork.Save();

                    jsonBooking.FitPackage = _unitOfWork.PackageRepository.Get(filter: p => p.Id == jsonBooking.FitPackage.Id).FirstOrDefault();
                    _unitOfWork.Save();


                    // Fill up the list
                    List<Branch> branchjsonBooking = new List<Branch>();
                    for (int i = 0; i < jsonBooking.Branches.Count(); i++)
                    {
                        branchjsonBooking.Add(_unitOfWork.BranchRepository.Get(filter: p => p.Id == jsonBooking.Branches.ElementAt(i).Id).FirstOrDefault());
                    }
                    jsonBooking.Branches = branchjsonBooking;
                    _unitOfWork.Save();

                    List<Resource> resourcejsonBooking = new List<Resource>();
                    for (int i = 0; i < jsonBooking.Resources.Count(); i++)
                    {
                        resourcejsonBooking.Add(_unitOfWork.ResourceRepository.Get(filter: p => p.Id == jsonBooking.Resources.ElementAt(i).Id).FirstOrDefault());
                    }
                    jsonBooking.Resources = resourcejsonBooking;
                    _unitOfWork.Save();


                    // Get the current active Event (nimmt an das es nur 1 gibt)
                    if (_unitOfWork.EventRepository.Get(filter: ev => ev.IsLocked == false).FirstOrDefault() != null && _unitOfWork.EventRepository.Get(filter: ev => ev.Id == jsonBooking.Event.Id).FirstOrDefault() != null)
                    {
                        jsonBooking.Event = _unitOfWork.EventRepository.Get(filter: ev => ev.Id == jsonBooking.Event.Id).FirstOrDefault();
                        _unitOfWork.Save();
                    }
                    else
                    {
                        //falls es kein aktives gibt transaction rollbackn
                        transaction.Rollback();
                    }

                    // Finales Inserten des Booking Repositorys
                    _unitOfWork.BookingRepository.Insert(jsonBooking);
                    _unitOfWork.Save();
                    transaction.Commit();
                    _unitOfWork.Dispose();

                    //Senden der Bestätigungs E-Mail
                    EmailHelper.SendBookingAcceptedMail(jsonBooking);

                    return new OkObjectResult(jsonBooking);

                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();

                    String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                    System.Console.WriteLine(error);

                    return new BadRequestObjectResult(error);
                }
            }
        }

        /// <summary>
        /// Returns all saved Bookings
        /// </summary>
        /// <response code="200">Returns all available Bookings</response>
        [HttpGet]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var bookings = _unitOfWork.BookingRepository.Get(includeProperties: "Event,Branches,Company,Package,Location,Presentation");
            return new ObjectResult(bookings);
        }

        /// <response code="200">Returning Booking by id</response>
        /// <summary>
        /// Getting a booking by the id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetById(int id)
        {
            var bookings = _unitOfWork.BookingRepository.GetById(id);
            return new ObjectResult(bookings);
        }

        /// <response code="200">Returns the available bookings by company id</response>
        /// <summary>
        /// Getting all bookings by company id
        /// </summary>
        [HttpGet("getBookingByCompanyId/{id}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetbookingByCompanyId(int id)
        {
            var bookings = _unitOfWork.BookingRepository.Get(p => p.Company.Id == id);
            return new ObjectResult(bookings);
        }
    }
}

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
using System.Diagnostics.Contracts;

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
                this.Update(jsonBooking);
            else if (jsonBooking != null && jsonBooking.Company.Id == 0)
                return this.Insert(jsonBooking);

            Console.WriteLine("Bad Request 400: Possible Problem Json Serialization: " + jsonBooking.ToString());
            return new BadRequestObjectResult(jsonBooking);
        }

        [NonAction]
        public IActionResult Update(Booking jsonBooking)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);

            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction())
            {
                ChangeProtocol change = new ChangeProtocol();
                try
                {
                    // Update already persistent Entities ----------------------
                    Company toUpdate = this._unitOfWork.CompanyRepository.Get(filter: p => p.Id == jsonBooking.Company.Id).FirstOrDefault();

                    if (toUpdate.FK_Address != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Address).GetProperties())
                        {
                            if (!p.Name.ToLower().Contains("id") && p.GetValue(jsonBooking.Company.Address).Equals(p.GetValue(toUpdate.Address)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.Company.Address).ToString();
                                change.OldValue = p.GetValue(toUpdate).ToString();
                                change.TableName = nameof(Address);
                                //change.TypeOfValue = p.PropertyType;
                                Console.WriteLine("No Update for" + change.ColumName);
                            }
                        }
                    }

                    if (toUpdate.FK_Contact != 0 && toUpdate.Contact != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Contact).GetProperties())
                        {
                            if (!p.Name.ToLower().Contains("id") && p.GetValue(jsonBooking.Company.Contact).Equals(p.GetValue(toUpdate.Contact)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.Company.Contact).ToString();
                                change.OldValue = p.GetValue(toUpdate).ToString();
                                change.TableName = nameof(Contact);
                                //change.TypeOfValue = p.PropertyType;
                                Console.WriteLine("No Update for" + change.ColumName);
                            }
                        }
                    }
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
        public IActionResult Insert(Booking jsonBooking)
        {

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
                    //jsonBooking.Location = _unitOfWork.LocationRepository.Get(filter: p => p.Id == jsonBooking.Location.Id).FirstOrDefault();
                    jsonBooking.Location = _unitOfWork.LocationRepository.Get(filter: p => p.Id == jsonBooking.Location.Id).FirstOrDefault();
                    //_unitOfWork.Save();

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
                    if (_unitOfWork.EventRepository.Get(filter: ev => ev.IsCurrent == true).FirstOrDefault() != null)
                    {
                        jsonBooking.Event = _unitOfWork.EventRepository.Get(filter: ev => ev.Id == jsonBooking.Event.Id).FirstOrDefault();
                        _unitOfWork.Save();
                        jsonBooking.CreationDate = DateTime.Now;
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
            return new OkObjectResult(bookings);
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

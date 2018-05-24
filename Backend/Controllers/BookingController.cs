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
using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    //[Authorize(Roles = "Admin")]
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
            if (jsonBooking != null && this._unitOfWork.BookingRepository.Get(filter: c => c.Id == jsonBooking.Id).FirstOrDefault() != null)
            {
                return this.Update(jsonBooking);
            }
            else if (jsonBooking != null)
            {
                return this.Insert(jsonBooking);
            }
            else
            {
                return new BadRequestObjectResult(jsonBooking);
            }

        }

        [HttpPut]
        [Consumes("application/json")]
        public IActionResult Update(Booking jsonBooking)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);

            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction())
            {
                try
                {
                    //ManageChanges(jsonBooking);

                    if (jsonBooking.Presentation != null)
                    {
                        if (jsonBooking.Presentation.File != null)
                        {
                            if (jsonBooking.Presentation.File.Id != null)
                                _unitOfWork.DataFileRepository.Update(jsonBooking.Presentation.File);
                            else
                                _unitOfWork.DataFileRepository.Insert(jsonBooking.Presentation.File);
                        }

                        if (jsonBooking.Presentation.Id != null)
                            _unitOfWork.PresentationRepository.Update(jsonBooking.Presentation);
                        else
                            _unitOfWork.PresentationRepository.Insert(jsonBooking.Presentation);

                        _unitOfWork.Save();
                    }

                    ImageHelper.ManageBookingImages(jsonBooking);

                    if (jsonBooking.Logo != null)
                    {
                        if (jsonBooking.Logo.Id != null)
                            _unitOfWork.DataFileRepository.Update(jsonBooking.Logo);
                        else
                            _unitOfWork.DataFileRepository.Insert(jsonBooking.Logo);
                    }

                    foreach (Representative representative in jsonBooking.Representatives)
                    {
                        _unitOfWork.DataFileRepository.Update(representative.Image);
                        _unitOfWork.RepresentativeRepository.Update(representative);
                    }

                    _unitOfWork.ContactRepository.Update(jsonBooking.Contact);
                    _unitOfWork.BookingRepository.Update(jsonBooking);
                    _unitOfWork.Save();
                    transaction.Commit();

                    return new OkObjectResult(jsonBooking);
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    return DbErrorHelper.CatchDbError(ex);
                }
            }
        }

        // auslagern
        private void ManageChanges(Booking booking)
        {
            ChangeProtocol change = new ChangeProtocol();

            // Update already persistent Entities ----------------------
            Booking bookingToUpdate = this._unitOfWork.BookingRepository.Get(filter: c => c.Id == booking.Id).FirstOrDefault();
            change = new ChangeProtocol();
            if (booking.fk_FitPackage != 0 && bookingToUpdate.FitPackage != null)
            {
                //foreach (System.Reflection.PropertyInfo p in typeof(FitPackage).GetProperties())
                //{
                //    if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk")
                //        && p.GetValue(jsonBooking.FitPackage) != null && !p.GetValue(jsonBooking.FitPackage).Equals(p.GetValue(bookingToUpdate.FitPackage)))
                //    {
                //        change.ChangeDate = DateTime.Now;
                //        change.ColumnName = p.Name;
                //        change.NewValue = p.GetValue(jsonBooking.FitPackage).ToString();
                //        change.OldValue = p.GetValue(bookingToUpdate.FitPackage).ToString();
                //        change.TableName = nameof(FitPackage);
                //        change.IsPending = true;
                //        change.CompanyId = jsonBooking.fk_Company;
                //        change.isAdminChange = false;
                //        change.isReverted = false;
                //        _unitOfWork.ChangeRepository.Insert(change);
                //        _unitOfWork.Save();

                //        Console.WriteLine("Updated: " + change.ColumnName);
                //        change = new ChangeProtocol();
                //    }
                //}
                //_unitOfWork.PackageRepository.Update(jsonBooking.FitPackage);
                //_unitOfWork.Save();
            }

            change = new ChangeProtocol();
            if (booking.fk_Presentation != 0 && bookingToUpdate.Presentation != null)
            {
                foreach (System.Reflection.PropertyInfo p in typeof(Presentation).GetProperties())
                {
                    if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk")
                        && p.GetValue(booking.Presentation) != null && !p.GetValue(booking.Presentation).Equals(p.GetValue(bookingToUpdate.Presentation)))
                    {
                        change.ChangeDate = DateTime.Now;
                        change.ColumnName = p.Name;
                        change.NewValue = p.GetValue(booking.Presentation).ToString();
                        change.OldValue = p.GetValue(bookingToUpdate.Presentation).ToString();
                        change.TableName = nameof(Presentation);
                        change.IsPending = true;
                        change.CompanyId = booking.fk_Company;
                        change.isAdminChange = false;
                        change.isReverted = false;
                        _unitOfWork.ChangeRepository.Insert(change);
                        _unitOfWork.Save();

                        Console.WriteLine("Updated: " + change.ColumnName);
                        change = new ChangeProtocol();
                    }
                }
            }

            change = new ChangeProtocol();
            if (booking.Id != 0 && bookingToUpdate != null)
            {
                foreach (System.Reflection.PropertyInfo p in typeof(Booking).GetProperties())
                {
                    if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk")
                        && p.GetValue(booking) != null && !p.GetValue(booking).Equals(p.GetValue(bookingToUpdate)))
                    {
                        change.ChangeDate = DateTime.Now;
                        change.ColumnName = p.Name;
                        change.NewValue = Convert.ToString(p.GetValue(booking));
                        change.OldValue = Convert.ToString(p.GetValue(bookingToUpdate));
                        change.TableName = nameof(Booking);
                        change.IsPending = true;
                        change.CompanyId = booking.fk_Company;
                        change.isAdminChange = false;
                        change.isReverted = false;
                        _unitOfWork.ChangeRepository.Insert(change);
                        _unitOfWork.Save();

                        Console.WriteLine("Updated: " + change.ColumnName);
                        change = new ChangeProtocol();
                    }

                }
            }
        }

        [NonAction]
        private IActionResult Insert(Booking jsonBooking)
        {
            using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    // PRESENTATION
                    if (jsonBooking.Presentation != null)
                    {
                        foreach (PresentationBranches item in jsonBooking.Presentation.Branches)
                        {
                            _unitOfWork.PresentationBranchesRepository.Insert(item);
                        }

                        _unitOfWork.Save();
                        _unitOfWork.PresentationRepository.Insert(jsonBooking.Presentation);
                        _unitOfWork.Save();

                        foreach (PresentationBranches item in jsonBooking.Presentation.Branches)
                        {
                            item.fk_Presentation = jsonBooking.Presentation.Id;
                            _unitOfWork.PresentationBranchesRepository.Update(item);
                        }
                        _unitOfWork.Save();
                    }

                    // REPRESENTATIVES
                    for (int i = 0; i < jsonBooking.Representatives.Count; i++)
                    {
                        if (jsonBooking.Representatives.ElementAt(i).Id > 0)
                            _unitOfWork.RepresentativeRepository.Update(jsonBooking.Representatives.ElementAt(i));
                        else
                            _unitOfWork.RepresentativeRepository.Insert(jsonBooking.Representatives.ElementAt(i));
                        _unitOfWork.Save();
                    }

                    // IMAGES 
                    ImageHelper.ManageBookingImages(jsonBooking);

                    // PACKAGE
                    jsonBooking.FitPackage = _unitOfWork.PackageRepository.Get(filter: p => p.Id == jsonBooking.fk_FitPackage).FirstOrDefault();
                    _unitOfWork.Save();

                    // EVENT
                    // Get the current active Event (nimmt an das es nur 1 gibt)
                    if (_unitOfWork.EventRepository.Get(filter: ev => ev.RegistrationState.IsCurrent == true).FirstOrDefault() != null)
                    {
                        jsonBooking.Event = _unitOfWork.EventRepository.Get(filter: ev => ev.Id == jsonBooking.fk_Event).FirstOrDefault();
                        _unitOfWork.Save();
                        jsonBooking.CreationDate = DateTime.Now;
                    }
                    else
                    {
                        // make rollback if there are no active transactions
                        transaction.Rollback();
                    }

                    // BRANCHES
                    List<BookingBranches> bbranches = new List<BookingBranches>();
                    foreach (BookingBranches item in jsonBooking.Branches)
                    {
                        BookingBranches br = new BookingBranches();
                        br.fk_Branch = item.fk_Branch;
                        bbranches.Add(br);
                    }
                    jsonBooking.Branches = null;

                    // RESOURCES
                    List<ResourceBooking> rbooking = new List<ResourceBooking>();
                    foreach (ResourceBooking item in jsonBooking.Resources)
                    {
                        ResourceBooking bk = new ResourceBooking();
                        bk.fk_Resource = item.fk_Resource;
                        rbooking.Add(bk);
                    }
                    jsonBooking.Resources = null;

                    _unitOfWork.BookingRepository.Insert(jsonBooking);
                    _unitOfWork.Save();

                    // BRANCHES
                    foreach (BookingBranches item in bbranches)
                    {
                        item.fk_Booking = jsonBooking.Id;
                        item.Branch = _unitOfWork.BranchRepository.GetById(item.fk_Branch);
                        _unitOfWork.BookingBranchesRepository.Insert(item);
                        _unitOfWork.Save();
                    }

                    // RESOURCES
                    foreach (ResourceBooking item in rbooking)
                    {
                        item.fk_Booking = jsonBooking.Id;
                        _unitOfWork.ResourceBookingRepository.Insert(item);
                        _unitOfWork.Save();
                    }

                    jsonBooking.Resources = rbooking;
                    jsonBooking.Branches = bbranches;
                    _unitOfWork.Save(); // bringt des wos?

                    // LOCATION
                    if (jsonBooking.Location != null)
                    {
                        jsonBooking.Location.isOccupied = true;
                        _unitOfWork.LocationRepository.Update(jsonBooking.Location);
                        _unitOfWork.Save();
                    }

                    // COMPANY
                    if (jsonBooking.Company == null)
                    {
                        jsonBooking.Company = _unitOfWork.CompanyRepository.Get(p => p.Id == jsonBooking.fk_Company).FirstOrDefault();
                        if (jsonBooking.Company.Address == null)
                        {
                            jsonBooking.Company.Address = _unitOfWork.AddressRepository.Get(p => p.Id == jsonBooking.Company.fk_Address).FirstOrDefault();
                        }
                        if (jsonBooking.Company.Contact == null)
                        {
                            jsonBooking.Company.Contact = _unitOfWork.ContactRepository.Get(p => p.Id == jsonBooking.Company.fk_Contact).FirstOrDefault();
                        }
                    }

                    transaction.Commit();
                    _unitOfWork.Dispose();

                    //Senden der Bestätigungs E-Mail
                    DocumentBuilder doc = new DocumentBuilder();
                    doc.CreatePdfOfBooking(jsonBooking);
                    EmailHelper.SendMailByName("SendBookingAcceptedMail", jsonBooking, jsonBooking.Contact.Email);

                    return new OkObjectResult(jsonBooking);
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    return DbErrorHelper.CatchDbError(ex);
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
            List<Booking> bookings = _unitOfWork.BookingRepository.Get(includeProperties: "Event,Branches,Company,Package,Location,Presentation,Contact").ToList();
            if (bookings != null && bookings.Count > 0)
                return new OkObjectResult(bookings);
            else
                return new NoContentResult();
        }

        /// <response code="200">Returning Booking by id</response>
        /// <summary>
        /// Getting a booking by the id
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetById(int id)
        {
            Booking booking = _unitOfWork.BookingRepository.GetById(id);
            if (booking != null)
            {
                return new OkObjectResult(booking);
            }
            return new NoContentResult();
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

        /// <response code="200">Returns the available bookings by event id</response>
        /// <summary>
        /// Getting all bookings by event id
        /// </summary>
        [HttpGet("event/{id}")]
        [Authorize(ActiveAuthenticationSchemes = "Bearer", Policy = "IdentityUser")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetBookingByEventId(int id)
        {
            List<Booking> bookings = _unitOfWork.BookingRepository.Get(p => p.Event.Id == id).ToList();
            if (bookings != null && bookings.Count > 0)
            {
                return new ObjectResult(bookings);
            }
            return new NoContentResult();
        }
    }
}

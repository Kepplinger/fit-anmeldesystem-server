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
using Backend.Src.Persistence.Facades;
using Backend.Src.Utils;
using System.Security.Claims;

namespace Backend.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class BookingController : Controller {

        private IUnitOfWork _unitOfWork;
        private BookingFacade _bookingFacade;

        public BookingController(IUnitOfWork uow) {
            _unitOfWork = uow;
            _bookingFacade = new BookingFacade(_unitOfWork);
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
        [Authorize(Policy = "MemberAndWriteableAdmins")]
        public IActionResult Create([FromBody] Booking jsonBooking) {
            Booking booking = this._unitOfWork.BookingRepository.Get(filter: c => c.Id == jsonBooking.Id).FirstOrDefault();
            bool isAdminChange = UserClaimsHelper.IsUserAdmin(User.Identity as ClaimsIdentity);

            if (jsonBooking != null && booking != null) {
                return new OkObjectResult(_bookingFacade.Update(jsonBooking, true, isAdminChange));
            } else if (jsonBooking != null) {
                return this.Insert(jsonBooking);
            } else {
                return new BadRequestObjectResult(jsonBooking);
            }
        }

        /// <response code="200">Returns the available bookings by event id</response>
        /// <summary>
        /// Getting all bookings by event id
        /// </summary>
        [HttpGet("event/{id}")]
        [Authorize(Policy = "FitAdmin")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetBookingByEventId(int id) {
            List<Booking> bookings = _unitOfWork.BookingRepository.Get(p => p.Event.Id == id).ToList();
            if (bookings != null && bookings.Count > 0) {
                return new ObjectResult(bookings);
            } else {
                return new ObjectResult(new List<Booking>());
            }
        }

        /// <summary>
        /// Accepts or rejects target booking.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut("accept/{id}")]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult AcceptBooking(int id, [FromBody] int status) {
            Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Id == id).FirstOrDefault();
            if (booking != null) {
                booking.isAccepted = status;

                if (status == -1) {
                    if (booking.Location != null) {
                        booking.Location.isOccupied = false;
                        booking.fk_Location = null;
                        _unitOfWork.LocationRepository.Update(booking.Location);
                    }

                    if (booking.Presentation != null) {
                        booking.Presentation.IsAccepted = -1;
                        _unitOfWork.PresentationRepository.Update(booking.Presentation);
                    }
                }
                
                _unitOfWork.BookingRepository.Update(booking);
                _unitOfWork.Save();

                if (booking.isAccepted == 1) {
                    EmailHelper.SendMailByIdentifier("BA", booking, booking.Contact.Email, _unitOfWork);
                    if (EmailHelper.HasPendingData(booking)) {
                        EmailHelper.SendMailByIdentifier("DR", booking, booking.Contact.Email, _unitOfWork);
                    }
                } else if (booking.isAccepted == -1) {
                    EmailHelper.SendMailByIdentifier("BR", booking, booking.Contact.Email, _unitOfWork);
                }

                return new ObjectResult(booking);
            } else {
                return new BadRequestResult();
            }
        }

        [NonAction]
        private IActionResult Insert(Booking jsonBooking) {
            using (IDbContextTransaction transaction = _unitOfWork.BeginTransaction()) {
                try {
                    // PRESENTATION
                    if (jsonBooking.Presentation != null) {
                        foreach (PresentationBranch item in jsonBooking.Presentation.Branches) {
                            _unitOfWork.PresentationBranchesRepository.Insert(item);
                        }

                        _unitOfWork.Save();
                        _unitOfWork.PresentationRepository.Insert(jsonBooking.Presentation);
                        _unitOfWork.Save();

                        foreach (PresentationBranch item in jsonBooking.Presentation.Branches) {
                            item.fk_Presentation = jsonBooking.Presentation.Id;
                            _unitOfWork.PresentationBranchesRepository.Update(item);
                        }
                        _unitOfWork.Save();
                    }

                    // REPRESENTATIVES
                    for (int i = 0; i < jsonBooking.Representatives.Count; i++) {
                        if (jsonBooking.Representatives.ElementAt(i).Id > 0)
                            _unitOfWork.RepresentativeRepository.Update(jsonBooking.Representatives.ElementAt(i));
                        else
                            _unitOfWork.RepresentativeRepository.Insert(jsonBooking.Representatives.ElementAt(i));
                        _unitOfWork.Save();
                    }

                    // IMAGES 
                    ImageHelper.ManageBookingFiles(jsonBooking);

                    // PACKAGE
                    jsonBooking.FitPackage = _unitOfWork.PackageRepository.Get(filter: p => p.Id == jsonBooking.fk_FitPackage).FirstOrDefault();
                    _unitOfWork.Save();

                    // EVENT
                    // Get the current active Event (nimmt an das es nur 1 gibt)
                    if (_unitOfWork.EventRepository.Get(filter: ev => ev.RegistrationState.IsCurrent == true).FirstOrDefault() != null) {
                        jsonBooking.Event = _unitOfWork.EventRepository.Get(filter: ev => ev.Id == jsonBooking.fk_Event).FirstOrDefault();
                        _unitOfWork.Save();
                        jsonBooking.CreationDate = DateTime.Now;
                    } else {
                        // make rollback if there are no active transactions
                        transaction.Rollback();
                    }

                    // BRANCHES
                    List<BookingBranch> bbranches = new List<BookingBranch>();
                    foreach (BookingBranch item in jsonBooking.Branches) {
                        BookingBranch br = new BookingBranch();
                        br.fk_Branch = item.fk_Branch;
                        bbranches.Add(br);
                    }
                    jsonBooking.Branches = null;

                    // RESOURCES
                    List<ResourceBooking> rbooking = new List<ResourceBooking>();
                    foreach (ResourceBooking item in jsonBooking.Resources) {
                        ResourceBooking bk = new ResourceBooking();
                        bk.fk_Resource = item.fk_Resource;
                        rbooking.Add(bk);
                    }
                    jsonBooking.Resources = null;

                    _unitOfWork.BookingRepository.Insert(jsonBooking);
                    _unitOfWork.Save();

                    // BRANCHES
                    foreach (BookingBranch item in bbranches) {
                        item.fk_Booking = jsonBooking.Id;
                        item.Branch = _unitOfWork.BranchRepository.GetById(item.fk_Branch);
                        _unitOfWork.BookingBranchesRepository.Insert(item);
                        _unitOfWork.Save();
                    }

                    // RESOURCES
                    foreach (ResourceBooking item in rbooking) {
                        item.fk_Booking = jsonBooking.Id;
                        _unitOfWork.ResourceBookingRepository.Insert(item);
                        _unitOfWork.Save();
                    }

                    jsonBooking.Resources = rbooking;
                    jsonBooking.Branches = bbranches;
                    _unitOfWork.Save(); // TODO bringt des wos?

                    // LOCATION
                    if (jsonBooking.Location != null) {
                        jsonBooking.Location.isOccupied = true;
                        _unitOfWork.LocationRepository.Update(jsonBooking.Location);
                        _unitOfWork.Save();
                    }

                    // COMPANY
                    if (jsonBooking.Company == null) {
                        jsonBooking.Company = _unitOfWork.CompanyRepository.Get(p => p.Id == jsonBooking.fk_Company).FirstOrDefault();
                        if (jsonBooking.Company.Address == null) {
                            jsonBooking.Company.Address = _unitOfWork.AddressRepository.Get(p => p.Id == jsonBooking.Company.fk_Address).FirstOrDefault();
                        }
                        if (jsonBooking.Company.Contact == null) {
                            jsonBooking.Company.Contact = _unitOfWork.ContactRepository.Get(p => p.Id == jsonBooking.Company.fk_Contact).FirstOrDefault();
                        }
                    }

                    _unitOfWork.Save();
                    transaction.Commit();

                    foreach (BookingBranch branch in jsonBooking.Branches) {
                        branch.Branch = _unitOfWork.BranchRepository.GetById(branch.fk_Branch);
                    }

                    foreach (ResourceBooking resource in jsonBooking.Resources) {
                        resource.Resource = _unitOfWork.ResourceRepository.GetById(resource.fk_Resource);
                    }

                    //Senden der Bestätigungs E-Mail
                    DocumentBuilder doc = new DocumentBuilder();
                    doc.CreatePdfOfBooking(jsonBooking);
                    EmailHelper.SendMailByIdentifier("SBA", jsonBooking, jsonBooking.Contact.Email, _unitOfWork);

                    _unitOfWork.Dispose();

                    return new OkObjectResult(jsonBooking);
                } catch (DbUpdateException ex) {
                    transaction.Rollback();
                    return DbErrorHelper.CatchDbError(ex);
                }
            }
        }
    }
}

﻿using Backend.Core.Contracts;
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
                this.Update(jsonBooking);
            else if (jsonBooking != null)
                return this.Insert(jsonBooking);

            Console.WriteLine("Bad Request 400: Possible Problem Json Serialization: " + jsonBooking.ToString());
            return new BadRequestObjectResult(jsonBooking);
        }

        [HttpPut]
        [Consumes("application/json")]
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
                    Booking btoUpdate = this._unitOfWork.BookingRepository.Get(filter: c => c.Id == jsonBooking.Id).FirstOrDefault();
                    if (jsonBooking.fk_Company != 0 && toUpdate != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Company).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk") 
                                && p.GetValue(jsonBooking.Company) != null && !p.GetValue(jsonBooking.Company).Equals(p.GetValue(toUpdate)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.Company).ToString();
                                change.OldValue = p.GetValue(toUpdate).ToString();
                                change.TableName = nameof(Company);
                                change.RowId = toUpdate.Address.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.CompanyRepository.Update(jsonBooking.Company);
                        _unitOfWork.Save();
                    }
                    change = new ChangeProtocol();
                    if (jsonBooking.Company.fk_Address != 0 && toUpdate.Address != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Address).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk") 
                                && p.GetValue(jsonBooking.Company.Address) != null && !p.GetValue(jsonBooking.Company.Address).Equals(p.GetValue(toUpdate.Address)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.Company.Address).ToString();
                                change.OldValue = p.GetValue(toUpdate.Address).ToString();
                                change.TableName = nameof(Address);
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.AddressRepository.Update(jsonBooking.Company.Address);
                        _unitOfWork.Save();
                    }


                    change = new ChangeProtocol();
                    if (jsonBooking.Company.fk_Contact != 0 && toUpdate.Contact != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Contact).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk") 
                                && p.GetValue(jsonBooking.Company.Contact) != null && !p.GetValue(jsonBooking.Company.Contact).Equals(p.GetValue(toUpdate.Contact)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.Company.Contact).ToString();
                                change.OldValue = p.GetValue(toUpdate.Contact).ToString();
                                change.TableName = nameof(Contact);
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.ContactRepository.Update(jsonBooking.Company.Contact);
                        _unitOfWork.Save();
                    }
                    change = new ChangeProtocol();
                    if (jsonBooking.fk_FitPackage != 0 && btoUpdate.FitPackage != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(FitPackage).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk")
                                && p.GetValue(jsonBooking.FitPackage) != null && !p.GetValue(jsonBooking.FitPackage).Equals(p.GetValue(btoUpdate.FitPackage)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.FitPackage).ToString();
                                change.OldValue = p.GetValue(btoUpdate.FitPackage).ToString();
                                change.TableName = nameof(FitPackage);
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.PackageRepository.Update(jsonBooking.FitPackage );
                        _unitOfWork.Save();
                    }

                    change = new ChangeProtocol();
                    if (jsonBooking.fk_Presentation != 0 && btoUpdate.Presentation != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Presentation).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk")
                                && p.GetValue(jsonBooking.Presentation) != null && !p.GetValue(jsonBooking.Presentation).Equals(p.GetValue(btoUpdate.Presentation)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonBooking.Company.Contact).ToString();
                                change.OldValue = p.GetValue(toUpdate).ToString();
                                change.TableName = nameof(Presentation);
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.PresentationRepository.Update(jsonBooking.Presentation);
                        _unitOfWork.Save();
                    }

                    change = new ChangeProtocol();
                    if (jsonBooking.Id != 0 &&  btoUpdate != null)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Booking).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk")
                                && p.GetValue(jsonBooking) != null && !p.GetValue(jsonBooking).Equals(p.GetValue(btoUpdate)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = Convert.ToString(p.GetValue(jsonBooking));
                                change.OldValue = Convert.ToString(p.GetValue(btoUpdate));
                                change.TableName = nameof(Booking);
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                           
                        }
                        _unitOfWork.BookingRepository.Update(jsonBooking);
                        _unitOfWork.Save();

                    }
                    _unitOfWork.Save();
                    change = new ChangeProtocol();

                    transaction.Commit();

                    return new OkObjectResult(jsonBooking);
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    if (ex.InnerException != null)
                    {
                        String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                        System.Console.WriteLine(error);
                        return new BadRequestObjectResult(error);
                    }
                    else
                    {
                        String error = "*********************\n\nDbUpdateException Message: " + ex.Message;
                        System.Console.WriteLine(error);
                        return new BadRequestObjectResult(error);
                    }
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
                    

                    for (int i = 0; i < jsonBooking.Representatives.Count; i++)
                    {
                        if (jsonBooking.Representatives.ElementAt(i).Id > 0)
                            _unitOfWork.RepresentativeRepository.Update(jsonBooking.Representatives.ElementAt(i));
                        else
                            _unitOfWork.RepresentativeRepository.Insert(jsonBooking.Representatives.ElementAt(i));
                        _unitOfWork.Save();
                    }

                    //Generate Pictures 
                    string compImgPath = new ImageHelper().BookingImages(jsonBooking);
                    jsonBooking.Logo = compImgPath;

                    jsonBooking.FitPackage = _unitOfWork.PackageRepository.Get(filter: p => p.Id == jsonBooking.FitPackage.Id).FirstOrDefault();
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

                    ResourceBooking bk = new ResourceBooking();

                    bk.fk_Resource = jsonBooking.Resources.ElementAt(0).fk_Resource;
                    jsonBooking.Resources = null;
                    _unitOfWork.BookingRepository.Insert(jsonBooking);
                    _unitOfWork.Save();

                    jsonBooking.Resources = new List<ResourceBooking>();
                    bk.fk_Booking = jsonBooking.Id;

                    _unitOfWork.ResourceBookingRepository.Insert(bk);
                    _unitOfWork.Save();

                    jsonBooking.Location.isOccupied = true;
                    _unitOfWork.LocationRepository.Update(jsonBooking.Location);
                    _unitOfWork.Save();

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
                    if (ex.InnerException != null)
                    {
                        String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                        System.Console.WriteLine(error);
                        return new BadRequestObjectResult(error);
                    }
                    else
                    {
                        String error = "*********************\n\nDbUpdateException Message: " + ex.Message;
                        System.Console.WriteLine(error);
                        return new BadRequestObjectResult(error);
                    }
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

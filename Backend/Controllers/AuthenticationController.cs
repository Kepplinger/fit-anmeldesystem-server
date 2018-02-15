﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;
using StoreService.Persistence;
using Backend.Core.Entities;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class AuthenticationController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public AuthenticationController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult CheckIfCompanyExists([FromBody] JToken json)
        {
            Company existing;
            string mail = json["email"].Value<string>();

            using (IUnitOfWork uow = new UnitOfWork())
            {
                existing = uow.CompanyRepository.Get(filter: p => p.Contact.Email.Equals(mail)).FirstOrDefault();

                if (existing == null)
                    existing = uow.CompanyRepository.Get(filter: p => p.FolderInfo.Email.Equals(mail)).FirstOrDefault();
            }
            if (existing != null)
            {
                var a = new
                {
                    existing = true
                };
                return new OkObjectResult(a);
            }
            else
            {
                var a = new
                {
                    existing = false
                };
                return new OkObjectResult(a);
            }

        }

        [HttpPost("mail/code")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult SendCompanyCodeForgotten([FromBody] JToken json)
        {

            string mail = String.Empty;
            try
            {
                mail = json["email"].Value<string>();
            }
            catch (NullReferenceException e)
            {
                var error = new
                {
                    errorMessage = "Es wurde keine E-Mail übermittelt!"
                };
                return new BadRequestObjectResult(error);
            }

            Company company = _unitOfWork.CompanyRepository.Get(filter: p => p.Contact.Email.Equals(mail), includeProperties: "Contact").FirstOrDefault();

            if (company == null)
                company = _unitOfWork.CompanyRepository.Get(filter: p => p.FolderInfo.Email.Equals(mail), includeProperties: "Contact").FirstOrDefault();

            if (company != null)
            {
                EmailHelper.SendForgotten(company);
                return new NoContentResult();
            }
            else
            {
                var error = new
                {
                    errorMessage = "Es gibt kein Unternehmen mit dieser E-Mail!"
                };
                return new BadRequestObjectResult(error);
            }
        }

        [HttpPost("booking/token")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult GetBookingAndCompanyByToken([FromBody] JToken json)
        {
            string token = json["token"].Value<string>();
            Company actCompany = this._unitOfWork.CompanyRepository.Get(filter: g => g.RegistrationToken.Equals(token),includeProperties: "Address,Contact,FolderInfo").FirstOrDefault();

            if (actCompany == null)
            {
                var error = new
                {
                    errorMessage = "Es ist kein Unternehmen mit diesem Token bekannt! Bitte Überprüfen Sie Ihren Token!"
                };
                return new BadRequestObjectResult(error);
            }

            // Get Booking
            Booking lastBooking = this._unitOfWork.BookingRepository.Get(f => f.Company.Id.Equals(actCompany.Id)).OrderByDescending(p => p.CreationDate).FirstOrDefault();

            // If there is no last Booking send just Company
            if (lastBooking == null)
            {
                var companyJson = new
                {
                    company = actCompany
                };
                return new OkObjectResult(companyJson);
            }
            else
            {
                var booking = new
                {
                    booking = lastBooking
                };
                return new OkObjectResult(booking);
            }
        }
    }
}

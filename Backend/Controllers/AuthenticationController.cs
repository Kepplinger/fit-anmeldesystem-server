using System;
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
                {
                    List<Booking> bookings = uow.BookingRepository.Get().ToList();

                    for (int i = 0; i < bookings.Count; i++)
                    {
                        if (bookings.ElementAt(i).Email.Equals(mail))
                        {
                            existing = bookings.ElementAt(i).Company;
                        }
                    }
                }
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
            {
                List<Booking> bookings = _unitOfWork.BookingRepository.Get().ToList();

                for (int i = 0; i < bookings.Count; i++)
                {
                    if (bookings.ElementAt(i).Email.Equals(mail))
                    {
                        company = bookings.ElementAt(i).Company;
                    }
                }
            }

            if (company != null)
            {
                EmailHelper.SendMailByName("SendForgotten", company, company.Contact.Email);
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

        [HttpPost("company/token")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public IActionResult BookingLogin([FromBody] JToken json)
        {
            string token = json["token"].Value<string>();
            Graduate actGraduate = this._unitOfWork.GraduateRepository.Get(g => g.RegistrationToken.Equals(token)).FirstOrDefault();

            if (actGraduate != null)
            {
                var graduateJson = new
                {
                    graduate = actGraduate
                };
                return new OkObjectResult(graduateJson);
            }

            Company actCompany = this._unitOfWork.CompanyRepository.Get(filter: g => g.RegistrationToken.Equals(token),includeProperties: "Address,Contact").FirstOrDefault();

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
                if (lastBooking.Event.IsCurrent)
                {
                    var booking = new
                    {
                        currentBooking = lastBooking
                    };
                    return new OkObjectResult(booking);
                }
                else
                {
                    var booking = new
                    {
                        oldBooking = lastBooking
                    };
                    return new OkObjectResult(booking);
                }
            }
        }
    }
}

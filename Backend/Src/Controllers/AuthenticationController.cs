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
using Backend.Src.Utils;
using Microsoft.AspNetCore.Identity;
using Backend.Core.Entities.UserManagement;
using Backend.Controllers.UserManagement;
using Microsoft.Extensions.Options;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class AuthenticationController : Controller {

        private IUnitOfWork _unitOfWork;
        private readonly UserManager<FitUser> _userManager;
        private readonly IJwtFactory _jwtFactory;

        public AuthenticationController(IUnitOfWork uow,
                                        UserManager<FitUser> userManager,
                                        IJwtFactory jwtFactory) {
            _unitOfWork = uow;
            _userManager = userManager;
            _jwtFactory = jwtFactory;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult CheckIfCompanyExists([FromBody] JToken json) {
            Company existing;
            string mail = json["email"].Value<string>();

            using (IUnitOfWork uow = new UnitOfWork()) {
                existing = uow.CompanyRepository.Get(filter: p => p.Contact.Email.Equals(mail)).FirstOrDefault();

                if (existing == null) {
                    List<Booking> bookings = uow.BookingRepository.Get().ToList();

                    for (int i = 0; i < bookings.Count; i++) {
                        if (bookings.ElementAt(i).Email.Equals(mail)) {
                            existing = bookings.ElementAt(i).Company;
                        }
                    }
                }
            }
            if (existing != null) {
                var a = new {
                    existing = true
                };
                return new OkObjectResult(a);
            } else {
                var a = new {
                    existing = false
                };
                return new OkObjectResult(a);
            }

        }

        [HttpPost("mail/code")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult SendCompanyCodeForgotten([FromBody] JToken json) {

            string mail = String.Empty;
            try {
                mail = json["email"].Value<string>();
            } catch (NullReferenceException e) {
                var error = new {
                    errorMessage = "Es wurde keine E-Mail übermittelt!"
                };
                return new BadRequestObjectResult(error);
            }

            Company company = _unitOfWork.CompanyRepository.Get(filter: p => p.Contact.Email.Equals(mail), includeProperties: "Contact").FirstOrDefault();

            if (company == null) {
                List<Booking> bookings = _unitOfWork.BookingRepository.Get().ToList();

                for (int i = 0; i < bookings.Count; i++) {
                    if (bookings.ElementAt(i).Email.Equals(mail)) {
                        company = bookings.ElementAt(i).Company;
                    }
                }
            }

            if (company != null) {
                EmailHelper.SendMailByIdentifier("SF", company, company.Contact.Email, _unitOfWork);
                return new NoContentResult();
            } else {
                var error = new {
                    errorMessage = "Es gibt kein Unternehmen mit dieser E-Mail!"
                };
                return new BadRequestObjectResult(error);
            }
        }

        [HttpPost("token")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(Booking), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] JToken json) {
            string registrationCode = json["token"].Value<string>();

            var identity = await UserClaimsHelper.GetClaimsIdentity(registrationCode, registrationCode, _jwtFactory, _userManager);

            if (identity == null) {
                return BadRequest(new {
                    errorMessage = "Es ist kein Account mit diesem Token bekannt."
                });
            }

            string authToken = await _jwtFactory.GenerateEncodedToken(registrationCode, identity);

            Graduate actGraduate = this._unitOfWork.GraduateRepository.Get(g => g.RegistrationToken.ToUpper().Equals(registrationCode.ToUpper()), includeProperties: "Address").FirstOrDefault();

            if (actGraduate != null) {
                var graduateJson = new {
                    graduate = actGraduate
                };
                return GetEntityTokenResponse(graduateJson, authToken);
            }

            Company actCompany = this._unitOfWork.CompanyRepository.Get(filter: g => g.RegistrationToken.ToUpper().Equals(registrationCode.ToUpper()), includeProperties: "Address,Contact").FirstOrDefault();

            if (actCompany == null) {
                var error = new {
                    errorMessage = "Es ist kein Unternehmen mit diesem Token bekannt! Bitte Überprüfen Sie Ihren Token!"
                };
                return new BadRequestObjectResult(error);
            }

            // Get Booking
            List<Booking> lastBooking = _unitOfWork.BookingRepository.Get(f => f.Company.Id.Equals(actCompany.Id)).OrderByDescending(p => p.CreationDate).ToList();

            // If there is no last Booking just send Company
            if (lastBooking == null || lastBooking.Count() == 0) {
                var companyJson = new {
                    company = actCompany
                };
                return GetEntityTokenResponse(companyJson, authToken);
            } else {
                if (lastBooking.ElementAt(0).Event.RegistrationState.IsCurrent) {
                    var booking = new {
                        currentBooking = lastBooking.ElementAt(0)
                    };
                    return GetEntityTokenResponse(booking, authToken);
                } else {
                    var booking = new {
                        oldBooking = lastBooking
                    };
                    return GetEntityTokenResponse(booking, authToken);
                }
            }
        }

        private IActionResult GetEntityTokenResponse(object entity, string token) {
            return new OkObjectResult(new {
                authToken = token,
                entity = entity
            });
        }
    }
}

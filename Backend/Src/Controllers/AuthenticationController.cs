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
            string mail = json["email"].Value<string>();
            bool useGraduateMails = json["useGraduateMails"].Value<bool>();

            bool existing = _unitOfWork.CompanyRepository.Get(filter: c => c.Contact.Email.Equals(mail) && c.IsAccepted == 1).Count() >= 1;

            if (!existing) {
                existing = _unitOfWork.BookingRepository.Get(filter: b => b.Contact.Email == mail || b.Email == mail).Count() >= 1;
            }

            if (!existing && useGraduateMails) {
                existing = _unitOfWork.GraduateRepository.Get(filter: g => g.Email == mail).Count() >= 1;
            }

            return new OkObjectResult(new {
                existing = existing
            });

        }

        [HttpPost("mail/code")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult SendCodeForgottenMail([FromBody] JToken json) {
            string mail = json["email"].Value<string>();
            bool useGraduateMails = json["useGraduateMails"].Value<bool>();

            Graduate graduate = null;
            Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Contact.Email.Equals(mail) && c.IsAccepted == 1).FirstOrDefault();

            if (company == null) {
                company = _unitOfWork.BookingRepository.Get(filter: b => b.Contact.Email == mail || b.Email == mail).Select(b => b.Company).FirstOrDefault();
            }

            if (company == null && useGraduateMails) {
                graduate = _unitOfWork.GraduateRepository.Get(filter: g => g.Email == mail).FirstOrDefault();
            }

            if (company != null) {
                EmailHelper.SendMailByIdentifier("SFC", company, company.Contact.Email, _unitOfWork);
                return new NoContentResult();
            } else if (graduate != null && useGraduateMails) {
                EmailHelper.SendMailByIdentifier("SFG", graduate, graduate.Email, _unitOfWork);
                return new NoContentResult();
            } else {
                return new BadRequestObjectResult(new {
                    errorMessage = "Es gibt kein Unternehmen mit dieser E-Mail!"
                });
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
                return new BadRequestObjectResult(new {
                    errorMessage = "Es ist kein Unternehmen mit diesem Token bekannt! Bitte Überprüfen Sie Ihren Token!"
                });
            }

            // Get Booking
            Booking lastBooking = _unitOfWork.BookingRepository.Get(f => f.Company.Id.Equals(actCompany.Id)).OrderByDescending(p => p.CreationDate).FirstOrDefault();

            // If there is no last Booking just send Company
            if (lastBooking == null) {
                var companyJson = new {
                    company = actCompany
                };
                return GetEntityTokenResponse(companyJson, authToken);
            } else {
                if (lastBooking.Event.RegistrationState.IsCurrent) {
                    if (lastBooking.isAccepted >= 0) {
                        var booking = new {
                            currentBooking = lastBooking
                        };
                        return GetEntityTokenResponse(booking, authToken);
                    } else {
                        return new BadRequestObjectResult(new {
                            errorMessage = "Ihre Anmeldung wurde leider bereits abelehnt!"
                        });
                    }
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

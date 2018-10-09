using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;
using StoreService.Persistence;
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Backend.Core;
using Backend.Src.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Backend.Src.Core.DTOs;
using Backend.Src.Core.Mapper;

namespace Backend.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EmailController : Controller {

        private IUnitOfWork _unitOfWork;

        public EmailController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [Authorize(Policy = "WritableAdmin")]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        public IActionResult GetAllEmails() {
            Email test = _unitOfWork.EmailRepository.Get().FirstOrDefault();
            List<Email> emails = _unitOfWork.EmailRepository.Get(includeProperties: "AvailableVariables").ToList();

            if (emails != null && emails.Count > 0) {
                return new OkObjectResult(emails.Select(e => EmailMapper.MapEmailToDto(e)));
            } else {
                return new NoContentResult();
            }
        }

        [HttpPut]
        [Authorize(Policy = "WritableAdmin")]
        public IActionResult UpdateMail([FromBody] EmailDTO email) {
            Email emailEntity = EmailMapper.MapDtoToEmail(email, _unitOfWork);

            if (_unitOfWork.EmailRepository.Get(m => m.Id == emailEntity.Id).FirstOrDefault() != null) {
                _unitOfWork.EmailRepository.Update(emailEntity);
                _unitOfWork.Save();
                return new OkObjectResult(EmailMapper.MapEmailToDto(emailEntity));
            }
            return new BadRequestResult();
        }

        [HttpPost]
        [Route("{identifier}")]
        [Authorize(Policy = "WritableAdmin")]
        public IActionResult SendMails(string identifier, [FromBody] int[] companyIds) {
            Email mail = _unitOfWork.EmailRepository.Get(m => m.Identifier.ToLower().Equals(identifier.ToLower())).FirstOrDefault();

            foreach (int id in companyIds) {
                if (mail.AvailableVariables.Any(v => v.EmailVariable.Entity == nameof(Company))) {
                    Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Id == id).FirstOrDefault();
                    if (company != null) {
                        EmailHelper.SendMailByIdentifier(identifier, company, company.Contact.Email, _unitOfWork);
                    }
                } else {
                    Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Company.Id == id).FirstOrDefault();
                    if (booking != null) {
                        EmailHelper.SendMailByIdentifier(identifier, booking, booking.Contact.Email, _unitOfWork);
                    }
                }
            }

            return new OkResult();
        }

        [HttpPost]
        [Route("custom")]
        [Authorize(Policy = "WritableAdmin")]
        public IActionResult SendCustomMails([FromBody] CustomEmailDTO customEmailBody) {

            Email email = new Email();
            email.Subject = customEmailBody.Subject;
            email.Template = customEmailBody.Body;
            email.Identifier = "C";

            foreach (int id in customEmailBody.CompanyIds) {
                if (customEmailBody.EntityType == nameof(Company)) {
                    Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Id == id).FirstOrDefault();
                    if (company != null) {
                        EmailHelper.SendMail(email, company, company.Contact.Email, _unitOfWork);
                    }
                } else {
                    Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Company.Id == id).FirstOrDefault();
                    if (booking != null) {
                        EmailHelper.SendMail(email, booking, booking.Contact.Email, _unitOfWork);
                    }
                }
            }

            return new OkResult();
        }

        [HttpPost]
        [Route("custom/test")]
        [Authorize(Policy = "WritableAdmin")]
        public IActionResult SendCustomTestMails([FromBody] CustomTestEmailDTO customEmailBody) {
            Email email = new Email();
            email.Subject = customEmailBody.Subject;
            email.Template = customEmailBody.Body;
            email.Identifier = "C";

            if (customEmailBody.EntityType == nameof(Company)) {
                Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Id == customEmailBody.CompanyId).FirstOrDefault();
                if (company != null) {
                    EmailHelper.SendMail(email, company, customEmailBody.Receiver, _unitOfWork);
                }
            } else {
                Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Company.Id == customEmailBody.CompanyId).FirstOrDefault();
                if (booking != null) {
                    EmailHelper.SendMail(email, booking, customEmailBody.Receiver, _unitOfWork);
                }
            }

            return new OkResult();
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = "WritableAdmin")]
        public IActionResult SendTestMail(int id, [FromQuery] string emailAddress, [FromQuery] int entityId, [FromQuery] string entityType) {
            Email email = _unitOfWork.EmailRepository.GetById(id);

            if (entityType.ToLower() == "booking") {
                Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Id == entityId).FirstOrDefault();
                EmailHelper.SendMail(email, booking, emailAddress, _unitOfWork);
            } else if (entityType.ToLower() == "company") {
                Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Id == entityId).FirstOrDefault();
                EmailHelper.SendMail(email, company, emailAddress, _unitOfWork);
            }

            return new OkResult();
        }

        [HttpPost("smtp")]
        [Authorize(Policy = "WritableFitAdmin")]
        public IActionResult SendSmtpTestMail([FromBody] SmtpConfig smtpConfig, [FromQuery] string emailAddress) {
            if (smtpConfig != null && emailAddress != string.Empty) {
                Email email = new Email("SMTP-Test-Mail", "SMTP-Test-Mail");
                EmailHelper.SendMail(email, emailAddress, smtpConfig);
                return new NoContentResult();
            } else {
                return new BadRequestResult();
            }
        }
    }
}
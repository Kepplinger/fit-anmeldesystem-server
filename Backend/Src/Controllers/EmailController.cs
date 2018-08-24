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

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EmailController : Controller {

        private IUnitOfWork _unitOfWork;

        public EmailController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        public IActionResult GetAllEmails() {
            Email test = _unitOfWork.EmailRepository.Get().FirstOrDefault();
            List<Email> emails = _unitOfWork.EmailRepository.Get(includeProperties: "AvailableVariables").ToList();

            if (emails != null && emails.Count > 0) {
                return new OkObjectResult(emails.Select(e => mapEmailToDto(e)));
            } else {
                return new NoContentResult();
            }
        }

        [HttpPut]
        public IActionResult UpdateMail([FromBody] EmailDTO email) {
            Email emailEntity = mapDtoToEmail(email, _unitOfWork);

            if (_unitOfWork.EmailRepository.Get(m => m.Id == emailEntity.Id).FirstOrDefault() != null) {
                _unitOfWork.EmailRepository.Update(emailEntity);
                _unitOfWork.Save();
                return new OkObjectResult(mapEmailToDto(emailEntity));
            }
            return new BadRequestResult();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult sendTestMail(int id, [FromQuery] string emailAddress, [FromQuery] int entityId, [FromQuery] string entityType) {
            Email email = _unitOfWork.EmailRepository.GetById(id);

            if (entityType.ToLower() == "booking") {
                Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Id == entityId).FirstOrDefault();
                EmailHelper.SendMail(email, booking, emailAddress);
            } else if (entityType.ToLower() == "company") {
                Company company = _unitOfWork.CompanyRepository.Get(filter: c => c.Id == entityId).FirstOrDefault();
                EmailHelper.SendMail(email, company, emailAddress);
            }

            return new OkResult();
        }

        private Email mapDtoToEmail(EmailDTO emailTransfer, IUnitOfWork uow) {
            return new Email {
                Id = emailTransfer.Id,
                Timestamp = emailTransfer.Timestamp,
                Identifier = emailTransfer.Identifier,
                Name = emailTransfer.Name,
                Description = emailTransfer.Description,
                Subject = emailTransfer.Subject,
                Template = emailTransfer.Template,
                AvailableVariables = uow.EmailVariableUsageRepository.Get(ev => ev.fk_Email == emailTransfer.Id).ToList()
            };
        }

        private object mapEmailToDto(Email email) {
            return new EmailDTO {
                Id = email.Id,
                Timestamp = email.Timestamp,
                Identifier = email.Identifier,
                Name = email.Name,
                Description = email.Description,
                Template = email.Template,
                Subject = email.Subject,
                AvailableVariables = email.AvailableVariables.Select(v => v.EmailVariable).ToList()
            };
        }
    }
}

public class EmailDTO : TimestampEntityObject {
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Template { get; set; }
    public string Subject { get; set; }
    public List<EmailVariable> AvailableVariables { get; set; }
}

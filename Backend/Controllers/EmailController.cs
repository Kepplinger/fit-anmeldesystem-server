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

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EmailController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        public IActionResult GetAllEmails()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Email test = uow.EmailRepository.Get().FirstOrDefault();
                List<Email> emails = uow.EmailRepository.Get(includeProperties: "AvailableVariables").ToList();

                if (emails != null && emails.Count > 0)
                {
                    return new OkObjectResult(emails.Select(e => mapEmailToDto(e)));
                }
                else
                {
                    return new NoContentResult();
                }
            }
        }
        
        [HttpPut]
        public IActionResult UpdateMail([FromBody] EmailDTO email)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Email emailEntity = mapDtoToEmail(email, uow);

                if (uow.EmailRepository.Get(m => m.Id == emailEntity.Id).FirstOrDefault() != null)
                {
                    uow.EmailRepository.Update(emailEntity);
                    uow.Save();
                    return new OkObjectResult(mapEmailToDto(emailEntity));
                }
                return new BadRequestResult();
            }
        }

        private Email mapDtoToEmail(EmailDTO emailTransfer, IUnitOfWork uow)
        {
            return new Email
            {
                Id = emailTransfer.Id,
                Timestamp = emailTransfer.Timestamp,
                Name = emailTransfer.Name,
                Description = emailTransfer.Description,
                Subject = emailTransfer.Subject,
                Template = emailTransfer.Template,
                AvailableVariables = uow.EmailVariableUsageRepository.Get(ev => ev.fk_Email == emailTransfer.Id).ToList()
            };
        }

        private object mapEmailToDto(Email email)
        {
            return new EmailDTO
            {
                Id = email.Id,
                Timestamp = email.Timestamp,
                Name = email.Name,
                Description = email.Description,
                Template = email.Template,
                Subject = email.Subject,
                AvailableVariables = email.AvailableVariables.Select(v => v.EmailVariable).ToList()
            };
        }
    }
}

public class EmailDTO: TimestampEntityObject
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Template { get; set; }
    public string Subject { get; set; }
    public List<EmailVariable> AvailableVariables { get; set; }
}

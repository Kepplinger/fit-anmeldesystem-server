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

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EmailController : Controller
    {
        [HttpGet]
        [ProducesResponseType(typeof(Email), StatusCodes.Status200OK)]
        public IActionResult GetAllEmail()
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                List<Email> emails = uow.EmailRepository.Get().ToList();
                if (emails != null && emails.Count > 0)
                {
                    return new OkObjectResult(emails);
                }
                else
                {
                    return new NoContentResult();
                }
            }
        }
        [HttpGet("byName")]
        public IActionResult GetEmailByName(string name)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Email email = uow.EmailRepository.Get(m => m.Name.ToLower().Equals(name.ToLower())).FirstOrDefault();
                if (email != null)
                {
                    return new OkObjectResult(email);
                }
                else
                {
                    return new NoContentResult();
                }
            }
        }

        [HttpGet("byId/{emailId:int}")]
        public IActionResult GetEmailById(long emailId)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Email email = uow.EmailRepository.Get(m => m.Id == emailId).FirstOrDefault();
                if (email != null)
                {
                    return new OkObjectResult(email);
                }
                else
                {
                    return new NoContentResult();
                }
            }
        }

        [HttpPut]
        public IActionResult UpdateMail([FromBody] Email email)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                if (uow.EmailRepository.Get(m => m.Id == email.Id).FirstOrDefault() != null)
                {
                    uow.EmailRepository.Update(email);
                    uow.Save();
                    return new OkResult();
                }
                return new BadRequestResult();
            }
        }
    }
}

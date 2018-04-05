using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;
using StoreService.Persistence;
using Backend.Core.Contracts;
using Backend.Core.Entities;

namespace Backend.Controllers
{
    public class EmailController : Controller
    {
        [HttpGet]
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
        [HttpGet("byName/{paramName}")]
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

        [HttpGet("byId/{paramName}")]
        public IActionResult GetEmailById(long id)
        {
            using (IUnitOfWork uow = new UnitOfWork())
            {
                Email email = uow.EmailRepository.Get(m => m.Id == id).FirstOrDefault();
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

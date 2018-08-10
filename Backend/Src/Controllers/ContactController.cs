using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class ContactController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public ContactController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <response code="200">Returns all available Contacts</response>
        /// <summary>
        /// Getting all Contacts from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Contact), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var contacts = _unitOfWork.ContactRepository.Get();
            return new OkObjectResult(contacts);
        }

        /// <response code="200">Returns all available Contacts from one company</response>
        /// <summary>
        /// Getting all Contacts from Company
        /// </summary>
        [HttpGet("{companyId}")]
        [ProducesResponseType(typeof(List<Contact>), StatusCodes.Status200OK)]
        public IActionResult GetByCompanyId(int companyId)
        {
            Contact contact = _unitOfWork.CompanyRepository.Get(filter: con => con.Id == companyId).FirstOrDefault().Contact;
            return new OkObjectResult(contact);
        }
    }
}

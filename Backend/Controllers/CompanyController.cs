using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class CompanyController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }
        
        /// <response code="200">Returns all available Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.CompanyRepository.Get();
            return new OkObjectResult(companies);
            
        }


        [HttpPost]
        [ProducesResponseType(typeof(Company),StatusCodes.Status200OK)]
        public IActionResult CreateCompany(Company jsonComp)
        {

            Company storeCompany = jsonComp;
            storeCompany.RegistrationToken = Guid.NewGuid().ToString();
            _unitOfWork.CompanyRepository.Insert(storeCompany);
            _unitOfWork.Save();
            return new ObjectResult(storeCompany);
        }
    }
}

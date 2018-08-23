using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Src.Persistence.Facades;
using Backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class CompanyController : Controller {

        private IUnitOfWork _unitOfWork;
        private CompanyFacade _companyFacade;

        public CompanyController(IUnitOfWork uow) {
            _unitOfWork = uow;
            _companyFacade = new CompanyFacade(_unitOfWork);
        }

        /// <response code="200">Returns all available Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult GetAll() {
            var companies = _unitOfWork.CompanyRepository.Get(includeProperties: "Address,Contact,Tags,Branches");
            return new OkObjectResult(companies);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult CreateCompany([FromBody] Company jsonComp) {

            if (jsonComp != null) {
                Company storeCompany = jsonComp;
                if (jsonComp.Address.Addition == null) {
                    jsonComp.Address.Addition = "";
                }

                string loginCode = "";

                do {
                    loginCode = Guid.NewGuid().ToString().ToUpper();
                    loginCode = loginCode.Replace("-", String.Empty);
                    loginCode = loginCode.Substring(0, 12);
                    loginCode = loginCode.Insert(4, "-").Insert(9, "-");
                } while (_unitOfWork.CompanyRepository.Get(filter: c => c.RegistrationToken == loginCode).Count() != 0);

                storeCompany.RegistrationToken = loginCode;
                _unitOfWork.ContactRepository.Insert(storeCompany.Contact);
                _unitOfWork.Save();
                _unitOfWork.AddressRepository.Insert(storeCompany.Address);
                _unitOfWork.Save();

                _unitOfWork.CompanyRepository.Insert(storeCompany);
                _unitOfWork.Save();
                EmailHelper.SendMailByIdentifier("PGC", storeCompany, storeCompany.Contact.Email);
                EmailHelper.SendMailByIdentifier("PGA", storeCompany, storeCompany.Contact.Email);

                return new ObjectResult(storeCompany);
            }
            return new BadRequestResult();
        }

        [HttpPut]
        [Consumes("application/json")]
        public IActionResult Update([FromBody]Company company, [FromQuery] bool isAdminChange) {
            Contract.Ensures(Contract.Result<IActionResult>() != null);
            try {
                return new OkObjectResult(_companyFacade.Update(company, true, isAdminChange));
            } catch (DbUpdateException ex) {
                return DbErrorHelper.CatchDbError(ex);
            }
        }

        [HttpPut("accept/{compId}")]
        public IActionResult AcceptCompany(int compId, [FromBody] int status) {
            Company company = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == compId, includeProperties: "Contact,Address,Tags,Branches").FirstOrDefault();
            if (company != null) {
                company.IsAccepted = status;
                _unitOfWork.CompanyRepository.Update(company);
                _unitOfWork.Save();

                if (company.IsAccepted == 1) {
                    EmailHelper.SendMailByIdentifier("PAC", company, company.Contact.Email);
                }

                return new OkObjectResult(company);
            }
            return new BadRequestResult();
        }

        [HttpDelete("assign")]
        [Consumes("application/json")]
        public IActionResult AssignCompany(int pendingCompanyId, int existingCompanyId) {

            Company existingCompany = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(existingCompanyId), includeProperties: "Contact").FirstOrDefault();
            Company pendingCompany = _unitOfWork.CompanyRepository.Get(filter: c => c.Id.Equals(pendingCompanyId), includeProperties: "Contact").FirstOrDefault();

            if (existingCompany.Contact.Email.Equals(pendingCompany.Contact.Email)) {
                EmailHelper.SendMailByIdentifier("CA", existingCompany, existingCompany.Contact.Email);
            } else {
                EmailHelper.SendMailByIdentifier("CA", existingCompany, existingCompany.Contact.Email);
                EmailHelper.SendMailByIdentifier("CA", pendingCompany, existingCompany.Contact.Email);
            }

            _unitOfWork.CompanyRepository.Delete(pendingCompany);
            _unitOfWork.Save();
            return new NoContentResult();
        }
    }
}

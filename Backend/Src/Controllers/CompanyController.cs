using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Src.Persistence.Facades;
using Backend.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class CompanyController : Controller {

        private IUnitOfWork _unitOfWork;
        private CompanyFacade _companyFacade;
        private readonly UserManager<FitUser> _userManager;

        public CompanyController(UserManager<FitUser> userManager, IUnitOfWork uow) {
            _unitOfWork = uow;
            _companyFacade = new CompanyFacade(_unitOfWork);
            _userManager = userManager;
        }

        /// <response code="200">Returns all available Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        [Authorize(Policy = "AnyAdmin")]
        public IActionResult GetAll() {
            var companies = _unitOfWork.CompanyRepository.Get(includeProperties: "Address,Contact,Tags,Branches");
            return new OkObjectResult(companies);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateCompany([FromBody] Company jsonComp) {

            if (jsonComp != null) {
                Company company = jsonComp;
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

                company.RegistrationToken = loginCode;
                _unitOfWork.ContactRepository.Insert(company.Contact);
                _unitOfWork.Save();
                _unitOfWork.AddressRepository.Insert(company.Address);
                _unitOfWork.Save();

                FitUser companyUser = new FitUser();
                companyUser.UserName = company.RegistrationToken;
                companyUser.Role = "Member";

                await _userManager.CreateAsync(companyUser, company.RegistrationToken);

                company.fk_FitUser = companyUser.Id;

                _unitOfWork.CompanyRepository.Insert(company);
                _unitOfWork.Save();
                EmailHelper.SendMailByIdentifier("PGC", company, company.Contact.Email, _unitOfWork);
                EmailHelper.SendMailByIdentifier("PGA", company, company.Contact.Email, _unitOfWork);

                return new ObjectResult(company);
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
        [Authorize(Policy = "WritableAdmin")]
        public IActionResult AcceptCompany(int compId, [FromBody] int status) {
            Company company = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == compId, includeProperties: "Contact,Address,Tags,Branches").FirstOrDefault();
            if (company != null) {
                company.IsAccepted = status;
                _unitOfWork.CompanyRepository.Update(company);
                _unitOfWork.Save();

                if (company.IsAccepted == 1) {
                    EmailHelper.SendMailByIdentifier("PAC", company, company.Contact.Email, _unitOfWork);
                }

                return new OkObjectResult(company);
            }
            return new BadRequestResult();
        }

        [HttpDelete("assign")]
        [Authorize(Policy = "WritableAdmin")]
        [Consumes("application/json")]
        public IActionResult AssignCompany(int pendingCompanyId, int existingCompanyId) {

            Company existingCompany = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(existingCompanyId), includeProperties: "Contact").FirstOrDefault();
            Company pendingCompany = _unitOfWork.CompanyRepository.Get(filter: c => c.Id.Equals(pendingCompanyId), includeProperties: "Contact").FirstOrDefault();

            if (existingCompany.Contact.Email.Equals(pendingCompany.Contact.Email)) {
                EmailHelper.SendMailByIdentifier("CA", existingCompany, existingCompany.Contact.Email, _unitOfWork);
            } else {
                EmailHelper.SendMailByIdentifier("CA", existingCompany, existingCompany.Contact.Email, _unitOfWork);
                EmailHelper.SendMailByIdentifier("CA", pendingCompany, existingCompany.Contact.Email, _unitOfWork);
            }

            _unitOfWork.CompanyRepository.Delete(pendingCompany);
            _unitOfWork.Save();
            return new NoContentResult();
        }
    }
}

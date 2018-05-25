using Backend.Core.Contracts;
using Backend.Core.Entities;
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

        public CompanyController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        /// <response code="200">Returns all available Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult GetAll() {
            var companies = _unitOfWork.CompanyRepository.Get(filter: p => p.IsPending == false, includeProperties: "Address,Contact");
            return new OkObjectResult(companies);
        }

        /// <response code="200">Returns all pending Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet("pending")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult GetAllPending() {
            var companies = _unitOfWork.CompanyRepository.Get(filter: f => f.IsPending == true, includeProperties: "Address,Contact");
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
                EmailHelper.SendMailByName("IsPendingGottenCompany", storeCompany, storeCompany.Contact.Email);
                EmailHelper.SendMailByName("IsPendingGottenAdmin", storeCompany, storeCompany.Contact.Email);

                return new ObjectResult(storeCompany);
            }
            return new BadRequestResult();
        }

        [HttpPut("accepting")]
        public IActionResult Accepting([FromBody] int compId) {
            Company c = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == compId, includeProperties: "Contact,Address").FirstOrDefault();
            if (c != null) {
                c.IsPending = false;
                this._unitOfWork.CompanyRepository.Update(c);
                this._unitOfWork.Save();
                EmailHelper.SendMailByName("IsPendingAcceptedCompany", c, c.Contact.Email);

                return new OkResult();
            }
            return new BadRequestResult();
        }
        [HttpGet("presentation/{eventId:int}")]
        public IActionResult PresentationByEvent(int eventId) {
            List<object> pres = new List<object>();
            List<Booking> bookings = _unitOfWork.BookingRepository.Get(p => p.Presentation != null && p.Event.Id == eventId).ToList();
            for (int i = 0; i < 10; i++) {
                /*var companyPresentations = new
                {
                    company = bookings.ElementAt(i).Company,
                    presentation = bookings.ElementAt(i).Presentation,
                };*/
                var companyPresentations = new {
                    companyName = "company Name: " + i,
                    presentationTitle = "presentation title" + i,
                    presentationDescr = "This is a presentation description from: " + i,
                };
                pres.Add(companyPresentations);
            }
            return new OkObjectResult(pres);
        }

        [HttpPut]
        [Consumes("application/json")]
        public IActionResult Update([FromBody]Company jsonCompany) {
            Contract.Ensures(Contract.Result<IActionResult>() != null);

            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction()) {
                try {
                    Company companyToUpdate = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(jsonCompany.Id), includeProperties: "Address,Contact").FirstOrDefault();

                    if (jsonCompany.Address.Id != 0) {
                        ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Address), jsonCompany.Address, companyToUpdate.Address, nameof(Address), companyToUpdate.Id, false);
                        _unitOfWork.AddressRepository.Update(jsonCompany.Address);
                        _unitOfWork.Save();
                    }

                    if (jsonCompany.Contact.Id != 0) {
                        ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Contact), jsonCompany.Contact, companyToUpdate.Contact, nameof(Contact), companyToUpdate.Id, false);
                        _unitOfWork.ContactRepository.Update(jsonCompany.Contact);
                        _unitOfWork.Save();
                    }

                    if (jsonCompany.Id != 0) {
                        jsonCompany.RegistrationToken = companyToUpdate.RegistrationToken;
                        ChangeProtocolHelper.GenerateChangeProtocolForType(_unitOfWork, typeof(Company), jsonCompany, companyToUpdate, nameof(Company), companyToUpdate.Id, false);
                        _unitOfWork.CompanyRepository.Update(jsonCompany);
                        _unitOfWork.Save();
                    }

                    transaction.Commit();
                    return new OkObjectResult(jsonCompany);
                    
                } catch (DbUpdateException ex) {
                    transaction.Rollback();
                    return DbErrorHelper.CatchDbError(ex);
                }
            }

        }

        [HttpDelete("assign")]
        [Consumes("application/json")]
        public IActionResult CompanyAssign(int pendingCompanyId, int existingCompanyId) {

            Company existingCompany = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(existingCompanyId), includeProperties: "Contact").FirstOrDefault();
            Company pendingCompany = _unitOfWork.CompanyRepository.Get(filter: c => c.Id.Equals(pendingCompanyId), includeProperties: "Contact").FirstOrDefault();


            if (existingCompany.Contact.Email.Equals(pendingCompany.Contact.Email)) {
                EmailHelper.SendMailByName("CompanyAssigned", existingCompany, existingCompany.Contact.Email);
            } else {
                EmailHelper.SendMailByName("CompanyAssigned", existingCompany, existingCompany.Contact.Email);
                EmailHelper.SendMailByName("CompanyAssigned", pendingCompany, existingCompany.Contact.Email);
            }

            _unitOfWork.CompanyRepository.Delete(pendingCompany);
            _unitOfWork.Save();
            return new NoContentResult();
        }
    }
}

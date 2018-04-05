using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

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
            var companies = _unitOfWork.CompanyRepository.Get(filter: p => p.IsPending == false, includeProperties: "Address,Contact");
            return new OkObjectResult(companies);
        }

        /// <response code="200">Returns all pending Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet("pending")]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult GetAllPending()
        {
            var companies = _unitOfWork.CompanyRepository.Get(filter: f => f.IsPending == true, includeProperties: "Address,Contact");
            return new OkObjectResult(companies);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult CreateCompany([FromBody] Company jsonComp)
        {
            if (jsonComp != null)
            {
                Company storeCompany = jsonComp;

                string finalCode = "";
                string encoded = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                encoded = encoded.Replace("/", "_").Replace("+", "-");
                encoded = encoded.Substring(0, 12);

                for (int i = 1; i < encoded.Length; i++)
                {
                    if (i % 4 == 0)
                    {
                        finalCode += encoded[i - 1];
                        finalCode += "-";
                    }
                    else
                    {
                        finalCode += encoded[i - 1];
                    }
                }

                storeCompany.RegistrationToken = finalCode;
                _unitOfWork.ContactRepository.Insert(storeCompany.Contact);
                _unitOfWork.Save();
                _unitOfWork.AddressRepository.Insert(storeCompany.Address);
                _unitOfWork.Save();

                _unitOfWork.CompanyRepository.Insert(storeCompany);
                _unitOfWork.Save();
                EmailHelper.isPendingGottenAdmin(storeCompany);
                EmailHelper.isPendingGottenCompany(storeCompany);
                return new ObjectResult(storeCompany);
            }
            return new BadRequestResult();
        }

        [HttpPut("accepting")]
        public IActionResult Accepting([FromBody] int compId)
        {
            Company c = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == compId, includeProperties: "Contact,Address").FirstOrDefault();
            if (c != null)
            {
                c.IsPending = false;
                EmailHelper.IsPendingAcceptedCompany(c);
                return new OkResult();
            }
            return new BadRequestResult();
        }
        [HttpGet("presentation/{eventId:int}")]
        public IActionResult PresentationByEvent(int eventId)
        {
            List<object> pres = new List<object>();
            List<Booking> bookings = _unitOfWork.BookingRepository.Get(p => p.Presentation != null && p.Event.Id == eventId).ToList();
            for (int i = 0; i < 10; i++)
            {
                /*var companyPresentations = new
                {
                    company = bookings.ElementAt(i).Company,
                    presentation = bookings.ElementAt(i).Presentation,
                };*/

                var companyPresentations = new
                {
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
        public IActionResult Update([FromBody]Company jsonCompany)
        {
            Contract.Ensures(Contract.Result<IActionResult>() != null);

            using (IDbContextTransaction transaction = this._unitOfWork.BeginTransaction())
            {
                ChangeProtocol change = new ChangeProtocol();

                try
                {
                    Company toUpdate = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(jsonCompany.Id), includeProperties: "Address,Contact").FirstOrDefault();
                    if (jsonCompany.Address.Id != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Address).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk") && p.GetValue(jsonCompany.Address) != null && !p.GetValue(jsonCompany.Address).Equals(p.GetValue(toUpdate.Address)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonCompany.Address).ToString();
                                change.OldValue = p.GetValue(toUpdate.Address).ToString();
                                change.TableName = nameof(Address);
                                change.RowId = toUpdate.Address.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();
                                Console.WriteLine("No Update for" + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.AddressRepository.Update(jsonCompany.Address);
                        _unitOfWork.Save();

                    }

                    change = new ChangeProtocol();

                    if (jsonCompany.Contact.Id != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Contact).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk") && p.GetValue(jsonCompany.Contact) != null && !p.GetValue(jsonCompany.Contact).Equals(p.GetValue(toUpdate.Contact)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonCompany.Contact).ToString();
                                change.OldValue = p.GetValue(toUpdate.Contact).ToString();
                                change.TableName = nameof(Contact);
                                change.RowId = toUpdate.Contact.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }

                        }
                        _unitOfWork.ContactRepository.Update(jsonCompany.Contact);
                        _unitOfWork.Save();


                    }
                    change = new ChangeProtocol();

                    if (jsonCompany.Id != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Company).GetProperties())
                        {
                            jsonCompany.RegistrationToken = toUpdate.RegistrationToken;
                            if (!p.Name.Contains("Timestamp") && p.Name != "FolderInfo" && !p.Name.ToLower().Contains("id") && !p.Name.ToLower().Contains("fk") && p.GetValue(jsonCompany) != null && !p.GetValue(jsonCompany).Equals(p.GetValue(toUpdate)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = Convert.ToString(p.GetValue(jsonCompany));
                                change.OldValue = Convert.ToString(p.GetValue(toUpdate));
                                change.TableName = nameof(Company);
                                change.RowId = toUpdate.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                change.isAdminChange = false;
                                change.isReverted = false;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();
                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.CompanyRepository.Update(jsonCompany);
                        _unitOfWork.Save();
                    }
                    _unitOfWork.Save();
                    change = new ChangeProtocol();

                    transaction.Commit();
                    return new OkObjectResult(jsonCompany);
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();

                    String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                    System.Console.WriteLine(error);

                    return new BadRequestObjectResult(error);
                }
            }

        }

        [HttpDelete("assign")]
        [Consumes("application/json")]
        public IActionResult CompanyAssign(int pendingCompanyId, int existingCompanyId)
        {

            Company existingCompany = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(existingCompanyId), includeProperties: "Contact").FirstOrDefault();
            Company pendingCompany = _unitOfWork.CompanyRepository.Get(filter: c => c.Id.Equals(pendingCompanyId), includeProperties: "Contact").FirstOrDefault();


            if (existingCompany.Contact.Email.Equals(pendingCompany.Contact.Email))
            {
                EmailHelper.AssignedCompany(existingCompany);
            }
            else
            {
                EmailHelper.AssignedCompany(existingCompany);
                EmailHelper.AssignedCompany(pendingCompany);
            }

            _unitOfWork.CompanyRepository.Delete(pendingCompany);
            _unitOfWork.Save();
            return new NoContentResult();
        }
    }
}

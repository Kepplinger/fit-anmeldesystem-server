using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
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
            
            var companies = _unitOfWork.CompanyRepository.Get(includeProperties: "Address,Contact,FolderInfo");
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
            Company storeCompany = jsonComp;
            storeCompany.RegistrationToken = Guid.NewGuid().ToString();
            _unitOfWork.CompanyRepository.Insert(storeCompany);
            _unitOfWork.Save();
            return new ObjectResult(storeCompany);
        }

        [HttpPut("accepting")]
        public IActionResult Accepting([FromBody] int compId)
        {
            Company c = _unitOfWork.CompanyRepository.Get(filter: p => p.Id == compId).FirstOrDefault();
            if (c != null)
            {
                c.IsPending = false;
                return new OkResult();
            }
            return new BadRequestResult();
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
                    Company toUpdate = _unitOfWork.CompanyRepository.Get(filter: p => p.Id.Equals(jsonCompany.Id),includeProperties: "Address,Contact").FirstOrDefault();
                    if (jsonCompany.FK_Address != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Address).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && p.GetValue(jsonCompany.Address) != null && !p.GetValue(jsonCompany.Address).Equals(p.GetValue(toUpdate.Address)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonCompany.Address).ToString();
                                change.OldValue = p.GetValue(toUpdate.Address).ToString();
                                change.TableName = nameof(Address);
                                change.RowId = toUpdate.Address.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();
                                Console.WriteLine("No Update for" + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        }
                        _unitOfWork.AddressRepository.Update(jsonCompany.Address);
                    }

                    if (jsonCompany.FK_Contact != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Contact).GetProperties())
                        {
                            if (!p.Name.Contains("Timestamp") && !p.Name.ToLower().Contains("id") && p.GetValue(jsonCompany.Contact) != null && !p.GetValue(jsonCompany.Contact).Equals(p.GetValue(toUpdate.Contact)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = p.GetValue(jsonCompany.Contact).ToString();
                                change.OldValue = p.GetValue(toUpdate.Contact).ToString();
                                change.TableName = nameof(Contact);
                                change.RowId = toUpdate.Contact.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }
                        
                        }
                        _unitOfWork.ContactRepository.Update(jsonCompany.Contact);

                    }

                    if (jsonCompany.Id != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Company).GetProperties())
                        {
                            jsonCompany.RegistrationToken = toUpdate.RegistrationToken;
                            if (!p.Name.Contains("Timestamp") && p.Name!="FolderInfo" && !p.Name.ToLower().Contains("id") && p.GetValue(jsonCompany) != null && !p.GetValue(jsonCompany).Equals(p.GetValue(toUpdate)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumnName = p.Name;
                                change.NewValue = Convert.ToString(p.GetValue(jsonCompany));
                                change.OldValue = Convert.ToString(p.GetValue(toUpdate));
                                change.TableName = nameof(Company);
                                change.RowId = toUpdate.Id;
                                change.IsPending = true;
                                change.CompanyId = toUpdate.Id;
                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();
                                Console.WriteLine("Updated: " + change.ColumnName);
                                change = new ChangeProtocol();
                            }

                        }
                        _unitOfWork.CompanyRepository.Update(jsonCompany);

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
    }
}

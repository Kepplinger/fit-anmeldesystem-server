﻿using Backend.Core.Contracts;
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

        [HttpPut]
        public IActionResult Accepting(int compId)
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
                    if (toUpdate.FK_Address != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Address).GetProperties())
                        {
                            if (!p.Name.ToLower().Contains("id") && p.GetValue(jsonCompany.Address,null) != null && !p.GetValue(jsonCompany.Address).Equals(p.GetValue(toUpdate.Address)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumName = p.Name;
                                change.NewValue = p.GetValue(jsonCompany.Address).ToString();
                                change.OldValue = p.GetValue(toUpdate.Address).ToString();
                                change.TableName = nameof(Address);

                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();
                                change = new ChangeProtocol();

                                //change.TypeOfValue = p.PropertyType;
                                Console.WriteLine("No Update for" + change.ColumName);
                            }
                        }
                    }

                    if (toUpdate.FK_Contact != 0)
                    {
                        foreach (System.Reflection.PropertyInfo p in typeof(Contact).GetProperties())
                        {
                            if (!p.Name.ToLower().Contains("id") && p.GetValue(jsonCompany.Contact, null) != null && !p.GetValue(jsonCompany.Contact).Equals(p.GetValue(toUpdate.Contact)))
                            {
                                change.ChangeDate = DateTime.Now;
                                change.ColumName = p.Name;
                                change.NewValue = p.GetValue(jsonCompany.Contact).ToString();
                                change.OldValue = p.GetValue(toUpdate.Contact).ToString();
                                change.TableName = nameof(Contact);

                                _unitOfWork.ChangeRepository.Insert(change);
                                _unitOfWork.Save();

                                change = new ChangeProtocol();

                                //change.TypeOfValue = p.PropertyType;
                                Console.WriteLine("Updated: " + change.ColumName);
                            }
                        }
                    }

                    if (toUpdate.Name != null && !jsonCompany.Name.Equals(toUpdate.Name))
                    {
                        change.ChangeDate = DateTime.Now;
                        change.ColumName = "Name";
                        change.NewValue = jsonCompany.Name;
                        change.OldValue = toUpdate.Name;
                        change.TableName = nameof(Company);
                        _unitOfWork.ChangeRepository.Insert(change);
                        _unitOfWork.Save();
                        change = new ChangeProtocol();

                        Console.WriteLine("Updated: " + change.ColumName);

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

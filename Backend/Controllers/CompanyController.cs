using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]

    public class CompanyController
    {
        private IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }
        
        /// <summary>
        /// Creates a Company Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Company), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Company temp)
        {
            System.Console.WriteLine(temp.Name);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.CompanyRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        /// <response code="200">Returns all available Companies</response>
        /// <summary>
        /// Getting all Companies from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Company), 200)]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.CompanyRepository.Get();
            return new ObjectResult(companies);
        }
    }
}

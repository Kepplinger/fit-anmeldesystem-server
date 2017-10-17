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

        [HttpPut("Create")]
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

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var companies = _unitOfWork.CompanyRepository.Get();
            return new ObjectResult(companies);
        }
    }
}

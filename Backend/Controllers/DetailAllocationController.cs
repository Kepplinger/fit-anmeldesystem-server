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

    public class DetailAllocationController
    {
        private IUnitOfWork _unitOfWork;

        public DetailAllocationController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPut("Create")]
        public IActionResult Create([FromBody] DetailAllocation temp)
        {
            System.Console.WriteLine(temp.Text);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.DetailAllocationRepository.Insert(temp);
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
            var detailAllocations = _unitOfWork.DetailAllocationRepository.Get();
            return new ObjectResult(detailAllocations);
        }
    }
}

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

        /// <summary>
        /// Creates a DetailAllocation Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(DetailAllocation), 200)]
        [ProducesResponseType(typeof(void), 101)]
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

        /// <response code="200">Returns all available DeetailAllocations</response>
        /// <summary>
        /// Getting all DetailAllocations from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(DetailAllocation), 200)]
        public IActionResult GetAll()
        {
            var detailAllocations = _unitOfWork.DetailAllocationRepository.Get();
            return new ObjectResult(detailAllocations);
        }
    }
}

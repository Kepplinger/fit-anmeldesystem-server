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
    public class AreaController
    {
        private IUnitOfWork _unitOfWork;

        public AreaController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Deletes a specific Area.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Area), 200)]
        public IActionResult Create([FromBody] Area temp)
        {
            System.Console.WriteLine(temp.Designation);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.AreaRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        /// <summary>
        /// Returns all saved Addresses
        /// </summary>
        /// <response code="200">Returns all available Addresses</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Area), 200)]
        public IActionResult GetAll()
        {
            var areas = _unitOfWork.AreaRepository.Get();
            return new ObjectResult(areas);
        }
    }
}

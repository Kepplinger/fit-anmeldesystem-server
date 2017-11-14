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

    public class PackageController
    {
        private IUnitOfWork _unitOfWork;

        public PackageController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Package Object
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Package), 200)]
        public IActionResult Create([FromBody] Package temp)
        {
            System.Console.WriteLine(temp.Name);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.PackageRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Returns all saved Bookings
        /// </summary>
        /// <response code="200">Returns all available Bookings</response>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(IActionResult), 200)]

        public IActionResult GetAll()
        {
            var packages = _unitOfWork.PackageRepository.Get();
            return new ObjectResult(packages);
        }
    }
}

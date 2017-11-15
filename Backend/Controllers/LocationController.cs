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

    public class LocationController
    {
        private IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Location Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Location), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Location temp)
        {
            System.Console.WriteLine(temp.Number);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.LocationRepository.Insert(temp);
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

        /// <response code="200">Returns all available Locations</response>
        /// <summary>
        /// Getting all Locations from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Location), 200)]
        public IActionResult GetAll()
        {
            var locations = _unitOfWork.LocationRepository.Get();
            return new ObjectResult(locations);
        }
    }
}

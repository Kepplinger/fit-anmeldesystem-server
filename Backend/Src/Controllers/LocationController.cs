﻿using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class LocationController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <response code="200">Returns all available Locations</response>
        /// <summary>
        /// Getting all Locations from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Location), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var locations = _unitOfWork.LocationRepository.Get();
            return new OkObjectResult(locations);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Location), StatusCodes.Status200OK)]
        public IActionResult PostLocation([FromBody]Location jsonLocation)
        {

            _unitOfWork.LocationRepository.Insert(jsonLocation);
            _unitOfWork.Save();

            return new OkObjectResult( jsonLocation);
        }

        [HttpPut()]
        [ProducesResponseType(typeof(Location), StatusCodes.Status200OK)]
        public IActionResult PutLocation([FromBody]Location jsonLocation)
        {
            _unitOfWork.LocationRepository.Update(jsonLocation);
            _unitOfWork.Save();
            return new OkObjectResult(jsonLocation);
        }
    }
}
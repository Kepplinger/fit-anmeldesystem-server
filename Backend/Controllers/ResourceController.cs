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

    public class ResourceController
    {
        private IUnitOfWork _unitOfWork;

        public ResourceController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Resource Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Resource), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Resource temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.ResourceRepository.Insert(temp);
                    _unitOfWork.Save();

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

    
        /// <response code="200">Returns all available Resources</response>
        /// <summary>
        /// Getting all Resources from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Resource), 200)]
        public IActionResult GetAll()
        {
            var resources = _unitOfWork.ResourceRepository.Get();
            return new ObjectResult(resources);
        }
    }
}

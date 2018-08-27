using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
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
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(Resource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
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
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }


        /// <response code="200">Returns all available Resources</response>
        /// <summary>
        /// Getting all Resources from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Resource), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var resources = _unitOfWork.ResourceRepository.Get();
            return new OkObjectResult(resources);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Resource), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] List<Resource> resources) {
            if (resources != null) {
                foreach (Resource resource in resources) {
                    if (resource.Id > 0) {
                        _unitOfWork.ResourceRepository.Update(resource);
                    } else {
                        _unitOfWork.ResourceRepository.Insert(resource);
                    }
                }
                _unitOfWork.Save();
                return new OkObjectResult(resources);
            } else {
                return new BadRequestResult();
            }
        }
    }
}

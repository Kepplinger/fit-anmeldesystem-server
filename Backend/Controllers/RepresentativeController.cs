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
    [Produces("application/json", "application/xml")]
    public class RepresentativeController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public RepresentativeController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

       /// <summary>
        /// Creates a Representative
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(Representative), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] Representative temp)
        {
            try
            {
                if (temp != null)
                {
                    _unitOfWork.RepresentativeRepository.Insert(temp);
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

        /// <response code="200">Returns all available Representatives</response>
        /// <summary>
        /// Getting all Representatives from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Representative), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var representatives = _unitOfWork.RepresentativeRepository.Get();
            return new OkObjectResult(representatives);
        }
    }
}

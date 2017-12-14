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
    public class PresentationController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public PresentationController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Presentation
        /// </summary>
        /// <response code="200">Returns the newly-created presentation</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BadRequestResult),StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] Presentation temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp != null)
                {
                    _unitOfWork.PresentationRepository.Insert(temp);
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

        /// <response code="200">Returns all available Presentations</response>
        /// <summary>
        /// Getting all Presentations
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var presentations = _unitOfWork.PresentationRepository.Get();
            return new ObjectResult(presentations);
        }
    }
}

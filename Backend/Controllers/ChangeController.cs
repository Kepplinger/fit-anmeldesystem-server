using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class ChangeController
    {
        private IUnitOfWork _unitOfWork;

        public ChangeController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ChangeProtocol), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            
            List<ChangeProtocol> changes = _unitOfWork.ChangeRepository.Get().ToList();
            if (changes != null && changes.Count > 0)
            {
                return new OkObjectResult(changes);

            }
            else
            {
                return new NoContentResult();
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using Backend.Core.Entities;

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

        [HttpGet]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            List<Presentation> pres = _unitOfWork.PresentationRepository.Get().ToList<Presentation>();
            if (pres != null && pres.Count > 0)
            {
                return new OkObjectResult(pres);
            }
            return new NoContentResult();
        }

        [HttpGet("presentationId:int")]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult Get(int presentationId)
        {
            Presentation pres = _unitOfWork.PresentationRepository.Get(p => p.Id == presentationId).FirstOrDefault();
            pres.IsAccepted = true;
            return new OkObjectResult(pres);
        }

    }
}

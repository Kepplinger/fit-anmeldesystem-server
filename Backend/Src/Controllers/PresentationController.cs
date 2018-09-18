using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Src.Persistence.Facades;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class PresentationController : Controller {

        private IUnitOfWork _unitOfWork;
        private PresentationFacade _presentationFacade;

        public PresentationController(IUnitOfWork uow) {
            _unitOfWork = uow;
            _presentationFacade = new PresentationFacade(_unitOfWork);
        }

        [HttpGet("{eventId}")]
        [Authorize(Policy = "FitAdmin")]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult GetByEvent(int eventID) {
            List<PresentationDTO> presentations = _unitOfWork.BookingRepository
                .Get(b => b.fk_Event == eventID)
                .Select(b => new PresentationDTO { presentation = b.Presentation, company = b.Company })
                .ToList();

            if (presentations != null && presentations.Count > 0) {
                return new OkObjectResult(presentations);
            }
            return new NoContentResult();
        }

        [HttpPut()]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult Update(int id, [FromBody] Presentation presentation) {
            if (presentation != null) {
                return new ObjectResult(_presentationFacade.Update(presentation));
            } else {
                return new BadRequestResult();
            }
        }

        [HttpPut("accept/{id}")]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult Accept(int id, [FromBody] int status) {
            Presentation presentation = _unitOfWork.PresentationRepository.Get(filter: p => p.Id == id).FirstOrDefault();
            if (presentation != null) {
                presentation.IsAccepted = status;
                return new ObjectResult(_presentationFacade.Update(presentation));
            } else {
                return new BadRequestResult();
            }
        }
    }
}

class PresentationDTO {
    public Presentation presentation;
    public Company company;
}

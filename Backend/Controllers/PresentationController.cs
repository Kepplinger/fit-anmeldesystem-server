using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using Backend.Core.Entities;

namespace Backend.Controllers {
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class PresentationController : Controller {
        private IUnitOfWork _unitOfWork;

        public PresentationController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet("{eventId}")]
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
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult Update(int id, [FromBody] Presentation presentation) {
            if (presentation != null) {
                _unitOfWork.PresentationRepository.Update(presentation);
                _unitOfWork.Save();
                return new ObjectResult(presentation);
            } else {
                return new BadRequestResult();
            }
        }

        [HttpPut("accept/{id}")]
        [ProducesResponseType(typeof(Presentation), StatusCodes.Status200OK)]
        public IActionResult Accept(int id, [FromBody] int status) {
            Presentation presentation = _unitOfWork.PresentationRepository.Get(filter: p => p.Id == id).FirstOrDefault();
            if (presentation != null) {
                presentation.IsAccepted = status;
                _unitOfWork.PresentationRepository.Update(presentation);
                _unitOfWork.Save();
                return new ObjectResult(presentation);
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

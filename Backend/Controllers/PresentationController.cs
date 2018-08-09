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
                return new ObjectResult(UpdatePresentation(presentation));
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
                return new ObjectResult(UpdatePresentation(presentation));
            } else {
                return new BadRequestResult();
            }
        }

        private Presentation UpdatePresentation(Presentation presentation) {

            List<PresentationBranch> presentationBranches = _unitOfWork.PresentationBranchesRepository
                .Get(pb => pb.fk_Presentation == presentation.Id, includeProperties: "Branch")
                .ToList();

            foreach (PresentationBranch presentationBranch in presentationBranches) {
                int index = presentation.Branches.FindIndex(pb => pb.fk_Branch == presentationBranch.fk_Branch);

                if (index != -1) {
                    presentation.Branches[index] = presentationBranch;
                } else {
                    _unitOfWork.PresentationBranchesRepository.Delete(presentationBranch);
                    _unitOfWork.Save();
                }
            }

            _unitOfWork.PresentationRepository.Update(presentation);
            _unitOfWork.Save();
            presentation = _unitOfWork.PresentationRepository.Get(filter: p => p.Id == presentation.Id).FirstOrDefault();
            return presentation;
        }
    }
}

class PresentationDTO {
    public Presentation presentation;
    public Company company;
}

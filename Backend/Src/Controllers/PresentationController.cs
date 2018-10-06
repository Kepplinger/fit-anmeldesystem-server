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
using Backend.Utils;

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
                .Get(b => b.fk_Event == eventID && b.Presentation != null)
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
            Booking booking = _unitOfWork.BookingRepository.Get(filter: b => b.Presentation != null && b.Presentation.Id == id).FirstOrDefault();

            if (booking.Presentation != null) {
                booking.Presentation.IsAccepted = status;
                Presentation presentation = _presentationFacade.Update(booking.Presentation);

                if (status == 1) {
                    EmailHelper.SendMailByIdentifier("PA", booking, booking.Contact.Email, _unitOfWork);
                } else if (status == -1) {
                    booking.fk_FitPackage = _unitOfWork.PackageRepository.Get(p => p.Discriminator == 2).FirstOrDefault().Id;
                    _unitOfWork.BookingRepository.Update(booking);
                    _unitOfWork.Save();
                    EmailHelper.SendMailByIdentifier("PR", booking, booking.Contact.Email, _unitOfWork);
                }

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

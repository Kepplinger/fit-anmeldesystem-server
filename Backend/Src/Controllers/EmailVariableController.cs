using Backend.Core;
using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class EmailVariableController : Controller {

        private IUnitOfWork _unitOfWork;

        public EmailVariableController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(EmailVariable), StatusCodes.Status200OK)]
        public IActionResult GetAll() {
            List<EmailVariable> emailVariables = _unitOfWork.EmailVariableRepository.Get().ToList();
            return new OkObjectResult(emailVariables);
        }
    }
}

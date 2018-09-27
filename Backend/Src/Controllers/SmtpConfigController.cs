using Backend.Core.Contracts;
using Backend.Src.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class SmtpConfigController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public SmtpConfigController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(SmtpConfig), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public IActionResult Get() {
            SmtpConfig smtpConfig = _unitOfWork.SmtpConfigRepository.Get().FirstOrDefault();
            if (smtpConfig != null) {
                return new OkObjectResult(smtpConfig);
            } else {
                return new NoContentResult();
            }
        }

        [HttpPut]
        [Authorize(Policy = "WritableFitAdmin")]
        public IActionResult Update([FromBody] SmtpConfig smtpConfig) {
            if (smtpConfig != null) {
                _unitOfWork.SmtpConfigRepository.Update(smtpConfig);
                _unitOfWork.Save();
                return new OkObjectResult(smtpConfig);
            } else {
                return new NoContentResult();
            }
        }
    }
}

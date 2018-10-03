using Backend.Core.Contracts;
using Backend.Src.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MemberStatusController {

        private IUnitOfWork _unitOfWork;

        public MemberStatusController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(MemberStatus), StatusCodes.Status200OK)]
        public IActionResult GetAll() {
            List<MemberStatus> meberStati = _unitOfWork.MemberStatusRepository.Get().ToList();
            return new OkObjectResult(meberStati);
        }
    }
}

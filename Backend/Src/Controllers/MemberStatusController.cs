using Backend.Core.Contracts;
using Backend.Src.Core.Entities;
using Microsoft.AspNetCore.Authorization;
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
            List<MemberStatus> meberStati = _unitOfWork.MemberStatusRepository
                .Get(m => !m.IsArchive)
                .OrderBy(m => m.DefaultPrice)
                .ToList();
            return new OkObjectResult(meberStati);
        }

        [HttpGet]
        [Route("archive")]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(MemberStatus), StatusCodes.Status200OK)]
        public IActionResult GetAllArchived() {
            var meberStati = _unitOfWork.MemberStatusRepository.Get(filter: m => m.IsArchive);
            return new OkObjectResult(meberStati);
        }

        [HttpPut]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(MemberStatus), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] List<MemberStatus> meberStati) {
            if (meberStati != null) {
                foreach (MemberStatus mebmerStatus in meberStati) {
                    if (mebmerStatus.Id > 0) {
                        _unitOfWork.MemberStatusRepository.Update(mebmerStatus);
                    } else {
                        _unitOfWork.MemberStatusRepository.Insert(mebmerStatus);
                    }
                }
                _unitOfWork.Save();
                return new OkObjectResult(meberStati);
            } else {
                return new BadRequestResult();
            }
        }
    }
}

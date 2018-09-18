using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using StoreService.Persistence;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class TagController : Controller {

        private IUnitOfWork _unitOfWork;

        public TagController(IUnitOfWork uow) {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [Authorize(Policy = "AnyAdmin")]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
        public IActionResult Get() {
            List<Tag> tags = _unitOfWork.TagRepository.Get().ToList();
            if (tags != null) {
                return new OkObjectResult(tags);
            } else {
                return new NoContentResult();
            }
        }

        [HttpPut]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Tag), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] List<Tag> tags) {
            if (tags != null) {
                foreach (Tag tag in tags) {
                    if (tag.Id > 0) {
                        _unitOfWork.TagRepository.Update(tag);
                    } else {
                        _unitOfWork.TagRepository.Insert(tag);
                    }
                }
                _unitOfWork.Save();
                return new OkObjectResult(tags);
            } else {
                return new BadRequestResult();
            }
        }
    }
}

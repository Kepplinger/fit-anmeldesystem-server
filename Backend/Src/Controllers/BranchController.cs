using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class BranchController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public BranchController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Branch), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var branches = _unitOfWork.BranchRepository.Get(filter: b => !b.IsArchive);
            return new OkObjectResult(branches);
        }

        [HttpGet]
        [Route("archive")]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Branch), StatusCodes.Status200OK)]
        public IActionResult GetAllArchived() {
            var branches = _unitOfWork.BranchRepository.Get(filter: b => b.IsArchive);
            return new OkObjectResult(branches);
        }

        [HttpPut]
        [Authorize(Policy = "WritableFitAdmin")]
        [ProducesResponseType(typeof(Branch), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        public IActionResult Put([FromBody] List<Branch> branches) {
            if (branches != null) {
                foreach (Branch branch in branches) {
                    if (branch.Id > 0) {
                        _unitOfWork.BranchRepository.Update(branch);
                    } else {
                        _unitOfWork.BranchRepository.Insert(branch);
                    }
                }
                _unitOfWork.Save();
                return new OkObjectResult(branches);
            } else {
                return new BadRequestResult();
            }
        }
    }
}
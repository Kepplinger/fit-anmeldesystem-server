using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class PackageController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public PackageController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Returns all Packages
        /// </summary>
        /// <response code="200">Returns all available Bookings</response>
        [HttpGet]
        [ProducesResponseType(typeof(IActionResult), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var packages = _unitOfWork.PackageRepository.Get();
            return new OkObjectResult(packages);
        }

        [HttpPut]
        [Authorize(Policy = "WritableFitAdmin")]
        public IActionResult Update([FromBody] FitPackage fitPackage) {
            _unitOfWork.PackageRepository.Update(fitPackage);
            _unitOfWork.Save();

            return new OkObjectResult(_unitOfWork.PackageRepository.Get());
        }
    }
}

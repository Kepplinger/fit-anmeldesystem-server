using Backend.Core.Contracts;
using Backend.Src.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class LockPageController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public LockPageController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpGet]
        public IActionResult GetLockPage()
        {
            LockPage lockPage = _unitOfWork.LockPageRepository.Get().FirstOrDefault();
            return new OkObjectResult(lockPage);
        }

        [HttpPost]
        public IActionResult GetLockPage([FromBody] LockPage lockPage)
        {
            _unitOfWork.LockPageRepository.Update(lockPage);
            _unitOfWork.Save();
            return new OkObjectResult(lockPage);
        }
    }
}

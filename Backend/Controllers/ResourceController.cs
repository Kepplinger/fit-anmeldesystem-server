using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]

    public class ResourceController
    {
        private IUnitOfWork _unitOfWork;

        public ResourceController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPut("Create")]
        public IActionResult Create([FromBody] Resource temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.ResourceRepository.Insert(temp);
                    _unitOfWork.Save();

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var resources = _unitOfWork.ResourceRepository.Get();
            return new ObjectResult(resources);
        }
    }
}

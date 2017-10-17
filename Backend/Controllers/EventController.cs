using Backend.Core.Contracts;
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

    public class EventController
    {
        private IUnitOfWork _unitOfWork;

        public EventController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPut("Create")]
        public IActionResult Create([FromBody] Core.Entities.Event temp)
        {
            System.Console.WriteLine(temp.EventDate);
            try
            {
                if (temp != null)
                {
                    _unitOfWork.EventRepository.Insert(temp);
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
            var events = _unitOfWork.EventRepository.Get();
            return new ObjectResult(events);
        }
    }
}

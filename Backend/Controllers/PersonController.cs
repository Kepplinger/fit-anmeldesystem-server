
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

    public class PersonController
    {
        private IUnitOfWork _unitOfWork;

        public PersonController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPut("Create")]
        public IActionResult Create([FromBody] Person temp)
        {
            System.Console.WriteLine(temp.LastName);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.PersonRepository.Insert(temp);
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
            var persons = _unitOfWork.PersonRepository.Get();
            return new ObjectResult(persons);
        }
    }
}

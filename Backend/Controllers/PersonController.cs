
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

        /// <summary>
        /// Creates a Person Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Person), 200)]
        [ProducesResponseType(typeof(void), 101)]
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

        /// <response code="200">Returns all available Persons</response>
        /// <summary>
        /// Getting all Persosns from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Person), 200)]
        public IActionResult GetAll()
        {
            var persons = _unitOfWork.PersonRepository.Get();
            return new ObjectResult(persons);
        }
    }
}

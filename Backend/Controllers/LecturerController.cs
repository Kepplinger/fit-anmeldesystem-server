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

    public class LecturerController
    {
        private IUnitOfWork _unitOfWork;

        public LecturerController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Lecturer Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Lecturer), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Lecturer temp)
        {
            System.Console.WriteLine(temp.Person.LastName);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.LecturerRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        /// <response code="200">Returns all available Lecturers</response>
        /// <summary>
        /// Getting all Lecturers from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Lecturer), 200)]
        public IActionResult GetAll()
        {
            var lecturers = _unitOfWork.LecturerRepository.Get();
            return new ObjectResult(lecturers);
        }
    }
}

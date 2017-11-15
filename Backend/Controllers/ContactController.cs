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

    public class ContactController
    {
        private IUnitOfWork _unitOfWork;

        public ContactController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Contact Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Contact), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Contact temp)
        {
           // System.Console.WriteLine(temp.Person.LastName);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.ContactRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Console.WriteLine(ex.Message);
            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        /// <response code="200">Returns all available Contacts</response>
        /// <summary>
        /// Getting all Contacts from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Contact), 200)]
        public IActionResult GetAll()
        {
            var contacts = _unitOfWork.ContactRepository.Get();
            return new ObjectResult(contacts);
        }
    }
}

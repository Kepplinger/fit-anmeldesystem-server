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

    public class CategoryController
    {
        private IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Category Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Category), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Category temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.CategoryRepository.Insert(temp);
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

        /// <response code="200">Returns all available Categories</response>
        /// <summary>
        /// Getting all Categories from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Category), 200)]
        public IActionResult GetAll()
        {
            var categories = _unitOfWork.CategoryRepository.Get();
            return new ObjectResult(categories);
        }
    }
}

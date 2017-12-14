﻿using Backend.Core.Contracts;
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
    [Produces("application/json", "application/xml")]
    public class AreaController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public AreaController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates an Area
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="400">If the item is null</response>
        [HttpPost]
        [ProducesResponseType(typeof(Area), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Area), StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] Area temp)
        {
            System.Console.WriteLine(temp.Designation);
            try
            {
                if (temp != null)
                {
                    _unitOfWork.AreaRepository.Insert(temp);
                    _unitOfWork.Save();
                    return new ObjectResult(temp);
                }
            }
            catch (DbUpdateException ex)
            {
                String error = "*********************\n\nDbUpdateException Message: " + ex.Message + "\n\n*********************\n\nInnerExceptionMessage: " + ex.InnerException.Message;
                System.Console.WriteLine(error);
                return new BadRequestObjectResult(error);
            }
            return new StatusCodeResult(StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Returns all saved Addresses
        /// </summary>
        /// <response code="200">Returns all available Addresses</response>
        [HttpGet]
        [ProducesResponseType(typeof(Area), 200)]
        public IActionResult GetAll()
        {
            var areas = _unitOfWork.AreaRepository.Get();
            return new ObjectResult(areas);
        }
    }
}

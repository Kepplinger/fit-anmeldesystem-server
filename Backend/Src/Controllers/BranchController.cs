﻿using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class BranchController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public BranchController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <response code="200">Returns all available Addresses</response>
        /// <summary>
        /// Getting all Addresses from Database
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Branch), StatusCodes.Status200OK)]
        public IActionResult GetAll()
        {
            var branches = _unitOfWork.BranchRepository.Get();
            return new OkObjectResult(branches);
        }
    }
}
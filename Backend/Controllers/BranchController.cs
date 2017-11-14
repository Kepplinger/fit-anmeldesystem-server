using Backend.Core.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Backend.Core.Contracts;

namespace Backend.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class BranchController
    {
        private IUnitOfWork _unitOfWork;

        public BranchController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Address Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Branch), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Branch insertBranch)
        {
            System.Console.WriteLine(insertBranch.Name);
            try
            {
                if (insertBranch != null)
                {
                    _unitOfWork.BranchRepository.Insert(insertBranch);
                    _unitOfWork.Save();
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        [HttpGet("Test")]
        public IActionResult Test()
        {
            Branch b = new Branch() { Name = "Test" };
            //Address a = new Address() { Street = "Teststraße", City = "Wien", PostalCode = "2322", Number = "55" };
            /*_unitOfWork.AddressRepository.Insert(a);
            a.PostalCode = "2222";
            _unitOfWork.Save();
            _unitOfWork.AddressRepository.Update(a);
            _unitOfWork.Save();*/
            return new OkObjectResult(b);
        }


        /// <response code="200">Returns all available Addresses</response>
        /// <summary>
        /// Getting all Addresses from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Branch), 200)]
        public IActionResult GetAll()
        {
            var branches = _unitOfWork.BranchRepository.Get();
            return new ObjectResult(branches);
        }
    }
}
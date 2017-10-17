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
    public class AddressController
    {
        private IUnitOfWork _unitOfWork;

        public AddressController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        /// <summary>
        /// Creates a Address Object.
        /// </summary>
        /// <response code="200">Returns the newly-created item</response>
        /// <response code="101">If the item is null</response>
        [HttpPut("Create")]
        [ProducesResponseType(typeof(Address), 200)]
        [ProducesResponseType(typeof(void), 101)]
        public IActionResult Create([FromBody] Address insertAdress)
        {
            System.Console.WriteLine(insertAdress.Street);
            try
            {
                if (insertAdress != null)
                {
                    _unitOfWork.AddressRepository.Insert(insertAdress);
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
            Address a = new Address() { Street = "Teststraße", City = "Wien", PostalCode = "2322", Number = "55" };
            /*_unitOfWork.AddressRepository.Insert(a);
            a.PostalCode = "2222";
            _unitOfWork.Save();
            _unitOfWork.AddressRepository.Update(a);
            _unitOfWork.Save();*/
            return new OkObjectResult(a);
        }


        /// <response code="200">Returns all available Addresses</response>
        /// <summary>
        /// Getting all Addresses from Database
        /// </summary>
        [HttpGet("GetAll")]
        [ProducesResponseType(typeof(Address), 200)]
        public IActionResult GetAll()
        {
            var addresses = _unitOfWork.AddressRepository.Get();
            return new ObjectResult(addresses);
        }
    }
}
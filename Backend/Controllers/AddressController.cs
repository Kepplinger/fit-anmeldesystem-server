using Backend.Core.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Backend.Core.Contracts;

namespace Backend.Controllers
{
     [Route("api/[controller]")]
    public class AddressController
    {
        private IUnitOfWork _unitOfWork;
        
       public AddressController(IUnitOfWork uow)
       {
            this._unitOfWork = uow;
       }

       [HttpPut("Create")]
        public  IActionResult Create([FromBody] Address temp)
        {
            System.Console.WriteLine(temp.Street);
            try
            {
                if (temp!=null)
                {
                    
                    _unitOfWork.AddressRepository.Insert(temp);
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

        [HttpGet("Test")]
        public IActionResult Test()
        {
            Address a = new Address() { Street = "Teststraﬂe", City = "Wien", PostalCode = "2322", Number="55" };
            _unitOfWork.AddressRepository.Insert(a);
            a.PostalCode = "2222";
            _unitOfWork.Save();
            _unitOfWork.AddressRepository.Update(a);
            _unitOfWork.Save();
            return new OkObjectResult(a);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var addresses = _unitOfWork.AddressRepository.Get();
            return new ObjectResult(addresses);
        }
    }
}
using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
     [Route("api/[controller]")]
    public class AddressController
    {
        private IRepository<Address> _addressRepo;
        
       public AddressController(ApplicationContext cb)
       {
           _addressRepo = new Repository<Address>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Address temp)
        {
            System.Console.WriteLine(temp.Street);
            try
            {
                if (temp!=null)
                {
                    
                    _addressRepo.Insert(temp);
                    _addressRepo.Save();
                    //System.Console.WriteLine(temp.Company.Name);
                    
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        [HttpGet("GetAll", Name = "GetAll")]
        public IActionResult GetAll()
        {
            string[] include = new string[] {""};
            var bookings = from s in _addressRepo.GetAll() select s;
            return new ObjectResult(bookings);
        }
    }
}
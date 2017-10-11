using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class PersonController
    {
         private IRepository<Person> _personRepo;
            
        public PersonController(ApplicationContext cb)
        {
            _personRepo = new Repository<Person>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Person temp)
        {
            System.Console.WriteLine(temp.LastName);
            try
            {
                if (temp!=null)
                {
                    _personRepo.Insert(temp);
                    _personRepo.Save();
                    //System.Console.WriteLine(temp.Company.Name);
                    
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }   
           public IActionResult GetAll()
        {
            var persons = from st in _personRepo.GetAll() select st;
            return new ObjectResult(persons);
        }
    }
}
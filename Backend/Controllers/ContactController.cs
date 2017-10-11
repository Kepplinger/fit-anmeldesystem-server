using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ContactController
    {
         private IRepository<Contact> _contactRepo;
        
       public ContactController(ApplicationContext cb)
       {
           _contactRepo = new Repository<Contact>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Contact temp)
        {
            System.Console.WriteLine(temp.Person.LastName);
            try
            {
                if (temp!=null)
                {
                    _contactRepo.Insert(temp);
                    _contactRepo.Save();
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
            var contacts = from st in _contactRepo.GetAll(new String[]{"Person"}) select st;
            return new ObjectResult(contacts);
        }
    }
}
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
    public class PresentationController
    {
        private IRepository<Presentation> _presentationRepo;
            
        public PresentationController(ApplicationContext cb)
        {
            _presentationRepo = new Repository<Presentation>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Presentation temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp!=null)
                {
                    _presentationRepo.Insert(temp);
                    _presentationRepo.Save();
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
            var presentations = from st in _presentationRepo.GetAll() select st;
            return new ObjectResult(presentations);
        }
       
    }
}
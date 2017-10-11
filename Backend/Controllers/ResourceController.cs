using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class ResourceController
    {
        private IRepository<Resource> _resourceRepo;
            
        public ResourceController(ApplicationContext cb)
        {
            _resourceRepo = new Repository<Resource>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Resource temp)
        {
            System.Console.WriteLine(temp.Name);
            try
            {
                if (temp!=null)
                {
                    _resourceRepo.Insert(temp);
                    _resourceRepo.Save();
                    //System.Console.WriteLine(temp.Company.Name);
                    
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }   
    }
}
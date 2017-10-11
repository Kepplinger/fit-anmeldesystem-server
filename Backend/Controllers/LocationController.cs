using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class LocationController
    {
        private IRepository<Location> _locationRepo;
            
        public LocationController(ApplicationContext cb)
        {
            _locationRepo = new Repository<Location>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Location temp)
        {
            System.Console.WriteLine(temp.Number);
            try
            {
                if (temp!=null)
                {
                    _locationRepo.Insert(temp);
                    _locationRepo.Save();
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
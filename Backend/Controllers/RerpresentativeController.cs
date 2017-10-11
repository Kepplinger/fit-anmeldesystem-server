using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class RerpresentativeController
    {
        
        private IRepository<Rerpresentative> _rerpresentativeRepo;

        public RerpresentativeController(ApplicationContext cb) => _rerpresentativeRepo = new Repository<Rerpresentative>(cb);

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Rerpresentative temp)
        {
            System.Console.WriteLine(temp);
            try
            {
                if (temp!=null)
                {
                    _rerpresentativeRepo.Insert(temp);
                    _rerpresentativeRepo.Save();
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
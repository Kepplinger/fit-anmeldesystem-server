using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class RerpresentativeController
    {
        
        private IRepository<Representative> _rerpresentativeRepo;

        public RerpresentativeController(ApplicationContext cb) => _rerpresentativeRepo = new Repository<Repre>(cb);

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
    }
}
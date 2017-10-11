using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
     [Route("api/[controller]")]
    public class DeatilAllocationController
    {
        private IRepository<DetailAllocation> _detailAllocationRepo;
            
       public DeatilAllocationController(ApplicationContext cb)
       {
           _detailAllocationRepo = new Repository<DetailAllocation>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] DetailAllocation temp)
        {
            System.Console.WriteLine(temp.Text);
            try
            {
                if (temp!=null)
                {
                    _detailAllocationRepo.Insert(temp);
                    _detailAllocationRepo.Save();
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
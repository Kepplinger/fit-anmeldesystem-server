using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    public class ResourceBookingController
    {
             private IRepository<ResourceBooking> _resourceBookingRepo;
            
        public ResourceBookingController(ApplicationContext cb)
        {
            _resourceBookingRepo = new Repository<ResourceBooking>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] ResourceBooking temp)
        {
            System.Console.WriteLine(temp.Resource.Name);
            try
            {
                if (temp!=null)
                {
                    _resourceBookingRepo.Insert(temp);
                    _resourceBookingRepo.Save();
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
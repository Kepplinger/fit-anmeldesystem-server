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
    public class EventController
    {
        private IRepository<Backend.Entities.Event> _eventRepo;
            
        public EventController(ApplicationContext cb)
        {
            _eventRepo = new Repository<Backend.Entities.Event>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Backend.Entities.Event temp)
        {
            System.Console.WriteLine(temp.EventDate);
            try
            {
                if (temp!=null)
                {
                    _eventRepo.Insert(temp);
                    _eventRepo.Save();
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
            var events = from st in _eventRepo.GetAll() select st;
            return new ObjectResult(events);
        }
    }
}
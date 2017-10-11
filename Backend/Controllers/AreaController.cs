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
    public class AreaController
    {
         private IRepository<Area> _areaRepo;
        
       public AreaController(ApplicationContext cb)
       {
           _areaRepo = new Repository<Area>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Area temp)
        {
            System.Console.WriteLine(temp.Designation);
            try
            {
                if (temp!=null)
                {
                    _areaRepo.Insert(temp);
                    _areaRepo.Save();
                    //System.Console.WriteLine(temp.Company.Name);
                    
                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }
         [HttpGet("GetAll", Name = "GetAll")]
        public IActionResult GetAll()
        {
            var areas = from st in _areaRepo.GetAll(new String[]{"Event"}) select st;
            return new ObjectResult(areas);
        }
    }
}
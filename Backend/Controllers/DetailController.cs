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
    public class DetailController
    {
          private IRepository<Detail> _detailRepo;
        
       public DetailController(ApplicationContext cb)
       {
           _detailRepo = new Repository<Detail>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Detail temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp!=null)
                {
                    _detailRepo.Insert(temp);
                    _detailRepo.Save();
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
            var details = from st in _detailRepo.GetAll() select st;
            return new ObjectResult(details);
        }
    }
}
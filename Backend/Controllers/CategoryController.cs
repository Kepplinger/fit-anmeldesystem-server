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
    public class CategoryController
    {
        private IRepository<Category> _categoryRepo;
        
       public CategoryController(ApplicationContext cb)
       {
           _categoryRepo = new Repository<Category>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Category temp)
        {
            System.Console.WriteLine(temp.Description);
            try
            {
                if (temp!=null)
                {
                    _categoryRepo.Insert(temp);
                    _categoryRepo.Save();
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
            var categories = from st in _categoryRepo.GetAll(new String[]{"Location"}) select st;
            return new ObjectResult(categories);
        }
    }
}
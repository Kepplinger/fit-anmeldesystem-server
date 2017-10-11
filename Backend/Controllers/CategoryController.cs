using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
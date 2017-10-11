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
    public class CompanyController
    {
        private IRepository<Company> _companyRepo;
        
       public CompanyController(ApplicationContext cb)
       {
           _companyRepo = new Repository<Company>(cb);
       }

       [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Company temp)
        {
            System.Console.WriteLine(temp.Name);
            try
            {
                if (temp!=null)
                {
                    _companyRepo.Insert(temp);
                    _companyRepo.Save();
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
            var companies = from st in _companyRepo.GetAll(new String[]{"Contact","Address"}) select st;
            return new ObjectResult(companies);
        }
    }
}
using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class LecturerController
    {
        private IRepository<Lecturer> _lecturerRepo;
            
        public LecturerController(ApplicationContext cb)
        {
            _lecturerRepo = new Repository<Lecturer>(cb);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Lecturer temp)
        {
            System.Console.WriteLine(temp.Person.LastName);
            try
            {
                if (temp!=null)
                {
                    _lecturerRepo.Insert(temp);
                    _lecturerRepo.Save();
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
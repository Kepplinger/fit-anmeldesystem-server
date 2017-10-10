using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class BookingController
    {
        private IRepository<Booking> _bookingRepo;

        public BookingController(ApplicationContext cb) {
            _bookingRepo = new Repository<Booking>(cb);
        }


        [HttpGet("GetAll", Name = "GetAll")]
        public IActionResult GetAll()
        {
            var bookings = from s in _bookingRepo.GetAll(new String[]{"Category","Presentation"}) select s;
            return new ObjectResult(bookings);
        }

        [HttpPut("Create", Name = "Creates")]
        public  IActionResult Create([FromBody] Booking temp)
        {
            System.Console.WriteLine(temp.Presentation.Description);
            try
            {
                if (temp!=null)
                {
                    _bookingRepo.Insert(temp);
                    _bookingRepo.Save();
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
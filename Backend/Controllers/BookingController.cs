using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Data;
using Backend.Entities;
using Backend.Persistence;
using Backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    public class BookingController
    {
        private BookingRepository _bookingRepo;

        public BookingController(ApplicationContext cb) {
            _bookingRepo = new BookingRepository(cb);
        }

        [HttpGet("GetAll", Name = "GetAll")]
        public IActionResult GetAll()
        {
            return new ObjectResult(_bookingRepo.GetAll(new String[]{"Category","Presentation"}));
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
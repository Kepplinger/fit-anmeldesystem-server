using System;
using System.Collections.Generic;
using System.Linq;
using Backend.Data;
using Backend.Models;
using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;

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

    }
}
using Backend.Core.Contracts;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers
{
    [Route("api/[controller]")]

    public class LecturerController
    {
        private IUnitOfWork _unitOfWork;

        public LecturerController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPut("Create")]
        public IActionResult Create([FromBody] Lecturer temp)
        {
            System.Console.WriteLine(temp.Person.LastName);
            try
            {
                if (temp != null)
                {

                    _unitOfWork.LecturerRepository.Insert(temp);
                    _unitOfWork.Save();
                    //System.Console.WriteLine(temp.Company.Name);

                    return new StatusCodeResult(StatusCodes.Status200OK);
                }
            }
            catch (DbUpdateException ex)
            {

            }
            return new StatusCodeResult(StatusCodes.Status101SwitchingProtocols);
        }

        //[HttpGet("Test")]
        //public IActionResult Test()
        //{
        //    Address a = new Address() { Street = "Teststraße", City = "Wien", PostalCode = "2322", Number = "55" };
        //    _unitOfWork.AddressRepository.Insert(a);
        //    a.PostalCode = "2222";
        //    _unitOfWork.Save();
        //    _unitOfWork.AddressRepository.Update(a);
        //    _unitOfWork.Save();
        //    return new OkObjectResult(a);
        //}

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var lecturers = _unitOfWork.LecturerRepository.Get();
            return new ObjectResult(lecturers);
        }
    }
}

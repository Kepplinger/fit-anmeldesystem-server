using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Contracts;
using StoreService.Persistence;
using Backend.Core.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class LogInController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public LogInController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }


        [HttpPost("bookingbytoken")]
        [Microsoft.AspNetCore.Mvc.ProducesResponseType(typeof(Company), StatusCodes.Status200OK)]
        public IActionResult GetBookingAndCompanyByToken([FromBody] JToken json)
        {
            string token = json["token"].Value<string>();
            Company c = _unitOfWork.CompanyRepository.Get(filter: g => g.RegistrationToken.Equals(token)).First();

            _unitOfWork.BookingRepository.Get(f => f.Company.Id.Equals(c.Id)).OrderByDescending(p => p.CreationDate);

            return new OkObjectResult(c);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.Core.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Utils;
using StoreService.Persistence;
using Backend.Core.Entities;
using System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class RegistrationController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public RegistrationController(IUnitOfWork uow)
        {
            this._unitOfWork = uow;
        }

        [HttpPost]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        public IActionResult ForgotCode([FromBody] JToken json)
        {
            Debugger.Break();

            Company existing;
            string mail = json["email"].Value<string>();

            using (IUnitOfWork uow = new UnitOfWork())
            {
                existing = uow.CompanyRepository.Get(filter: p => p.FolderInfo.Email.Equals(mail)).FirstOrDefault();
            }
            if (existing != null)
            {
                var a = new
                {
                    existing = "true"
                };
                return new OkObjectResult(a);
            }
            else
            {
                var a = new
                {
                    existing = "false"
                };
                return new OkObjectResult(a);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend.Core.Entities;
using Backend.Core.Contracts;
using StoreService.Persistence;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Controllers {

    [Route("api/[controller]")]
    [Produces("application/json", "application/xml")]
    public class GraduateController : Controller {

        [HttpPut]
        [Authorize(Policy = "MemberAndWriteableAdmins")]
        public IActionResult updateGraduate([FromBody] Graduate graduate) {
            using (IUnitOfWork uow = new UnitOfWork()) {
                Graduate toUpdate = uow.GraduateRepository.Get(g => g.Id == graduate.Id).FirstOrDefault();

                if (toUpdate != null) {
                    graduate.RegistrationToken = toUpdate.RegistrationToken;
                    uow.AddressRepository.Update(graduate.Address);
                    uow.GraduateRepository.Update(graduate);
                    uow.Save();
                    return new OkObjectResult(graduate);
                } else {
                    return new BadRequestResult();
                }
            }
        }

        [HttpGet]
        [Authorize(Policy = "AnyAdmin")]
        public IActionResult GetAll() {
            using (IUnitOfWork uow = new UnitOfWork()) {
                List<Graduate> graduates = uow.GraduateRepository.Get(includeProperties: "Address").ToList();

                if (graduates != null && graduates.Count > 0) {
                    return new OkObjectResult(graduates);
                } else {
                    return new NoContentResult();
                }
            }
        }
    }
}
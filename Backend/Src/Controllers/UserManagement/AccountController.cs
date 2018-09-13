using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StoreService.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers.UserManagement
{
    [Route("api/[controller]")]

    public class AccountController : Controller
    {
        private IUnitOfWork uow;
        private readonly UserManager<FitUser> _userManager;

        public AccountController(UserManager<FitUser> userManager, IUnitOfWork uow)
        {
            _userManager = userManager;
            this.uow = uow;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UserCredentials userCredentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            userCredentials.FitUser.UserName = userCredentials.FitUser.Email;
            var result = await _userManager.CreateAsync(userCredentials.FitUser, userCredentials.Password);
            await _userManager.AddToRoleAsync(userCredentials.FitUser, userCredentials.FitUser.Role);

            if (!result.Succeeded) return new BadRequestObjectResult(result);

            uow.Save();

            return new OkObjectResult("Account created");
        }
    }

    public class UserCredentials {
        public FitUser FitUser { get; set; }
        public string Password { get; set; }
    }
}

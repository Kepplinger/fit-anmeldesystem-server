using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Controllers.UserManagement
{
    [Route("api/[controller]")]

    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountController(UserManager<IdentityUser> userManager, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        }

        // POST api/accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]RegistrationUser registered)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            var result = await _userManager.CreateAsync(registered.User, registered.RegistrationPassword);

            if (!result.Succeeded) return new BadRequestObjectResult(result);

            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }
    }
}

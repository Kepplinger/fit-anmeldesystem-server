using Backend.Core.Contracts;
using Backend.Core.Entities;
using Backend.Core.Entities.UserManagement;
using Backend.Persistence;
using Microsoft.AspNetCore.Authorization;
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
        private IUnitOfWork _unitOfWork;
        private readonly UserManager<FitUser> _userManager;

        public AccountController(UserManager<FitUser> userManager, IUnitOfWork uow)
        {
            _userManager = userManager;
            _unitOfWork = uow;
        }

        [HttpGet]
        [Authorize(Policy = "WritableFitAdmin")]
        public IActionResult Get()
        {
            return new OkObjectResult(GetAllAdmins());
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = "WritableFitAdmin")]
        public async Task<IActionResult> Delete(string id)
        {
            FitUser user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
            _unitOfWork.Save();

            return new OkObjectResult(GetAllAdmins());
        }

        [HttpPost]
        [Authorize(Policy = "WritableFitAdmin")]
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

            _unitOfWork.Save();

            return new OkObjectResult(GetAllAdmins());
        }

        private List<object> GetAllAdmins()
        {
            return _userManager.Users.Where(u => u.Role != "Member")
                .Select(u => new {
                    id = u.Id,
                    email = u.Email,
                    role = u.Role
                } as object).ToList();
        }
    }

    public class UserCredentials
    {
        public FitUser FitUser { get; set; }
        public string Password { get; set; }
    }
}

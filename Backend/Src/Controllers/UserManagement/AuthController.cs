using Backend.Core.Entities.UserManagement;
using Backend.Src.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Controllers.UserManagement {

    [Route("api/[controller]")]
    public class AuthController : Controller {
        private readonly UserManager<FitUser> _userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;

        public AuthController(UserManager<FitUser> userManager,
                              IJwtFactory jwtFactory) {
            _userManager = userManager;
            _jwtFactory = jwtFactory;

            _serializerSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented
            };
        }

        // POST api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody]LoginCredentials credentials) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var identity = await UserClaimsHelper.GetClaimsIdentity(credentials.UserName, credentials.Password, _jwtFactory, _userManager);
            if (identity == null) {
                return BadRequest(new {
                    errorMessage = "Falsche E-Mail oder Passwort!"
                });
            }

            // Serialize and return the response
            var response = new {
                auth_token = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity),
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }
    }
}

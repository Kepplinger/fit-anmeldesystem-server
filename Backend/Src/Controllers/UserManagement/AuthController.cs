using Backend.Core.Entities.UserManagement;
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
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<FitUser> userManager,
                              IJwtFactory jwtFactory,
                              IOptions<JwtIssuerOptions> jwtOptions) {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

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

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null) {
                return BadRequest("Login failed invalid Username or Passsword");
            }

            // Serialize and return the response
            var response = new {
                id = identity.Claims.Single(c => c.Type == "id").Value,
                auth_token = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity),
                expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
            };

            var json = JsonConvert.SerializeObject(response, _serializerSettings);
            return new OkObjectResult(json);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password) {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)) {
                // get the user to verifty
                FitUser userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null) {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password)) {
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userToVerify));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}

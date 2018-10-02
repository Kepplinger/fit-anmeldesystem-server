using Backend.Controllers.UserManagement;
using Backend.Core.Entities.UserManagement;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Backend.Src.Utils {
    public class UserClaimsHelper {
        public static async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password, IJwtFactory jwtFactory, UserManager<FitUser> userManager) {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password)) {
                // get the user to verifty
                FitUser userToVerify = await userManager.FindByNameAsync(userName);

                if (userToVerify != null) {
                    // check the credentials  
                    if (await userManager.CheckPasswordAsync(userToVerify, password)) {
                        return await Task.FromResult(jwtFactory.GenerateClaimsIdentity(userToVerify));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        public static bool IsUserAdmin(ClaimsIdentity identity) {
            Claim role = identity.Claims.Where(c => c.Type == "rol").FirstOrDefault();
            return role != null && role.Value != "Member";
        }
    }
}

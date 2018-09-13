using Backend.Core.Contracts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities.UserManagement {

    public class FitUser : IdentityUser {
        public string Role { get; set; }
    }
}

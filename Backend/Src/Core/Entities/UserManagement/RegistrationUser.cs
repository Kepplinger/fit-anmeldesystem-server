using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities.UserManagement
{
    public class RegistrationUser
    {

        public IdentityUser User { get; set; }

        public string RegistrationPassword { get; set; }

    }
}

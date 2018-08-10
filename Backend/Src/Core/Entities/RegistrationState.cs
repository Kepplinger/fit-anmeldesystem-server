using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class RegistrationState : EntityObject
    {
        public bool IsLocked { get; set; }

        public bool IsCurrent { get; set; }
    }
}

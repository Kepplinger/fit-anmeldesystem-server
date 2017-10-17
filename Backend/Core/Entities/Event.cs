using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Core.Entities
{
    public class Event : EntityObject
    {
        [Required]
        public DateTime EventDate { get; set; }
        [Required]
        public DateTime RegistrationStart { get; set; }
        [Required]
        public DateTime RegistrationEnd { get; set; }
        [Required]
        public bool IsLocked { get; set; }

    }
}
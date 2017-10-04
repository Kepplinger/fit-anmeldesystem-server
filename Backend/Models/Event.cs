using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class Event : IEntityObject
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime RegistrationStart { get; set; }
        [Required]
        public DateTime RegistrationEnd { get; set; }
        [Required]
        public bool IsLocked { get; set; }
    }
}
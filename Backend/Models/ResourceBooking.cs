using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class ResourceBooking : IEntityObject
    {
        [ForeignKey("FK_Booking")]
        public Booking Booking { get; set; }

        public int FK_Booking { get; set; }
        
        [ForeignKey("FK_Resource")]
        public Resource Resource { get; set; }
        public int FK_Resource { get; set; }
        [Required]
        public int Amount { get; set; }   
    }
}
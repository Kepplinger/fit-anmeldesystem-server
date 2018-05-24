
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Branch : TimestampEntityObject
    {
        [Required]
        public string Name { get; set; }

        public List<Booking> Bookings { get; set; }
    }
}
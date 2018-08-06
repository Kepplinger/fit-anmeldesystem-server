using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Location : TimestampEntityObject
    {       
        [Required]
        public string Number { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public double XCoordinate { get; set; }
        [Required]
        public double YCoordinate { get; set; }
        [Required]
        public bool isOccupied { get; set; }
    }
}
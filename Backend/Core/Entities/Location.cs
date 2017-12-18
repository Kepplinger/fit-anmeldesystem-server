using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Location :EntityObject
    {       
        [Required]
        public int Number { get; set; }
        [ForeignKey("FK_Area")]
        public Area Area { get; set; }
        public int FK_Area { get; set; }
        [Required]
        public string Category { get; set; }
        [Required]
        public double XCoordinate { get; set; }
        [Required]
        public double YCoordinate { get; set; }
    }
}
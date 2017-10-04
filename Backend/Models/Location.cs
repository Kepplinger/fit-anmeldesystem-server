using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class Location :IEntityObject
    {       
        [Required]
        public int Number { get; set; }
        [ForeignKey("FK_Area")]
        public Area Area { get; set; }
        public int FK_Area { get; set; }
        [Required]
        public double XCoordinate { get; set; }
        [Required]
        public double YCoordinate { get; set; }
    }
}
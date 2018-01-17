using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Area : EntityObject
    {
        [Required]
        public string Designation { get; set; }
        [Required]
        public string GraphicURL { get; set; }

        [ForeignKey("FK_Locations"),Required]
        public List<Location> Locations { get; set; }
    }
}
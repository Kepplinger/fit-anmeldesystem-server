using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Backend.Core.Entities
{
    public class Area : TimestampEntityObject
    {
        [Required]
        public string Designation { get; set; }

        public DataFile Graphic { get; set; }

        public int fk_Event { get; set; }

        [JsonIgnore]
        [ForeignKey("fk_Event")]
        public Event Event { get; set; }

        public List<Location> Locations { get; set; }
    }
}
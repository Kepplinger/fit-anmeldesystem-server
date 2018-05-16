using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Backend.Core.Entities
{
    public class Area : EntityObject
    {
        [Required]
        public string Designation { get; set; }


        public string GraphicUrl { get; set; }

        public int? fk_Event { get; set; }

        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Event")]
        public virtual Event Event { get; set; }


        [Required]
        public List<Location> Locations { get; set; }
    }
}
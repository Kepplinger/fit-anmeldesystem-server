using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Backend.Core.Entities
{
    public class Resource : EntityObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }


        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("FK_Booking")]
        public virtual Booking Booking { get; set; }
        public int? FK_Booking { get; set; }
    }
}
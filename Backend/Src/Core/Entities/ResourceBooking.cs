using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Backend.Core.Entities
{
    public class ResourceBooking : TimestampEntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Booking")]
        public Booking Booking { get; set; }
        public int? fk_Booking { get; set; }

        [ForeignKey("fk_Resource")]
        public Resource Resource { get; set; }
        public int fk_Resource { get; set; }
    }
}
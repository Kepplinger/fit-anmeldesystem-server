using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Backend.Core.Entities
{
    public class ResourceBooking : EntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("FK_Booking")]
        public Booking Booking { get; set; }
        public int? FK_Booking { get; set; }

        [ForeignKey("FK_Resource")]
        public Resource Resource { get; set; }
        public int FK_Resource { get; set; }

    }
}
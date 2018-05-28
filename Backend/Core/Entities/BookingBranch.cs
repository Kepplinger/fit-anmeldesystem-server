using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class BookingBranch : TimestampEntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Booking")]
        public Booking Booking { get; set; }
        public int? fk_Booking { get; set; }

        [ForeignKey("fk_Branch")]
        public Branch Branch { get; set; }
        public int fk_Branch { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class CompanyTag : TimestampEntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Company")]
        public Company Comapny { get; set; }
        public int? fk_Company { get; set; }

        [ForeignKey("fk_Tag")]
        public Tag Tag { get; set; }
        public int fk_Tag { get; set; }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class CompanyBranch : TimestampEntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Company")]
        public Company Comapny { get; set; }
        public int? fk_Company { get; set; }

        [ForeignKey("fk_Branch")]
        public Branch Branch { get; set; }
        public int fk_Branch { get; set; }
    }
}

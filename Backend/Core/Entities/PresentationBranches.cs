﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class PresentationBranches : TimestampEntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Presentation")]
        public Presentation Presentation { get; set; }
        public int? fk_Presentation { get; set; }

        [ForeignKey("fk_Branch")]
        public Branch Branch { get; set; }
        public int fk_Branch { get; set; }
    }
}

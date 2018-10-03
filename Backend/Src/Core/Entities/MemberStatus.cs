using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Core.Entities
{
    public class MemberStatus : TimestampEntityObject {
        public string Name { get; set; }

        public double DefaultPrice { get; set; }

        public bool IsArchive { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class Package : EntityObject
    {
        public string Name { get; set; }

        public string Tag { get; set; }

        public int Number { get; set; }
    }
}

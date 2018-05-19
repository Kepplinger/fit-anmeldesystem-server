using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class DataFile : EntityObject
    {
        public string Name { get; set; }

        public string DataUrl { get; set; }

        public DataFile(string name, string dataUrl)
        {
            Name = name;
            DataUrl = dataUrl;
        }
    }
}

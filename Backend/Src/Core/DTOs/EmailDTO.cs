using Backend.Core;
using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Core.DTOs {
    public class EmailDTO : TimestampEntityObject {
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Template { get; set; }
        public string Subject { get; set; }
        public List<EmailVariable> AvailableVariables { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Src.Core.DTOs
{
    public class CustomTestEmailDTO
    {
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EntityType { get; set; }
        public int CompanyId { get; set; }
        public string Receiver { get; set; }
    }
}

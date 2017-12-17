using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class Representative : EntityObject
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }

    }
}
using Backend.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class EntityObject : IEntityObject {
        [Key]
        public int Id { get; set; }
    }
}

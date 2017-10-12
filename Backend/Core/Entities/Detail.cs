using Backend.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Core.Entities
{
    public class Detail : EntityObject
    {
        [Required]
        [MaxLength(30)]
        public string Description { get; set; }
    }
}
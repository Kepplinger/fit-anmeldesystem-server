using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class Detail : IEntityObject
    {
        [Required]
        [MaxLength(30)]
        public string Description { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Models
{
    public class Contact : IEntityObject
    {
        [ForeignKey("FK_Person")]
        public Person Person { get; set; }
        public int FK_Person { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Entities
{
    public class Rerpresentative : IEntityObject
    {
        [ForeignKey("FK_Person")]
        public Person Person { get; set; }
        public int FK_Person { get; set; }
        [ForeignKey("FK_Booking")]
        public Booking Booking { get; set; }

        public int FK_Booking { get; set; }
    }
}
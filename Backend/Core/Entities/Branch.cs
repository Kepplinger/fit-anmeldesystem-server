
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Branch : EntityObject
    {
        [Required]
        public string Name { get; set; }


        [ForeignKey("FK_Presentation")]
        public virtual Presentation Presentation { get; set; }

        public int FK_Presentation{get; set;}
}
        public int? FK_Presentation { get; set; }
    }
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Resource : EntityObject
    {
        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        [Required]
        [MaxLength(150)]
        public string Description { get; set; }
        
    }
}
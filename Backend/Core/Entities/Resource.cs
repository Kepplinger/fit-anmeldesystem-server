using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Resource : EntityObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }
        
    }
}
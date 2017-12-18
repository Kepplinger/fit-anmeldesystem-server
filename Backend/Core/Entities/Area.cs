using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Area : EntityObject
    {
        [Required]
        public string Designation { get; set; }
        [Required]
        public string GraphicURL { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class FitPackage : TimestampEntityObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Discriminator { get; set; }

        [Required]
        public int Price { get; set; }

        public string Description { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Branch : EntityObject
    {
        [Required]
        public string Name { get; set; }
    }
}

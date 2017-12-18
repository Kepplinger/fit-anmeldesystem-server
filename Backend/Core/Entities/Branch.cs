
namespace Backend.Core.Entities
{
    public class Branch : EntityObject
    {
        [System.ComponentModel.DataAnnotations.Required]
        public string Name { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Representative : EntityObject
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public DataFile Image { get; set; }

    }
}
using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Email : EntityObject
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required, MaxLength(300)]
        public string Description { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string Subject { get; set; }

        public Email() { }

        public Email(string _name, string _desc, string _template)
        {
            this.Name = _name;
            this.Description = _desc;
            this.Template = _template;
        }
    }
}

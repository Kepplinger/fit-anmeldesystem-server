using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Email : EntityObject
    {
        [Required, MaxLength(25)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string Subject { get; set; }

        public List<EmailVariable> AvailableVariables { get; set; }

        public Email() { }

        public Email(string _name, string _desc, string _template, string _subject)
        {
            this.Name = _name;
            this.Description = _desc;
            this.Template = _template;
            this.Subject = _subject;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class Email : TimestampEntityObject
    {
        [Required]
        public string Identifier { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Template { get; set; }

        [Required]
        public string Subject { get; set; }

        public List<EmailVariableUsage> AvailableVariables { get; set; }

        public Email() { }

        public Email(string _template, string _subject) {
            this.Template = _template;
            this.Subject = _subject;
        }

        public Email(string _identifier, string _name, string _desc, string _template, string _subject)
        {
            this.Identifier = _identifier;
            this.Name = _name;
            this.Description = _desc;
            this.Template = _template;
            this.Subject = _subject;
        }
    }
}

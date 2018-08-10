using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core
{
    public class EmailVariable : TimestampEntityObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public string Entity { get; set; }

        public EmailVariable() { }

        public EmailVariable(string name, string value, string entity)
        {
            Name = name;
            Value = entity + "." + value;
            Entity = entity;
        }
    }
}

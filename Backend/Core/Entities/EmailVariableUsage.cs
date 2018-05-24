using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class EmailVariableUsage : TimestampEntityObject
    {
        [JsonIgnore]
        [IgnoreDataMember]
        [ForeignKey("fk_Email")]
        public Email Email { get; set; }
        public int? fk_Email { get; set; }

        [ForeignKey("fk_EmailVariable")]
        public EmailVariable EmailVariable { get; set; }
        public int fk_EmailVariable { get; set; }
         
        public EmailVariableUsage() { }

        public EmailVariableUsage(Email email, EmailVariable emailVariable)
        {
            Email = email;
            EmailVariable = emailVariable;
        }

        public EmailVariableUsage(int? fk_Email, EmailVariable emailVariable)
        {
            this.fk_Email = fk_Email;
            EmailVariable = emailVariable;
        }
    }
}

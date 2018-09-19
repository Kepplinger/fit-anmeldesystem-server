using Backend.Core.Entities.UserManagement;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Company : TimestampEntityObject
    {
        [JsonIgnore]
        public FitUser FitUser { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("fk_Address"), Required]
        public Address Address { get; set; }
        public int fk_Address { get; set; }

        [ForeignKey("fk_Contact"), Required]
        public Contact Contact { get; set; }
        public int fk_Contact { get; set; }

        public List<CompanyTag> Tags { get; set; }

        public List<CompanyBranch> Branches { get; set; }

        [Required]
        public int IsAccepted { get; set; }

        [Required, JsonIgnore]
        public string RegistrationToken { get; set; }

        public double MemberPaymentAmount { get; set; }

        public int MemberSince { get; set; }

        public int MemberStatus { get; set; } // 0 nichts, 1 interessiert, 2 kla, 3 groß

        public Company() {
            Tags = new List<CompanyTag>();
            Branches = new List<CompanyBranch>();
        }
    }
}
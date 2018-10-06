using Backend.Core.Entities.UserManagement;
using Backend.Src.Core.Entities;
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

        [JsonIgnore]
        [ForeignKey(nameof(FitUser))]
        public string fk_FitUser { get; set; }

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

        public int fk_MemberStatus { get; set; }

        [ForeignKey("fk_MemberStatus")]
        public MemberStatus MemberStatus { get; set; }

        public Company() {
            Tags = new List<CompanyTag>();
            Branches = new List<CompanyBranch>();
        }
    }
}
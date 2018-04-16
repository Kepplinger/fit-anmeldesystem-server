using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Company : EntityObject
    {
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [ForeignKey("FK_Address"), Required]
        public Address Address { get; set; }
        public int FK_Address { get; set; }

        [ForeignKey("FK_Contact"), Required]
        public Contact Contact { get; set; }
        public int FK_Contact { get; set; }

        [Required]
        public bool IsPending { get; set; }

        [Required, JsonIgnore]
        public string RegistrationToken { get; set; }

        public double MemberPaymentAmount { get; set; }

        public int MemberSince { get; set; }

        public int MemberStatus { get; set; } // 0 nichts, 1 interessiert, 2 kla, 3 groß
    }
}
using Backend.Core.Entities.UserManagement;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Graduate : TimestampEntityObject
    {
        [JsonIgnore]
        public FitUser FitUser { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(FitUser))]
        public string fk_FitUser { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [ForeignKey("fk_Address"), Required]
        public Address Address { get; set; }

        public int fk_Address { get; set; }

        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int GraduationYear { get; set; }

        [Required, JsonIgnore]
        public string RegistrationToken { get; set; }
    }
}

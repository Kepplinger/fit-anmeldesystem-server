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

        [ForeignKey("FK_Address"),Required]
        public Address Address { get; set; }
        public int FK_Address { get; set; }

        [ForeignKey("FK_Contact"),Required]
        public Contact Contact { get; set; }
        public int FK_Contact { get; set; }

        [ForeignKey("FK_FolderInfo")]
        public FolderInfo FolderInfo { get; set; }
        public int FK_FolderInfo { get; set; }
            
        [Required]
        public bool IsPending { get; set; }



        [Required,JsonIgnore]
        public string RegistrationToken { get; set; }
 
       
        
    }
}
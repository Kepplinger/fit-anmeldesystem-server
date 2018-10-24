using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Booking : TimestampEntityObject
    {
        [ForeignKey("fk_Event"), Required]
        public Event Event { get; set; }
        public int fk_Event { get; set; }

        [ForeignKey("fk_Company"), Required]
        public Company Company { get; set; }
        public int fk_Company { get; set; }

        [ForeignKey("fk_Location")]
        public Location Location { get; set; }

        public int? fk_Location { get; set; }

        [ForeignKey("fk_Presentation")]
        public Presentation Presentation { get; set; }
        public int? fk_Presentation { get; set; }

        public List<BookingBranch> Branches { get; set; }

        [Required]
        public List<Representative> Representatives { get; set; }
        
        public List<ResourceBooking> Resources { get; set; }

        [Required]
        public int isAccepted { get; set; }

        [ForeignKey("fk_FitPackage"), Required]
        public FitPackage FitPackage { get; set; }
        public int fk_FitPackage { get; set; }

        public string AdditionalInfo { get; set; }

        public string Remarks { get; set; }

        public bool ProvidesSummerJob { get; set; }

        public bool ProvidesThesis { get; set; }

        public DateTime CreationDate { get; set; }

        public String CompanyDescription { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        [Required, Url]
        public string Homepage { get; set; }

        public DataFile Logo { get; set; }

        [Required]
        public string Branch { get; set; }

        public int EstablishmentsCountInt { get; set; }

        public string EstablishmentsInt { get; set; }

        public int EstablishmentsCountAut { get; set; }

        [ForeignKey("fk_Contact")]
        public Contact Contact { get; set; }

        public int? fk_Contact{ get; set; }

        public string EstablishmentsAut { get; set; }

        [JsonIgnore]
        public string PdfFilePath { get; set; }
    }
}
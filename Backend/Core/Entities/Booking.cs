using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Booking : EntityObject
    {
        [ForeignKey("FK_Event"), Required]
        public Event Event { get; set; }
        public int FK_Event { get; set; }

        [ForeignKey("FK_Company"), Required]
        public Company Company { get; set; }
        public int FK_Company { get; set; }

        [ForeignKey("FK_Location")]
        public Location Location { get; set; }

        public int? FK_Location { get; set; }

        [ForeignKey("FK_Presentation")]
        public Presentation Presentation { get; set; }
        public int? FK_Presentation { get; set; }

        public List<Branch> Branches { get; set; }

        [Required]
        public List<Representative> Representatives { get; set; }

        public List<ResourceBooking> Resources { get; set; }

        [Required]
        public bool isAccepted { get; set; }

        [ForeignKey("FK_FitPackage"), Required]
        public FitPackage FitPackage { get; set; }
        public int FK_FitPackage { get; set; }

        [MaxLength(500)]
        public string AdditionalInfo { get; set; }

        [MaxLength(500)]
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

        public string Logo { get; set; }

        [Required, MaxLength(20)]
        public string Branch { get; set; }

        public int EstablishmentsCountInt { get; set; }

        [MaxLength(30)]
        public string EstablishmentsInt { get; set; }

        public int EstablishmentsCountAut { get; set; }

        [ForeignKey("FK_Contact")]
        public Contact Contact { get; set; }

        [MaxLength(30)]
        public string EstablishmentsAut { get; set; }

        public string PdfFilePath { get; set; }

    }
}
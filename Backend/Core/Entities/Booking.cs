using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Booking : EntityObject
    {
        public Event Event { get; set; }

        public Company Company { get; set; }

        public Location Location { get; set; }

        public Presentation Presentation { get; set; }

        public List<Branch> Branches { get; set; }

        [Required]
        public List<Representative> Representatives { get; set; }

        public List<Resource> Resources { get; set; }

        [Required]
        public bool isAccepted { get; set; }

        public FitPackage FitPackage { get; set; }

        [MaxLength(500)]
        public string AdditionalInfo { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        public bool ProvidesSummerJob { get; set; }

        public bool ProvidesThesis { get; set; }

        public DateTime CreationDate { get; set; }

        public String CompanyDescription { get; set; }

    }
}
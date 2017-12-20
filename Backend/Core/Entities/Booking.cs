using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Booking : EntityObject
    {
        [ForeignKey("FK_Event"),Required]
        public Event Event { get; set; }
        public int FK_Event { get; set; }

        [ForeignKey("FK_Company"),Required]
        public Company Company { get; set; }
        public int FK_Company { get; set; }

        [ForeignKey("FK_Location"),Required]
        public Location Location { get; set; }
        public int FK_Location { get; set; }

        [ForeignKey("FK_Presentation")]
        public Presentation Presentation { get; set; }
        public int? FK_Presentation { get; set; }

        [ForeignKey("FK_Branches")]
        public List<Branch> Branches { get; set; }

        [ForeignKey("FK_Representatives"),Required]
        public List<Representative> Representatives { get; set; }

        [ForeignKey("FK_Resources")]
        public List<Resource> Resources { get; set; }

        [Required]
        public bool isAccepted { get; set; }

        [ForeignKey("FK_FitPackage"),Required]
        public FitPackage FitPackage { get; set; }
        public int FK_FitPackage { get; set; }

        [MaxLength(500)]
        public string AdditionalInfo { get; set; }

        [MaxLength(500)]
        public string Remarks { get; set; }

        public bool ProvidesSummerJob { get; set; }

        public bool ProvidesThesis { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
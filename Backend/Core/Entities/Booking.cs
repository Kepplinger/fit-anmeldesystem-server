using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Entities
{
    public class Booking : EntityObject
    {

        public int FK_Event { get; set; }
        
        [ForeignKey("FK_Event")]
        [Required]
        public Event Event { get; set; }

        [ForeignKey("FK_Company")]
        [Required]
        public Company Company { get; set; }
        public int FK_Company { get; set; }

        [ForeignKey("FK_Location")]
        [Required]
        public Location Location { get; set; }
        public int FK_Location { get; set; }

        [ForeignKey("FK_Presentation")]
        public virtual Presentation Presentation { get; set; }
        public int? FK_Presentation { get; set; }

        [ForeignKey("FK_Branches")]
        public List<Branch> Branches { get; set; }
        public int FK_Branches { get; set; }


        [ForeignKey("FK_Representatives")]
        [Required]
        public List<Representative> Representatives { get; set; }
        public int FK_Representatives { get; set; }

        [ForeignKey("FK_Resources")]
        public List<Resource> Resources { get; set; }
        public int FK_Resources { get; set; }

        [Required]
        public bool isAccepted { get; set; }

        [ForeignKey("FK_FitPackage")]
        public FitPackage FitPackage { get; set; }
        public int FK_FitPackage { get; set; }

        public string AdditionalInfo { get; set; }

        public string Remarks { get; set; }

        public bool ProvidesSummerJob { get; set; }

        public bool ProvidesThesis { get; set; }

        public DateTime CreationDate { get; set; }

        public override string ToString()
        {
            return string.Format("[Booking: FK_Event={0}, Event={1}, Company={2}, FK_Company={3}, Location={4}, FK_Location={5}, Presentation={6}, FK_Presentation={7}, Branches={8}, FK_Branches={9}, Representatives={10}, FK_Representatives={11}, Resources={12}, FK_Resources={13}, isAccepted={14}, Package={15}, FK_Package={16}, AdditionalInfo={17}, Remarks={18}, ProvidesSummerJob={19}, ProvidesThesis={20}, CreationDate={21}]", FK_Event, Event, Company, FK_Company, Location, FK_Location, Presentation, FK_Presentation, Branches, FK_Branches, Representatives, FK_Representatives, Resources, FK_Resources, isAccepted, FitPackage, FK_FitPackage, AdditionalInfo, Remarks, ProvidesSummerJob, ProvidesThesis, CreationDate);
        }
    }
}
using Backend.Core.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Entities
{
    public class Booking : EntityObject
    {

        public int FK_Event { get; set; }
        
        [ForeignKey("FK_Event")]
        public Event Event { get; set; }

        [ForeignKey("FK_Company")]
        public Company Company { get; set; }

        public int FK_Company { get; set; }

        [ForeignKey("FK_Location")]
        public Location Location { get; set; }
        public int FK_Location { get; set; }

        [ForeignKey("FK_Presentation")]
        public virtual Presentation Presentation { get; set; }

        public int FK_Presentation { get; set; }

        [ForeignKey("FK_Branches")]
        public List<Branch> Branches { get; set; }
        public int FK_Branches { get; set; }

        [Required]
        public bool isAccepted { get; set; }

        [ForeignKey("FK_Package")]
        public Package Package { get; set; }
        public int FK_Package { get; set; }

        public string CompanyDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public string Remarks { get; set; }
        public bool ProvidesSummerJob { get; set; }
        public bool ProvidesThesis { get; set; }
        public DateTime CreationDate { get; set; }

        public override string ToString()
        {
            return string.Format("[Booking: FK_Event={0}, Event={1}, Company={2}, FK_Company={3}, Location={4}, FK_Location={5}, Presentation={6}, FK_Presentation={7}, Branches={8}, FK_Branches={9}, isAccepted={10}, Package={11}, FK_Package={12}, CompanyDescription={13}, AdditionalInfo={14}, Remarks={15}, ProvidesSummerJob={16}, ProvidesThesis={17}, CreationDate={18}]", FK_Event, Event, Company, FK_Company, Location, FK_Location, Presentation, FK_Presentation, Branches, FK_Branches, isAccepted, Package, FK_Package, CompanyDescription, AdditionalInfo, Remarks, ProvidesSummerJob, ProvidesThesis, CreationDate);
        }
    }
}
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
    }
}
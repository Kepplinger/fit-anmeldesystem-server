using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Core.Entities
{
    public class Presentation : TimestampEntityObject
    {
        public List<PresentationBranch> Branches { get; set; }

        public string RoomNumber { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public int IsAccepted { get; set; }

        public DataFile File { get; set; }
    }
}
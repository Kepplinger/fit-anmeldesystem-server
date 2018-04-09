using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Core.Entities
{
    public class Presentation : EntityObject
    {
        public List<Branch> Branches { get; set; }

        public string RoomNumber { get; set; }

        [Required]
        [MaxLength(30)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsAccepted { get; set; }

        public string FileURL { get; set; }
    }
}
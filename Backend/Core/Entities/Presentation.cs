using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Core.Entities
{
    public class Presentation : EntityObject
    {
        [ForeignKey("FK_Branch")]
        public List<Branch> Branches { get; set; }
        public int FK_Branch { get; set; }

        [Required]
        public string RoomNumber { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        [MaxLength(400)]
        public string Description { get; set; }
        [Required]
        public bool IsAccepted { get; set; }

        public string FileURL { get; set; }
    }
}
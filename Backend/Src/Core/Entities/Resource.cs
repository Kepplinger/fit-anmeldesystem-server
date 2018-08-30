using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Backend.Core.Entities
{
    public class Resource : TimestampEntityObject
    {
        [Required]
        public string Name { get; set; }

        public bool IsArchive { get; set; }
    }
}
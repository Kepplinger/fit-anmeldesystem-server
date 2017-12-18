using Backend.Core.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class EntityObject : IEntityObject
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        public byte[] Timestamp
        {
            get;
            set;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Backend.Entities
{
    public class IEntityObject
    {
        public int Id { get; set; }

        byte[] Timestamp { get; set; }
    }
}
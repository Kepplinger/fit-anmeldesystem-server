using System;
using Backend.Core.Contracts;
using Backend.Core.Contracts.Repositories;
using System.ComponentModel.DataAnnotations;

namespace Backend.Core.Entities
{
    public class TimestampEntityObject : ITimestampEntityObject
    {
        [Key]
        public int Id { get; set; }

        [Timestamp]
        //public DateTime Timestamp { get; set; }
        public byte[] Timestamp { get; set; }
    }
}

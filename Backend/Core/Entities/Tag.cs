using System;
using Backend.Core.Contracts;
namespace Backend.Core.Entities
{
    public class Tag : EntityObject
    {
        public bool IsArchive { get; set; }

        public string Value { get; set; }
    }
}


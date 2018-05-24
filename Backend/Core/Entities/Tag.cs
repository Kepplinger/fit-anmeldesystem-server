using System;
using Backend.Core.Contracts;
namespace Backend.Core.Entities {

    public class Tag : TimestampEntityObject {
        public bool IsArchive { get; set; }

        public string Value { get; set; }
    }
}
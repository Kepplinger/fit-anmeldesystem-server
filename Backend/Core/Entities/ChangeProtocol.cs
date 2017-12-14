using System;
namespace Backend.Core.Entities
{
    public class ChangeProtocol
    {
        public ChangeProtocol(){}

        public string tableName { get; set; }

        public string columName { get; set; }

        public DateTime changeDate { get; set; }

        public string newValue { get; set; }

        public string oldValue { get; set; }
    }
}

using System;

namespace Backend.Core.Entities
{
    public class ChangeProtocol : EntityObject
    {
        public string TableName { get; set; }

        public string ColumName { get; set; }

        public DateTime ChangeDate { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }
    }
}

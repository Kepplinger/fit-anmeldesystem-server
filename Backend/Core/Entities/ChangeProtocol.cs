using System;

namespace Backend.Core.Entities
{
    public class ChangeProtocol : EntityObject
    {
        public string TableName { get; set; }

        public string ColumName { get; set; }

        public DateTime ChangeDate { get; set; }

        public object NewValue { get; set; }

        public object OldValue { get; set; }

        public Type TypeOfValue { get; set; }
    }
}

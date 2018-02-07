using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace Backend.Core.Entities
{
    public class ChangeProtocol : EntityObject
    {
        public string TableName { get; set; }

        public string ColumName { get; set; }

        public DateTime ChangeDate { get; set; }

        [NotMapped]
        public object NewValue { get; set; }

        [NotMapped]
        public object OldValue { get; set; }

        public Type TypeOfValue { get; set; }
    }
}

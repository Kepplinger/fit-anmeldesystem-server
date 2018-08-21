using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Backend.Utils;

namespace Backend.Core.Entities
{
    public class ChangeProtocol : TimestampEntityObject
    {
        public string TableName { get; set; }

        public string ColumnName { get; set; }

        public DateTime ChangeDate { get; set; }

        public string NewValue { get; set; }
            
        public string OldValue { get; set; }

        public int RowId { get; set; }

        public int CompanyId { get; set; }

        public bool IsPending { get; set; }

        public bool isAdminChange { get; set; }

        public bool isReverted { get; set; }
    }
}

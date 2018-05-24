using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend.Core.Contracts {
    interface ITimestampEntityObject : IEntityObject {
        /// <summary>
        /// Die Version dieses Datenbank-Objektes. Diese Version wird beim Erzeugen (Insert) 
        /// automatisch und mit jeder Änderung neu gesetzt. 
        /// </summary>
        byte[] Timestamp { get; set; }
    }
}

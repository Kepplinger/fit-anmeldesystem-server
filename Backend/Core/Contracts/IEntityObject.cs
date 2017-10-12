using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Core.Contracts
{
    /// <summary>
    /// Jede Entität muss eine Id und ein Concurrency-Feld haben
    /// Die Annotation [Timestamp] muss in der Klasse extra notiert werden.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityObject
    {
        /// <summary>
        /// Eindeutige Identitaet des Objektes.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// Die Version dieses Datenbank-Objektes. Diese Version wird beim Erzeugen (Insert) 
        /// automatisch und mit jeder Änderung neu gesetzt. 
        /// </summary>
        byte[] Timestamp { get; set; }
    }
}

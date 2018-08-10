namespace Backend.Core.Contracts {
    /// <summary>
    /// Jede Entität muss eine Id und ein Concurrency-Feld haben
    /// Die Annotation [Timestamp] muss in der Klasse extra notiert werden.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityObject {
        /// <summary>
        /// Eindeutige Identitaet des Objektes.
        /// </summary>
        int Id { get; set; }
    }
}

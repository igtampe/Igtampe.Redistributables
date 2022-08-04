namespace Igtampe.Exceptions {

    /// <summary>An exception relating to a type of identifiable object</summary>
    /// <typeparam name="E">Type of the object</typeparam>
    /// <typeparam name="F">Type of the object's ID. <b>This should be primitive, or contain some sensible ToString() method</b> (but who am I to stop you?)</typeparam>
    public class ObjectException<E,F> : Exception {

        /// <summary>ID of the object that this exception relates to</summary>
        public F ID { get; set; }

        /// <summary>Creates an object exception for an object with given ID </summary>
        /// <param name="ID">ID of the object that is related to this exception</param>
        public ObjectException(F ID) => this.ID=ID;

        /// <summary>Creates a UserException relating to the given Username, and a different message</summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public ObjectException(F ID, string? Message) : base(Message) => this.ID=ID;

        /// <summary>Message for this Exception</summary>
        public override string Message => base.Message ?? $"An unknown exception occurred relating to {nameof(E)} with ID '{ID}'";

    }
}

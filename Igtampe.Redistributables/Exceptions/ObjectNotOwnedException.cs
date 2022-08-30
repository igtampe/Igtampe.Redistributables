namespace Igtampe.Exceptions {

    /// <summary>Thrown if an entity attempts to act upon another object it does not own 
    /// (IE a user editing an object they do not own) <br/><br/>
    /// 
    /// <b>NOTE</b>: This exception and its message should only be shown to a client in circumstances where they 
    /// can view the object, but not act upon it. If a client cannot see this object and cannot act upon it, <b>obfuscate the 
    /// error and use <see cref="ObjectNotFoundException{E, F}"/> instead.</b></summary>
    /// <typeparam name="E">Type of the object</typeparam>
    /// <typeparam name="F">Type of the ID of the object</typeparam>
    public class ObjectNotOwnedException<E, F> : ObjectException<E, F> {

        /// <summary>Creates an Object Not Owned exception</summary>
        /// <param name="ID"></param>
        public ObjectNotOwnedException(F ID) : base(ID) {}

        /// <summary>Message of the exception</summary>
        public override string Message => $"{nameof(E)} with ID \'{ID}\' is not owned by executing entity";

    }
}

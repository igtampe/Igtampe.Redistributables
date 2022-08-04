namespace Igtampe.Exceptions {
    /// <summary>An Exception occurs when an object was not found, and it is required to proceed</summary>
    /// <typeparam name="E">Type of the object</typeparam>
    /// <typeparam name="F">Type of the ID of the object/></typeparam>
    public class ObjectNotFoundException<E, F> : ObjectException<E, F> {

        /// <summary>Creates an object not found exception</summary>
        /// <param name="ID"></param>
        public ObjectNotFoundException(F ID) : base(ID) {}

        /// <summary>Message relating to this exception</summary>
        public override string Message => $"{nameof(E)} with ID '{ID}' was not found!";
    }
}

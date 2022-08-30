using Igtampe.Exceptions;

namespace Igtampe.Toffee.Common.Exceptions {
    
    /// <summary>Exception thrown when an unknown issue happens relating to a Category object</summary>
    public class CategoryException : ObjectException<Category, Guid> {
    
        /// <summary>Creates a Category Exception</summary>
        /// <param name="ID"></param>
        public CategoryException(Guid ID) : base(ID) {}

        /// <summary>Creates a Category Exception with a custom message</summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public CategoryException(Guid ID, string? Message) : base(ID, Message) {}
    }
}

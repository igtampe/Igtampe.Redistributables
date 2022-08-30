using Igtampe.Exceptions;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Thrown when a Category is not Owned</summary>
    public class CategoryNotOwnedException : ObjectNotOwnedException<Category, Guid> {
        
        /// <summary>Creates a Category Not Owned Exception</summary>
        /// <param name="ID"></param>
        public CategoryNotOwnedException(Guid ID) : base(ID) {}
    }
}

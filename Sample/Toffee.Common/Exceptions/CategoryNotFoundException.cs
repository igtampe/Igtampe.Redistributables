using Igtampe.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igtampe.Toffee.Common.Exceptions {

    /// <summary>Thrown when a Category is not found</summary>
    public class CategoryNotFoundException : ObjectNotFoundException<Category, Guid> {
        
        /// <summary>Creates a Category Not Found Exception</summary>
        /// <param name="ID"></param>
        public CategoryNotFoundException(Guid ID) : base(ID) {}
    }
}

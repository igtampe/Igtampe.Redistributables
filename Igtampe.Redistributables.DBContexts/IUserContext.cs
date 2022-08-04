using Igtampe.ChopoAuth;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.DBContexts {

    /// <summary>Context for objects that are, or are derived from <see cref="ChopoAuth.User"/></summary>
    public interface IUserContext<E> where E : User {

        /// <summary>Table of all users</summary>
        public DbSet<E> User {get; set;}

        /// <summary>Apply any Include operation to sets that your user object should always contain. If there's no autoincludes, return the same set</summary>
        /// <returns></returns>
        public IQueryable<E> ApplyAutoIncludes(IQueryable<User> Set);

    }
}

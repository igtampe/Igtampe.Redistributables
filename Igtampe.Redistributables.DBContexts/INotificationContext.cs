using Igtampe.ChopoAuth;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.DBContexts {

    /// <summary>A context for objects that are, or are derivative forms of <see cref="Notification"/></summary>
    public interface INotificationContext<E,F> : IUserContext<F> where E: Notification<F> where F : User {

        /// <summary>Table of all users</summary>
        public DbSet<E> Notification {get; set;}

        /// <summary>Apply any Include operation to sets that your user object should always contain. If there's no autoincludes, return the same set</summary>
        /// <returns></returns>
        public IQueryable<E> ApplyAutoIncludes(IQueryable<E> Set) => Set;

    }
}

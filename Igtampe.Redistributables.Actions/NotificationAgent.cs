using Igtampe.ChopoAuth;
using Igtampe.ChopoSessionManager;
using Igtampe.DBContexts;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>Agent that handles actions for the Notifier system</summary>
    /// <typeparam name="E"></typeparam>
    /// <typeparam name="F"></typeparam>
    public class NotificationAgent<E, F> : SessionedActionAgent<E> where E : DbContext, INotificationContext<F> where F : User {

        /// <summary>Creates a Notification Agent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public NotificationAgent(E Context, ISessionManager Manager) : base(Context, Manager) {}

        /// <summary>Gets all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task<List<Notification>> GetAll(Guid? SessionID) {
            var S = await GetSession(SessionID);
            return await Context.Notification.Where(A => A.Owner != null && A.Owner.Username == S.Username).ToListAsync();
        }

        /// <summary>Deletes one notification from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task DeleteOne(Guid? SessionID, Guid ID) {
            var S = await GetSession(SessionID);
            
            Context.Notification.RemoveRange(Context.Notification.Where(A => A.Owner != null && A.Owner.Username == S.Username && A.ID == ID));
            await Context.SaveChangesAsync();
        }

        /// <summary>Deletes all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task DeleteAll(Guid? SessionID) {
            var S = await GetSession(SessionID);

            Context.Notification.RemoveRange(Context.Notification.Where(A => A.Owner != null && A.Owner.Username == S.Username));
            await Context.SaveChangesAsync();
        }
    }
}

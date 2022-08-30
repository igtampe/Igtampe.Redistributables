using Igtampe.ChopoAuth;
using Igtampe.ChopoSessionManager;
using Igtampe.DBContexts;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>Agent that handles actions for the Notifier system</summary>
    /// <typeparam name="E">Type of DB Context</typeparam>
    /// <typeparam name="F">Type of Notification</typeparam>
    /// <typeparam name="G">Type of User</typeparam>
    public class NotificationAgent<E, F, G> :
        SessionedActionAgent<E>
        where E : DbContext, IUserContext<G>, INotificationContext<F, G>
        where F : Notification<G>, new()
        where G : User, new() {

        private readonly AuthAgent<E, G> AuthAgent;

        /// <summary>Creates a Notification Agent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public NotificationAgent(E Context, ISessionManager Manager) : base(Context, Manager)
            => AuthAgent = new(Context, Manager);

        /// <summary>Gets all notifications from the logged in user</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        public async Task<List<F>> GetAll(Guid? SessionID) {
            var S = await GetSession(SessionID);
            return await Context.ApplyAutoIncludes(Context.Notification)
                .Where(A => A.Owner != null && A.Owner.Username == S.Username).ToListAsync();
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

        /// <summary>Sends a notification to the given username with given text</summary>
        /// <param name="Username">Username to search for and assign this notification to</param>
        /// <param name="Notif">Notification to send (without owner already set)</param>
        /// <returns></returns>
        public async Task<F> SendNotification(string Username, F Notif) {
            Notif.Owner = await AuthAgent.GetUser(Username);
            return await SendNotification(Notif);        
        }
            

        /// <summary>Sends a notification to the given user</summary>
        /// <param name="Notif">Notif to send with owner already set</param>
        /// <returns></returns>
        public async Task<F> SendNotification(F Notif) {

            //Ensure the date created is accurate
            Notif.DateCreated = DateTime.UtcNow;

            Context.Notification.Add(Notif);
            await Context.SaveChangesAsync();
            return Notif;
        }
    }
}

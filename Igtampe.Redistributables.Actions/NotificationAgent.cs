using Igtampe.ChopoAuth;
using Igtampe.ChopoSessionManager;
using Igtampe.DBContexts;
using Igtampe.Notifier;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>Agent that handles actions for the Notifier system</summary>
    /// <typeparam name="E"></typeparam>
    /// <typeparam name="F"></typeparam>
    public class NotificationAgent<E, F> : SessionedActionAgent<E> where E : DbContext, IUserContext<F>, INotificationContext<F> where F : User, new() {

        private readonly AuthAgent<E,F> AuthAgent;

        /// <summary>Creates a Notification Agent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public NotificationAgent(E Context, ISessionManager Manager) : base(Context, Manager) 
            => AuthAgent = new(Context, Manager);

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

        /// <summary>Sends a notification to the given username with given text</summary>
        /// <param name="Username"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public async Task<Notification> SendNotification(string Username, string Text) => await SendNotification(await AuthAgent.GetUser(Username), Text);
            

        /// <summary>Sends a notification to the given user</summary>
        /// <param name="User"></param>
        /// <param name="Text"></param>
        /// <returns></returns>
        public async Task<Notification> SendNotification(F User, string Text) {
            Notification N = new() { DateCreated = DateTime.UtcNow, Text = Text, Owner = User };
            Context.Notification.Add(N);
            await Context.SaveChangesAsync();
            return N;
        }
    }
}

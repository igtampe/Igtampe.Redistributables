using Igtampe.ChopoSessionManager;
using Igtampe.ChopoSessionManager.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>An Agent that can work on </summary>
    /// <typeparam name="E"></typeparam>
    public class SessionedActionAgent<E> : ActionAgent<E> where E : DbContext {

        /// <summary>Common session manager</summary>
        protected ISessionManager Manager;

        /// <summary>Creates a basic ActionAgent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public SessionedActionAgent(E Context, ISessionManager Manager) : base(Context) => this.Manager = Manager;

        /// <summary>Gets a session from the SessionManager</summary>
        /// <param name="SessionID"></param>
        /// <returns></returns>
        /// <exception cref="SessionNotFoundException"></exception>
        public async Task<Session> GetSession(Guid? SessionID) {
            Session? S = await Task.Run(() => Manager.FindSession(SessionID ?? Guid.Empty));
            return S is null ? throw new SessionNotFoundException(SessionID ?? Guid.Empty) : S;
        }
    }
}
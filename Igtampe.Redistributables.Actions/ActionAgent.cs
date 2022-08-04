using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>An Agent that can work on </summary>
    /// <typeparam name="E"></typeparam>
    public class ActionAgent<E> where E : DbContext {

        /// <summary>Data Context to act upon</summary>
        protected E Context { get; private set; }

        /// <summary>Creates a basic ActionAgent</summary>
        /// <param name="Context"></param>
        public ActionAgent(E Context) => this.Context = Context;

    }
}
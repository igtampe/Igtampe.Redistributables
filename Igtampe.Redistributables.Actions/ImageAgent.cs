using Igtampe.ChopoAuth;
using Igtampe.ChopoAuth.Exceptions;
using Igtampe.ChopoImageHandling;
using Igtampe.ChopoImageHandling.Exceptions;
using Igtampe.ChopoSessionManager;
using Igtampe.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Igtampe.Actions {

    /// <summary>Action Agent that can handle Chopo Image Handling image objects</summary>
    /// <typeparam name="E"></typeparam>
    /// <typeparam name="F"></typeparam>
    public class ImageAgent<E, F> : SessionedActionAgent<E> where E : DbContext, IImageContext, IUserContext<F> where F : User {
    
        /// <summary>Creates an ImageAgent</summary>
        /// <param name="Context"></param>
        /// <param name="Manager"></param>
        public ImageAgent(E Context, ISessionManager Manager) : base(Context, Manager) {}

        /// <summary>Gets an Image from the DB</summary>
        /// <param name="ID">ID of the image</param>
        /// <returns>The Image object</returns>
        /// <exception cref="ImageNotFoundException">Thrown if the image was not found</exception>
        public async Task<Image> GetImage(Guid ID) {
            Image? I = await Context.Image.FindAsync(ID);
            return I is null ? throw new ImageNotFoundException(ID) : I;
        }

        /// <summary>Checks the roles of a given u</summary>
        /// <param name="U">User to check the roles of</param>
        /// <returns></returns>
        /// <exception cref="UserRolesException">Thrown if the user does not meet the required roles</exception>
        protected virtual void CheckUploadRoles(F U) { }

        /// <summary>Adds a new image to the DB</summary>
        /// <param name="SessionID">ID of the session executing this request</param>
        /// <param name="I">Image to upload</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">Thrown if the Session is tied to no user</exception>
        public async Task<Image> CreateImage(Guid SessionID, Image I) {

            var S = await GetSession(SessionID);

            //Find the user
            F? U = await Context.User.FirstOrDefaultAsync(O => O.Username == S.Username);
            if (U == null) { throw new InvalidOperationException("This isn't supposed to happen"); }

            CheckUploadRoles(U);

            Context.Image.Add(I);
            await Context.SaveChangesAsync();

            return I;

        }
    }
}

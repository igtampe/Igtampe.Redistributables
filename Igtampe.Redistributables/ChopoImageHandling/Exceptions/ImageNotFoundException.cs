using Igtampe.Exceptions;

namespace Igtampe.ChopoImageHandling.Exceptions {

    /// <summary>An Exception that indicates the requested Image was not found, where one is required to proceed</summary>
    public class ImageNotFoundException : ObjectNotFoundException<Image,Guid> {

        /// <summary>Creates an ImageNotFound exception indicating the given ID was not found</summary>
        /// <param name="ID"></param>
        public ImageNotFoundException(Guid ID) : base(ID) {}
    }
}

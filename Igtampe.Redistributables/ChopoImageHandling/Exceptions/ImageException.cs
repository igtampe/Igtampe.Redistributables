using Igtampe.Exceptions;

namespace Igtampe.ChopoImageHandling.Exceptions {

    /// <summary>An Exception relating to an Image from ChopoImageHandling</summary>
    public class ImageException : ObjectException<Image,Guid> {

        /// <summary>Creates a ImageException for a notification with given ID</summary>
        /// <param name="ID"></param>
        public ImageException(Guid ID) : base(ID) { }

        /// <summary>Creates an ImageException relating to the given Image, and a different message</summary>
        /// <param name="ID"></param>
        /// <param name="Message"></param>
        public ImageException(Guid ID, string? Message) : base(ID, Message) { }
    }
}

using Igtampe.ChopoImageHandling;
using Igtampe.Controllers.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Igtampe.Controllers {

    /// <summary>Static Utilities for Controllers</summary>
    public static class ControllerUtils {

        /// <summary>Acceptable Image Formats</summary>
        public static readonly string[] ImageFormats = { 
            "image/png", "image/jpeg", 
            "image/gif","image/bmp"
        };

        /// <summary>Shorthand helper function to convert megabytes to bytes</summary>
        /// <param name="MB"></param>
        /// <returns></returns>
        public static int MegabytesToBytes(double MB) => Convert.ToInt32(1024 * 1024 * MB);

        /// <summary>Retrieves the body of an HTTPRequest as a byte array</summary>
        /// <param name="Request">The request whose body is to be retrieved</param>
        /// <param name="MaxSize">Maximum size (in megabytes) to accept from this request</param>
        /// <returns>A Byte Array of the entire body of the request</returns>
        public static async Task<byte[]> GetRequestBody(HttpRequest Request, double MaxSize = 1) {
            using var memoryStream = new MemoryStream();
            await Request.Body.CopyToAsync(memoryStream);
            byte[] Data = memoryStream.ToArray();
            return Data.Length > MegabytesToBytes(MaxSize) 
                ? throw new FileTooLargeException(MegabytesToBytes(MaxSize), Data.Length) 
                : Data;
        }

        /// <summary>Retrieves an Image object from a given HTTPRequest</summary>
        /// <param name="Request">The Request whose body is the image data</param>
        /// <param name="MaxSize">The maximum size of this image (in megabytes)</param>
        /// <returns></returns>
        public static async Task<Image> GetImageFromRequest(HttpRequest Request, double MaxSize = 1) {
            string? ContentType = Request.ContentType;
            
            if (!ImageFormats.Contains(ContentType)) { throw new UnacceptableMimeTypeException(ImageFormats,ContentType); }
            if (Request.ContentLength > MaxSize) { throw new FileTooLargeException(MegabytesToBytes(MaxSize), Convert.ToInt32(Request.ContentLength));  }

            Image I = new() {
                Type = ContentType,
                Data = await GetRequestBody(Request, MaxSize),
            };

            //Check again because we can't trust the request object
            return I.Data.Length > MegabytesToBytes(MaxSize) 
                ? throw new FileTooLargeException(MegabytesToBytes(MaxSize), Convert.ToInt32(Request.ContentLength)) 
                : I;
        }
    }
}

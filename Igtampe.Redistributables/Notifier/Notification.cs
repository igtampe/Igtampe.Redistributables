using Igtampe.ChopoAuth;
using System.Text.Json.Serialization;

namespace Igtampe.Notifier {

    /// <summary>Generic notification for <see cref="User"/></summary>
    public class Notification : Notification<User> { }

    /// <summary>A Notification to a descendant of <see cref="User"/></summary>
    public class Notification<E> : AutomaticallyGeneratableIdentifiable, Ownable<E>, Dateable where E : User {

        /// <summary>Text of this notification</summary>
        public string Text { get; set; } = "";

        /// <summary>Date and Time this notification was sent</summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>User this notification belongs to</summary>
        [JsonIgnore]
        public E? Owner { get; set; }

        /// <summary>String representation of this notification with all relevant data</summary>
        /// <returns></returns>
        public override string ToString() => $"Notification \'{ID}\' for {Owner}: [{DateCreated}] {Text}";

    }
}
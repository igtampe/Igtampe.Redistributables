using Igtampe.ChopoImageHandling;
using System.Text.Json.Serialization;

namespace Igtampe.Toffee.Common {

    /// <summary>Category for tasks</summary>
    public class Category : AutomaticallyGeneratableIdentifiable, Nameable, Describable {

        /// <summary>Name of this categotu</summary>
        public string Name { get; set; } = "";

        /// <summary>Description of this category</summary>
        public string Description { get; set; } = "";

        /// <summary>Hex color for the category</summary>
        public string Color { get; set; } = "";

        /// <summary>Creator of this category</summary>
        public User? Creator { get; set; }

        /// <summary>Tasks under this category</summary>
        [JsonIgnore]
        public List<Task>? Tasks { get; set; }

        /// <summary>Icon for this Category</summary>
        public Image? Icon { get; set; }
    }
}

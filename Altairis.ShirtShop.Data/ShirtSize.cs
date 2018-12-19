using System.ComponentModel.DataAnnotations;

namespace Altairis.ShirtShop.Data {
    public class ShirtSize {

        [Key]
        public int Id { get; set; }

        [Required]
        public int SortPriority { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required]
        public int Price { get; set; }

    }
}

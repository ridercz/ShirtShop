using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Altairis.ShirtShop.Data {
    public class ShirtSize {

        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, Range(0, 1000)]
        public int Price { get; set; }

        public int SortPriority { get; set; }

    }
}

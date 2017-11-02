using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Altairis.ShirtShop.Data {
    public class ShirtType {

        [Key]
        public int Id { get; set; }

        [Required]
        public int SortPriority { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

    }
}

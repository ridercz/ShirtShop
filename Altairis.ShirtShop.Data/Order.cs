using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Altairis.ShirtShop.Data {
    public class Order {

        [Key]
        public int OrderId { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime? DatePaid { get; set; }

        public DateTime? DateShipped { get; set; }

        [Required]
        public ShirtModel Model { get; set; }

        public int ShirtModelId { get; set; }

        [Required]
        public ShirtSize Size { get; set; }

        public int ShirtSizeId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required]
        public DeliveryMethod DeliveryMethod { get; set; }

        public int DeliveryMethodId { get; set; }

        [MaxLength(200)]
        public string DeliveryAddress { get; set; }

        [MaxLength(1000)]
        public string BuyerNotes { get; set; }

        [MaxLength(1000)]
        public string SellerNotes { get; set; }

    }
}

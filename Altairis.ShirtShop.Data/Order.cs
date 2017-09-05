using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Altairis.ShirtShop.Data {
    public class Order {

        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime? DatePaid { get; set; }

        public DateTime? DateShipped { get; set; }

        public ShirtModel ShirtModel { get; set; }

        public int ShirtModelId { get; set; }

        public ShirtSize ShirtSize { get; set; }

        public int ShirtSizeId { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [Required, MaxLength(100), EmailAddress]
        public string Email { get; set; }

        [Required, MaxLength(9), Phone, RegularExpression("^[0-9]{9}")]
        public string PhoneNumber { get; set; }

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

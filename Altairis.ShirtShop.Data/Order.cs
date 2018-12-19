using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ShirtShop.Data {
    public class Order {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        public DateTime? DatePaid { get; set; }

        public DateTime? DateSent { get; set; }

        public ShirtSize ShirtSize { get; set; }

        public int ShirtSizeId { get; set; }

        public ShirtType ShirtType { get; set; }

        public int ShirtTypeId { get; set; }

        [Required, MaxLength(50), DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

        [Required, MaxLength(50)]
        public string FullName { get; set; }

        [Required, MaxLength(50)]
        public string Street { get; set; }

        [Required, MaxLength(50)]
        public string City { get; set; }

        [Required, MaxLength(5), DataType(DataType.PostalCode)]
        public string ZipCode { get; set; }

    }
}

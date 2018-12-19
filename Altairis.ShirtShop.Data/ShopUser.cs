using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ShirtShop.Data {
    public class ShopUser : IdentityUser {

        [Required, MaxLength(50)]
        public string FullName { get; set; }

        public bool Enabled { get; set; } = true;

        public DateTimeOffset? LastLoginDate { get; set; }

    }
}

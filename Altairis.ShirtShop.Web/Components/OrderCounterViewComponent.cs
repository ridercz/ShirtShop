using System;
using System.Linq;
using System.Threading.Tasks;
using Altairis.ShirtShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Altairis.ShirtShop.Web.Components {

    public class OrderCounterViewComponent : ViewComponent {
        private readonly ShopDbContext _context;

        public OrderCounterViewComponent(ShopDbContext context) {
            this._context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int days) {
            var cutoffDate = DateTime.Today.AddDays(-days);
            var model = new OrderCounterModel {
                OrderCount = await this._context.Orders
                                .CountAsync(x => x.DateCreated >= cutoffDate),
                TotalSales = await this._context.Orders
                                .Where(x => x.DateCreated >= cutoffDate)
                                .SumAsync(x => x.ShirtSize.Price),
            };
            return this.View(model);
        }

    }

    public class OrderCounterModel {
        public int OrderCount { get; set; }
        public int TotalSales { get; set; }
    }

}

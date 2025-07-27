using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<ApplicationRole> ApplicationRoles { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<SalesOrderDetail> OrderDetails { get; set; }
        DbSet<SalesOrder> Orders { get; set; }
        DbSet<Product> Products { get; set; }
        DbSet<Vendor> Vendors { get; set; }
        DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }
        DbSet<ProductVendor> ProductVendors { get; set; }
        DbSet<Category> Categories { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
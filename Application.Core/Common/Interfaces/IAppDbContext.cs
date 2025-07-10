using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Core.Common.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<ApplicationRole> ApplicationRoles { get; set; }
        DbSet<ApplicationUser> ApplicationUsers { get; set; }
        DbSet<Customer> Customers { get; set; }
        DbSet<OrderDetail> OrderDetails { get; set; }
        DbSet<Order> Orders { get; set; }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
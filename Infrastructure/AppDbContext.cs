﻿using Application.Common.Interfaces;
using Domain;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public sealed class AppDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IAppDbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<SalesOrder> Orders { get; set; }
        public DbSet<SalesOrderDetail> OrderDetails { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<ProductVendor> ProductVendors { get; set; } 
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetails { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity's default configurations are applied

            // Configure IdentityUserLogin<Guid> with a composite primary key
            modelBuilder.Entity<IdentityUserLogin<Guid>>().HasKey(l => new { l.LoginProvider, l.ProviderKey });

            // Apply configurations from the assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            modelBuilder.ApplyConfiguration(new ResidentialCustomerConfiguration());
            modelBuilder.ApplyConfiguration(new CorporateCustomerConfiguration());
            modelBuilder.ApplyConfiguration(new GovernmentCustomerConfiguration());

            // If not already there, configure the discriminator for TPH
            modelBuilder.Entity<Customer>()
                .HasDiscriminator<string>("CustomerType")
                .HasValue<ResidentialCustomer>("Residential")
                .HasValue<CorporateCustomer>("Corporate")
                .HasValue<GovernmentCustomer>("Government");

            modelBuilder.HasSequence<int>("CustomerNumberSequence")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("ProductNumberSequence")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("VendorNumberSequence")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("SalesOrderNumberSequence")
                .StartsAt(1)
                .IncrementsBy(1);

            modelBuilder.HasSequence<int>("PurchaseOrderNumberSequence")
                .StartsAt(1)
                .IncrementsBy(1);
        }
    }    
}
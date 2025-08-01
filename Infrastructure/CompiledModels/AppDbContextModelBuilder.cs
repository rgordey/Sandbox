﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace Infrastructure.CompiledModels
{
    public partial class AppDbContextModel
    {
        private AppDbContextModel()
            : base(skipDetectChanges: false, modelId: new Guid("fcb8ae14-6cc0-4ac3-8d77-33194b05b56b"), entityTypeCount: 22)
        {
        }

        partial void Initialize()
        {
            var applicationRole = ApplicationRoleEntityType.Create(this);
            var applicationUser = ApplicationUserEntityType.Create(this);
            var category = CategoryEntityType.Create(this);
            var customer = CustomerEntityType.Create(this);
            var address = AddressEntityType.Create(this);
            var address0 = Address0EntityType.Create(this);
            var product = ProductEntityType.Create(this);
            var productVendor = ProductVendorEntityType.Create(this);
            var purchaseOrder = PurchaseOrderEntityType.Create(this);
            var purchaseOrderDetail = PurchaseOrderDetailEntityType.Create(this);
            var salesOrder = SalesOrderEntityType.Create(this);
            var salesOrderDetail = SalesOrderDetailEntityType.Create(this);
            var vendor = VendorEntityType.Create(this);
            var address1 = Address1EntityType.Create(this);
            var identityRoleClaim = IdentityRoleClaimEntityType.Create(this);
            var identityUserClaim = IdentityUserClaimEntityType.Create(this);
            var identityUserLogin = IdentityUserLoginEntityType.Create(this);
            var identityUserRole = IdentityUserRoleEntityType.Create(this);
            var identityUserToken = IdentityUserTokenEntityType.Create(this);
            var corporateCustomer = CorporateCustomerEntityType.Create(this, customer);
            var governmentCustomer = GovernmentCustomerEntityType.Create(this, customer);
            var residentialCustomer = ResidentialCustomerEntityType.Create(this, customer);

            CategoryEntityType.CreateForeignKey1(category, category);
            AddressEntityType.CreateForeignKey1(address, customer);
            Address0EntityType.CreateForeignKey1(address0, customer);
            ProductEntityType.CreateForeignKey1(product, category);
            ProductVendorEntityType.CreateForeignKey1(productVendor, product);
            ProductVendorEntityType.CreateForeignKey2(productVendor, vendor);
            PurchaseOrderEntityType.CreateForeignKey1(purchaseOrder, vendor);
            PurchaseOrderDetailEntityType.CreateForeignKey1(purchaseOrderDetail, purchaseOrder);
            PurchaseOrderDetailEntityType.CreateForeignKey2(purchaseOrderDetail, product);
            SalesOrderEntityType.CreateForeignKey1(salesOrder, customer);
            SalesOrderDetailEntityType.CreateForeignKey1(salesOrderDetail, salesOrder);
            SalesOrderDetailEntityType.CreateForeignKey2(salesOrderDetail, product);
            Address1EntityType.CreateForeignKey1(address1, vendor);
            IdentityRoleClaimEntityType.CreateForeignKey1(identityRoleClaim, applicationRole);
            IdentityUserClaimEntityType.CreateForeignKey1(identityUserClaim, applicationUser);
            IdentityUserLoginEntityType.CreateForeignKey1(identityUserLogin, applicationUser);
            IdentityUserRoleEntityType.CreateForeignKey1(identityUserRole, applicationRole);
            IdentityUserRoleEntityType.CreateForeignKey2(identityUserRole, applicationUser);
            IdentityUserTokenEntityType.CreateForeignKey1(identityUserToken, applicationUser);

            ApplicationRoleEntityType.CreateAnnotations(applicationRole);
            ApplicationUserEntityType.CreateAnnotations(applicationUser);
            CategoryEntityType.CreateAnnotations(category);
            CustomerEntityType.CreateAnnotations(customer);
            AddressEntityType.CreateAnnotations(address);
            Address0EntityType.CreateAnnotations(address0);
            ProductEntityType.CreateAnnotations(product);
            ProductVendorEntityType.CreateAnnotations(productVendor);
            PurchaseOrderEntityType.CreateAnnotations(purchaseOrder);
            PurchaseOrderDetailEntityType.CreateAnnotations(purchaseOrderDetail);
            SalesOrderEntityType.CreateAnnotations(salesOrder);
            SalesOrderDetailEntityType.CreateAnnotations(salesOrderDetail);
            VendorEntityType.CreateAnnotations(vendor);
            Address1EntityType.CreateAnnotations(address1);
            IdentityRoleClaimEntityType.CreateAnnotations(identityRoleClaim);
            IdentityUserClaimEntityType.CreateAnnotations(identityUserClaim);
            IdentityUserLoginEntityType.CreateAnnotations(identityUserLogin);
            IdentityUserRoleEntityType.CreateAnnotations(identityUserRole);
            IdentityUserTokenEntityType.CreateAnnotations(identityUserToken);
            CorporateCustomerEntityType.CreateAnnotations(corporateCustomer);
            GovernmentCustomerEntityType.CreateAnnotations(governmentCustomer);
            ResidentialCustomerEntityType.CreateAnnotations(residentialCustomer);

            var sequences = new Dictionary<(string, string), ISequence>();
            var customerNumberSequence = new RuntimeSequence(
                "CustomerNumberSequence",
                this,
                typeof(int));

            sequences[("CustomerNumberSequence", null)] = customerNumberSequence;

            var productNumberSequence = new RuntimeSequence(
                "ProductNumberSequence",
                this,
                typeof(int));

            sequences[("ProductNumberSequence", null)] = productNumberSequence;

            var purchaseOrderNumberSequence = new RuntimeSequence(
                "PurchaseOrderNumberSequence",
                this,
                typeof(int));

            sequences[("PurchaseOrderNumberSequence", null)] = purchaseOrderNumberSequence;

            var salesOrderNumberSequence = new RuntimeSequence(
                "SalesOrderNumberSequence",
                this,
                typeof(int));

            sequences[("SalesOrderNumberSequence", null)] = salesOrderNumberSequence;

            var vendorNumberSequence = new RuntimeSequence(
                "VendorNumberSequence",
                this,
                typeof(int));

            sequences[("VendorNumberSequence", null)] = vendorNumberSequence;

            AddAnnotation("Relational:Sequences", sequences);
            AddAnnotation("ProductVersion", "9.0.7");
            AddAnnotation("Relational:MaxIdentifierLength", 128);
            AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);
        }
    }
}

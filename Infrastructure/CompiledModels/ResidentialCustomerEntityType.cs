﻿// <auto-generated />
using System;
using System.Reflection;
using Domain;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

#pragma warning disable 219, 612, 618
#nullable disable

namespace Infrastructure.CompiledModels
{
    [EntityFrameworkInternal]
    public partial class ResidentialCustomerEntityType
    {
        public static RuntimeEntityType Create(RuntimeModel model, RuntimeEntityType baseEntityType = null)
        {
            var runtimeEntityType = model.AddEntityType(
                "Domain.ResidentialCustomer",
                typeof(ResidentialCustomer),
                baseEntityType,
                discriminatorProperty: "CustomerType",
                discriminatorValue: "Residential",
                propertyCount: 3);

            var firstName = runtimeEntityType.AddProperty(
                "FirstName",
                typeof(string),
                propertyInfo: typeof(ResidentialCustomer).GetProperty("FirstName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ResidentialCustomer).GetField("firstName", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                maxLength: 25);
            firstName.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var isSeniorDiscountEligible = runtimeEntityType.AddProperty(
                "IsSeniorDiscountEligible",
                typeof(bool),
                propertyInfo: typeof(ResidentialCustomer).GetProperty("IsSeniorDiscountEligible", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ResidentialCustomer).GetField("<IsSeniorDiscountEligible>k__BackingField", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                sentinel: false);
            isSeniorDiscountEligible.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            var lastName = runtimeEntityType.AddProperty(
                "LastName",
                typeof(string),
                propertyInfo: typeof(ResidentialCustomer).GetProperty("LastName", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                fieldInfo: typeof(ResidentialCustomer).GetField("lastName", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly),
                maxLength: 25);
            lastName.AddAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.None);

            return runtimeEntityType;
        }

        public static void CreateAnnotations(RuntimeEntityType runtimeEntityType)
        {
            runtimeEntityType.AddAnnotation("Relational:FunctionName", null);
            runtimeEntityType.AddAnnotation("Relational:Schema", null);
            runtimeEntityType.AddAnnotation("Relational:SqlQuery", null);
            runtimeEntityType.AddAnnotation("Relational:TableName", "Customers");
            runtimeEntityType.AddAnnotation("Relational:ViewName", null);
            runtimeEntityType.AddAnnotation("Relational:ViewSchema", null);

            Customize(runtimeEntityType);
        }

        static partial void Customize(RuntimeEntityType runtimeEntityType);
    }
}

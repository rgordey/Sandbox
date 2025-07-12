using Application.Core.Features.Customers.Queries;
using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using ArchUnitNET.Fluent;
using ArchUnitNET.xUnit;
using Domain;
using Infrastructure;

namespace ApplicationTests
{
    public sealed class ArchitectureTests
    {
        private static readonly Architecture Architecture = new ArchLoader().LoadAssemblies(
            typeof(Customer).Assembly, // Domain
            typeof(GetCustomerQuery).Assembly, // Application
            typeof(AppDbContext).Assembly // Infrastructure
        ).Build();

        [Fact]
        public void Application_ShouldNotDependOnInfrastructure()
        {
            var rule = ArchRuleDefinition.Types() // Fully qualify 'Types' to resolve the error
                .That().ResideInAssembly("Application.Core")                
                .Should().NotDependOnAny(ArchRuleDefinition.Types().That().ResideInAssembly("Infrastructure"));

            rule.WithoutRequiringPositiveResults().Check(Architecture);
        }
    }
}

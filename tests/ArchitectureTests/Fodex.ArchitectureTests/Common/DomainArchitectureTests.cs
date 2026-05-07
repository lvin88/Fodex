using FluentAssertions;
using Fodex.Common.Domain.Abstractions;
using Fodex.Common.Domain.Errors;
using NetArchTest.Rules;
using Xunit;

namespace Fodex.ArchitectureTests.Common;

public sealed class DomainArchitectureTests
{
    [Fact]
    public void Domain_ShouldNotDependOn_Application()
    {
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .Should()
            .NotHaveDependencyOn("Fodex.Common.Application")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Infrastructure()
    {
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .Should()
            .NotHaveDependencyOn("Fodex.Common.Infrastructure")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_Mediator()
    {
        // IDomainEvent is intentionally pure — the adapter lives in Application.
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .Should()
            .NotHaveDependencyOn("Mediator")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_FluentValidation()
    {
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .Should()
            .NotHaveDependencyOn("FluentValidation")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_EntityFrameworkCore()
    {
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .Should()
            .NotHaveDependencyOn("Microsoft.EntityFrameworkCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void Domain_ShouldNotDependOn_AspNetCore()
    {
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .Should()
            .NotHaveDependencyOn("Microsoft.AspNetCore")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void ConcreteDomainEvents_ShouldEndWith_DomainEvent()
    {
        // DomainEventBase is abstract — excluded. Only concrete leaf events are checked.
        var result = Types.InAssembly(AssemblyFixture.Domain)
            .That()
            .ImplementInterface(typeof(IDomainEvent))
            .And()
            .AreNotAbstract()
            .And()
            .AreNotInterfaces()
            .Should()
            .HaveNameEndingWith("DomainEvent")
            .GetResult();

        result.IsSuccessful.Should().BeTrue(because: FailingNames(result));
    }

    [Fact]
    public void Error_ShouldNotBeSealed()
    {
        // ValidationError in the Application layer inherits from Error.
        typeof(Error).IsSealed.Should().BeFalse(
            because: "ValidationError (Application layer) must be able to inherit from it");
    }

    private static string FailingNames(TestResult result) =>
        string.Join(", ", result.FailingTypes?.Select(t => t.FullName ?? t.Name) ?? []);
}

using System.Reflection;
using Fodex.Common.Application.Abstractions.Messaging;
using Fodex.Common.Domain.Abstractions;
using Fodex.Common.Infrastructure.Events;

namespace Fodex.ArchitectureTests;

internal static class AssemblyFixture
{
    internal static readonly Assembly Domain = typeof(IDomainEvent).Assembly;
    internal static readonly Assembly Application = typeof(ICommand).Assembly;
    internal static readonly Assembly Infrastructure = typeof(DomainEventNotification).Assembly;
    internal static readonly Assembly[] All = [Domain, Application, Infrastructure];
}

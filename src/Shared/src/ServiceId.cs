using System;

namespace Shared
{
    internal sealed class ServiceId : IServiceId
    {
        public string Id { get; } = $"{Guid.NewGuid():N}";
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Types
{
    public class StartupInitializer : IStartupInitializer
    {
        private readonly List<IInitializer> _initializers = new();

        public void AddInitializer(IInitializer initializer)
        {
            if (initializer is null || _initializers.Contains(initializer))
            {
                return;
            }

            _initializers.Add(initializer);
        }

        public async Task InitializeAsync()
        {
            foreach (var initializer in _initializers)
            {
                await initializer.InitializeAsync().ConfigureAwait(false);
            }
        }
    }
}

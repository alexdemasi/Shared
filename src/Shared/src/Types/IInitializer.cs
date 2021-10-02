using System.Threading.Tasks;

namespace Shared.Types
{
    public interface IInitializer
    {
        Task InitializeAsync();
    }
}

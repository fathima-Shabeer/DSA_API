using System.Collections.Generic;
using System.Threading.Tasks;

namespace DsaApi.Application.Interfaces
{
    // Note: For a real app, state management is crucial.
    // This simple service uses an in-memory stack, meaning:
    // 1. Data is lost on restart.
    // 2. It's not suitable for multiple concurrent users unless scoped or managed carefully.
    // Here, we'll use a Singleton for simplicity in this demo, meaning ONE shared stack for all requests.
    public interface IStackService
    {
        Task PushAsync(string item);
        Task<string> PopAsync();
        Task<string> PeekAsync();
        Task<int> GetCountAsync();
        Task<bool> IsEmptyAsync();
        Task ClearAsync();
        Task<IEnumerable<string>> GetAllItemsAsync(); // For viewing the stack state
    }
}
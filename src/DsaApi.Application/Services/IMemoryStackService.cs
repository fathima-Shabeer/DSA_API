using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DsaApi.Application.Interfaces;
using DsaApi.Domain.DataStructures; // Reference the Domain layer
using DsaApi.Application.Exceptions; // Custom exceptions (optional but good)

namespace DsaApi.Application.Services
{
    // Registered as Singleton in Program.cs for this demo (shared state)
    public class InMemoryStackService : IStackService
    {
        // Our actual stack data structure instance (from Domain)
        // Using string for simplicity in this example
        private readonly MyStack<string> _stack = new MyStack<string>();
        private readonly object _lock = new object(); // Simple locking for thread safety

        public Task PushAsync(string item)
        {
            if (string.IsNullOrEmpty(item))
            {
                throw new ArgumentNullException(nameof(item), "Item cannot be null or empty.");
            }

            lock (_lock)
            {
                _stack.Push(item);
            }
            return Task.CompletedTask; // Methods are sync, wrap in Task for async interface
        }

        public Task<string> PopAsync()
        {
            lock (_lock)
            {
                try
                {
                    string item = _stack.Pop();
                    return Task.FromResult(item);
                }
                catch (InvalidOperationException ex)
                {
                    // Wrap domain exceptions in application-specific ones if needed
                    // Or handle directly in controller based on status codes
                    throw new EmptyStackOperationException("Cannot pop from an empty stack.", ex);
                }
            }
        }

        public Task<string> PeekAsync()
        {
            lock (_lock)
            {
                try
                {
                    string item = _stack.Peek();
                    return Task.FromResult(item);
                }
                catch (InvalidOperationException ex)
                {
                    throw new EmptyStackOperationException("Cannot peek an empty stack.", ex);
                }
            }
        }

        public Task<int> GetCountAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_stack.Count);
            }
        }

        public Task<bool> IsEmptyAsync()
        {
            lock (_lock)
            {
                return Task.FromResult(_stack.IsEmpty);
            }
        }

        public Task ClearAsync()
        {
            lock (_lock)
            {
                _stack.Clear();
            }
            return Task.CompletedTask;
        }

        public Task<IEnumerable<string>> GetAllItemsAsync()
        {
            lock (_lock)
            {
                // Return a copy to prevent modification outside the service
                return Task.FromResult(_stack.GetItems().ToList().AsEnumerable());
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;

namespace DsaApi.Domain.DataStructures
{
    // Simple generic stack implementation using List<T> internally
    public class MyStack<T>
    {
        private readonly List<T> _items = new List<T>();

        public int Count => _items.Count;

        public bool IsEmpty => _items.Count == 0;

        public void Push(T item)
        {
            if (item == null)
            {
                // Or handle as needed - could allow nulls depending on requirements
                throw new ArgumentNullException(nameof(item), "Cannot push null onto the stack.");
            }
            _items.Add(item);
        }

        public T Pop()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot pop from an empty stack.");
            }

            T item = _items[_items.Count - 1];
            _items.RemoveAt(_items.Count - 1);
            return item;
        }

        public T Peek()
        {
            if (IsEmpty)
            {
                throw new InvalidOperationException("Cannot peek an empty stack.");
            }
            return _items[_items.Count - 1];
        }

        public void Clear()
        {
            _items.Clear();
        }

        // Optional: Get all items without modifying the stack (for inspection)
        public IEnumerable<T> GetItems()
        {
            // Return a reversed copy so the top is first, like a typical stack view
            var reversedCopy = new List<T>(_items);
            reversedCopy.Reverse();
            return reversedCopy;
        }
    }
}
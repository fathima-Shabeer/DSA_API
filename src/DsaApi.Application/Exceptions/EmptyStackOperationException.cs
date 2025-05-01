using System;

namespace DsaApi.Application.Exceptions
{
    public class EmptyStackOperationException : InvalidOperationException
    {
        public EmptyStackOperationException() : base("Operation cannot be performed on an empty stack.") { }
        public EmptyStackOperationException(string message) : base(message) { }
        public EmptyStackOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
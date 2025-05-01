using System.Collections.Generic;

namespace DsaApi.WebApi.DTOs
{
    public class StackStatusResponse
    {
        public int Count { get; set; }
        public bool IsEmpty { get; set; }
        public IEnumerable<string>? Items { get; set; } // Optional: Include items view
    }
}
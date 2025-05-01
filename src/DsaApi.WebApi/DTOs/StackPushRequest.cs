using System.ComponentModel.DataAnnotations;

namespace DsaApi.WebApi.DTOs
{
    public class StackPushRequest
    {
        [Required]
        [MinLength(1)]
        public string? Item { get; set; }
    }
}
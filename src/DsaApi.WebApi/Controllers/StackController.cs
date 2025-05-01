using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DsaApi.Application.Interfaces; // Use Application interfaces
using DsaApi.WebApi.DTOs;           // Use DTOs for requests/responses
using DsaApi.Application.Exceptions; // Handle specific exceptions
using Microsoft.Extensions.Logging; // Add Logging
using System.Collections.Generic;

namespace DsaApi.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] // Route: /api/stack
    public class StackController : ControllerBase
    {
        private readonly IStackService _stackService;
        private readonly ILogger<StackController> _logger;

        // Inject the service interface (DI)
        public StackController(IStackService stackService, ILogger<StackController> logger)
        {
            _stackService = stackService;
            _logger = logger;
        }

        // POST /api/stack/push
        [HttpPost("push")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Push([FromBody] StackPushRequest request)
        {
            // Input validation is handled by [ApiController] and model validation (Required attribute)
            // if (!ModelState.IsValid) return BadRequest(ModelState); // Automatic with [ApiController]

            _logger.LogInformation("Attempting to push item: {Item}", request.Item); // Use structured logging
            await _stackService.PushAsync(request.Item);
            _logger.LogInformation("Successfully pushed item: {Item}", request.Item);

            // Often use 204 No Content for successful actions that don't return an entity body
            return NoContent();
        }

        // DELETE /api/stack/pop (DELETE is appropriate as it removes an item)
        [HttpDelete("pop")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For empty stack
        public async Task<ActionResult<string>> Pop()
        {
            _logger.LogInformation("Attempting to pop item from stack.");
            try
            {
                string item = await _stackService.PopAsync();
                _logger.LogInformation("Successfully popped item: {Item}", item);
                return Ok(item); // Return the popped item
            }
            catch (EmptyStackOperationException ex)
            {
                _logger.LogWarning(ex, "Cannot pop from an empty stack.");
                // Return a 400 Bad Request when trying to pop from empty stack
                return BadRequest(new ProblemDetails { Title = "Cannot pop from empty stack", Detail = ex.Message });
            }
            // Catch other potential exceptions if needed
        }

        // GET /api/stack/peek
        [HttpGet("peek")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] // For empty stack
        public async Task<ActionResult<string>> Peek()
        {
            _logger.LogInformation("Attempting to peek item from stack.");
            try
            {
                string item = await _stackService.PeekAsync();
                _logger.LogInformation("Successfully peeked item: {Item}", item);
                return Ok(item); // Return the top item without removing
            }
            catch (EmptyStackOperationException ex)
            {
                _logger.LogWarning(ex, "Cannot peek an empty stack.");
                return BadRequest(new ProblemDetails { Title = "Cannot peek an empty stack", Detail = ex.Message });
            }
        }

        // GET /api/stack/status
        [HttpGet("status")]
        [ProducesResponseType(typeof(StackStatusResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<StackStatusResponse>> GetStatus()
        {
            _logger.LogInformation("Getting stack status.");
            var count = await _stackService.GetCountAsync();
            var isEmpty = await _stackService.IsEmptyAsync();
            var items = await _stackService.GetAllItemsAsync(); // Get current items for viewing

            var response = new StackStatusResponse
            {
                Count = count,
                IsEmpty = isEmpty,
                Items = items // Show the current state (top item first)
            };

            return Ok(response);
        }

        // DELETE /api/stack/clear
        [HttpDelete("clear")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Clear()
        {
            _logger.LogInformation("Clearing the stack.");
            await _stackService.ClearAsync();
            return NoContent();
        }
    }
}
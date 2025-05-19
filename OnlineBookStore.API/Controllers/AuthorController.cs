using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels.Author;
using OnlineBookStore.Bussiness.ViewModels;

namespace OnlineBookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AuthorController : BaseController
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpPost("AddAuthor")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorRequest request)
        {
            var result = await _authorService.CreateAuthorAsync(request);
            return Ok(result);
        }

        [HttpPut("EditAuthor")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorRequest request)
        {
            var result = await _authorService.UpdateAuthorAsync(id, request);
            return Ok(result);
        }

        [HttpDelete("DeleteAuthor")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var result = await _authorService.DeleteAuthorAsync(id);
            return Ok(result);
        }

        [HttpGet("GetAuthorsList")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAuthors()
        {
            var result = await _authorService.GetAllAuthorsAsync();
            return Ok(result);
        }

        [HttpGet("GetAuthorDetails")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAuthorById(int id)
        {
            var result = await _authorService.GetAuthorByIdAsync(id);
            return Ok(result);
        }
    }
}

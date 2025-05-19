using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Bussiness.IService;
using OnlineBookStore.Bussiness.ViewModels;
using OnlineBookStore.Bussiness.ViewModels.BorrowBooks;

namespace OnlineBookStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class BorrowBooksController : BaseController
    {
        private readonly IBorrowBooks _borrowBooks;

        public BorrowBooksController(IBorrowBooks borrowBooks)
        {
            _borrowBooks = borrowBooks;
        }

        [HttpPost("BorrowBook")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookRequest request)
        {
            var result = await _borrowBooks.BorrowBookAsync(request);
            return Ok(result);
        }

        [HttpPost("ReturnBook")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookRequest request)
        {
            var result = await _borrowBooks.ReturnBookAsync(request);
            return Ok(result);
        }
    }
}

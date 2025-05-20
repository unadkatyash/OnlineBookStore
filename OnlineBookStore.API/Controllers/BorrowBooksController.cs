using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineBookStore.Business.IService;
using OnlineBookStore.Business.ViewModels;
using OnlineBookStore.Business.ViewModels.BorrowBooks;

namespace OnlineBookStore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowBooksController : BaseController
    {
        private readonly IBorrowBooksService _borrowBooks;

        public BorrowBooksController(IBorrowBooksService borrowBooks)
        {
            _borrowBooks = borrowBooks;
        }

        [Authorize]
        [HttpPost("BorrowBook")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> BorrowBook([FromBody] BorrowBookRequest request)
        {
            var result = await _borrowBooks.BorrowBookAsync(request);
            return Ok(result);
        }

        [Authorize]
        [HttpPost("ReturnBook")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReturnBook([FromBody] ReturnBookRequest request)
        {
            var result = await _borrowBooks.ReturnBookAsync(request);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllBorrowRecords")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllBorrowRecords([FromBody] BorrowRecordFilterRequest filter)
        {
            var result = await _borrowBooks.GetAllBorrowRecordsAsync(filter);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("GetAllPaymentSummaries")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllPaymentSummaries([FromBody] PaymentSummaryFilterRequest filter)
        {
            var result = await _borrowBooks.GetAllPaymentSummariesAsync(filter);
            return Ok(result);
        }
    }
}

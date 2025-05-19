using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookStore.Common.Constants
{
    public class CommonMessage
    {
        public const string InvalidRequest = "Invalid request data.";
        public const string InvalidCredentials = "Login failed. Ensure your credentials are correct.";
        public const string LoginSuccessful = "Login successful.";
        public const string UserNotExist = "User does not exist.";
        public const string Message_SuccessSave = "{0} saved successfully!";
        public const string AlreadyExists = "{0} already exists.";
        public const string UnauthorizedAccess = "You are not authorized to perform this action.";
        public const string MoreThan0 = "{0} must be greater than 0.";
        public const string UserInactiveOrDeleted = "The user might be inactive or deleted. Please contact the administrator.";
        public const string EmailAssociatedWithDeletedUser = "This email is associated with a deleted or inactive account. Please contact the administrator.";

        public const string Created = "{0} created successfully.";
        public const string Updated = "{0} updated successfully.";
        public const string Deleted = "{0} deleted successfully.";
        public const string NotFound = "{0} not found.";
        public const string DataFetched = "{0} retrieved successfully.";

        public const string MemberHasBorrowedBooks = "Cannot delete member. Books are currently borrowed.";
        public const string MemberListFetched = "Member list retrieved successfully.";

        public const string BookOutOfStock = "Book is currently out of stock.";
        public const string FailedToFatchLimit = "Failed to fetch borrow limit from settings.";
        public const string ReachedLimit = "You have reached your borrowing limit of {0} books.";
        public const string ReturnSuccessfully = "Book returned successfully.";
        public const string CannotDeleteBookBorrowed = "Cannot delete the book.It is currently borrowed by:";
        public const string CannotDeleteAuthorWithBooks = "Cannot delete the author. The following books are associated: {0}";

    }
}

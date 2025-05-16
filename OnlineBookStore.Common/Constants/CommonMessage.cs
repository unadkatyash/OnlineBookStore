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

        public const string Created = "{0} created successfully.";
        public const string Updated = "{0} updated successfully.";
        public const string Deleted = "{0} deleted successfully.";
        public const string NotFound = "{0} not found.";
        public const string DataFetched = "{0} retrieved successfully.";

    }
}

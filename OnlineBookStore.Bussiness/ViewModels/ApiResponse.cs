using System.Net;

namespace OnlineBookStore.Business.ViewModels
{
    public class ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        public ApiResponse()
        {
            Code = (int)HttpStatusCode.OK;
            Data = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="messages">The messages.</param>
        public ApiResponse(HttpStatusCode code, List<string> messages)
        {
            Code = (int)code;
            Messages = messages;
            Data = new object();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse" /> class.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <param name="messages">The messages.</param>
        /// <param name="data">Data.</param>
        public ApiResponse(HttpStatusCode code, List<string> messages, dynamic data)
        {
            Code = (int)code;
            Messages = messages;
            Data = data;
        }

        public int Code { get; set; }
        public List<string> Messages { get; set; } = new List<string>();
        public dynamic Data { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Utilities
{
    public class ServiceResponse
    {
        public HttpStatusCode ResponseType { get; set; }
        public string Message { get; set; }

        public ServiceResponse(HttpStatusCode responseType)
        {
            ResponseType = responseType;
        }

        public ServiceResponse(HttpStatusCode responseType, string message)
        {
            ResponseType = responseType;
            Message = message;
        }
    }

    public class ServiceResponse<T>
    {
        public HttpStatusCode ResponseType { get; set; }
        public string Message { get; set; }
        public T ResponseContent { get; set; }

        public ServiceResponse(HttpStatusCode responseType)
        {
            ResponseType = responseType;
        }

        public ServiceResponse(HttpStatusCode responseType, string message)
        {
            ResponseType = responseType;
            Message = message;
        }

        public ServiceResponse(HttpStatusCode responseType, T responseContent)
        {
            ResponseType = responseType;
            ResponseContent = responseContent;
        }

        public ServiceResponse(HttpStatusCode responseType, string message, T responseContent)
        {
            ResponseType = responseType;
            Message = message;
            ResponseContent = responseContent;
        }
    }
}

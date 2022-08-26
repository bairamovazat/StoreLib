using System;
using System.Net;

namespace StoreLib.Exceptions
{
    public class NotFoundException : HttpException
    {

        public NotFoundException()
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }
        
        public NotFoundException(string endpoint, int httpStatusCode, string responseBody)
            : base(endpoint, httpStatusCode, responseBody)
        {
            
        }

        public NotFoundException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
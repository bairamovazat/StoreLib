using System;

namespace StoreLib.Exceptions
{
    public class TimeOutException : HttpException
    {
        public TimeOutException()
        {
        }

        public TimeOutException(string message)
            : base(message)
        {
        }
        
        public TimeOutException(string endpoint, int httpStatusCode, string responseBody)
            : base(endpoint, httpStatusCode, responseBody)
        {
            
        }

        public TimeOutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
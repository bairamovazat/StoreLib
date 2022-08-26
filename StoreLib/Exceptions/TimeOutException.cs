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

        public TimeOutException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
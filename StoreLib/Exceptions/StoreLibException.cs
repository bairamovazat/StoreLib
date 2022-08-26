using System;

namespace StoreLib.Exceptions
{
    public abstract class StoreLibException : Exception
    {
        protected StoreLibException()
        {
        }

        protected StoreLibException(string message)
            : base(message)
        {
        }

        protected StoreLibException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
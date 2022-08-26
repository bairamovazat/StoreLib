using System;
using System.Net;

namespace StoreLib.Exceptions
{
    public class HttpException : StoreLibException
    {
        private string Endpoint { get; set; }
        private int HttpStatusCode { get; set; }
        private string ResponseBody { get; set; }


        public HttpException()
        {
        }

        public HttpException(string message)
            : base(message)
        {
        }

        public HttpException(string message, string endpoint, int httpStatusCode, string responseBody)
            : base(message)
        {
            this.Endpoint = endpoint;
            this.HttpStatusCode = httpStatusCode;
            this.ResponseBody = responseBody;
        }
        public HttpException(string endpoint, int httpStatusCode, string responseBody)
            : base($"Endpoint: {endpoint}. Status code: {httpStatusCode}. Response: {responseBody}")
        {
            this.Endpoint = endpoint;
            this.HttpStatusCode = httpStatusCode;
            this.ResponseBody = responseBody;
        }

        public HttpException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
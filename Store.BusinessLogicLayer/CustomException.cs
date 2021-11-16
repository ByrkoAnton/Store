using System;
using System.Collections.Generic;
using System.Net;

namespace Store.BusinessLogicLayer
{
    public class CustomException : Exception
    {
        public List<string> ErrorList { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public CustomException(string error, HttpStatusCode statusCode)
        {
            ErrorList = new List<string>() {error};
            StatusCode = statusCode;
        }

        public CustomException(List<string> errorList, HttpStatusCode statusCode)
        {
            ErrorList = errorList;
            StatusCode = statusCode;
        }
    }
}

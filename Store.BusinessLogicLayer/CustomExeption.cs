using System;
using System.Collections.Generic;
using System.Net;

namespace Store.BusinessLogicLayer
{
    public class CustomExeption : Exception
    {
        public List<string> ErrorList { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public CustomExeption(string error, HttpStatusCode statusCode)
        {
            ErrorList = new List<string>() {error};
            StatusCode = statusCode;
        }

        public CustomExeption(List<string> errorList, HttpStatusCode statusCode)
        {
            ErrorList = errorList;
            StatusCode = statusCode;
        }
    }
}

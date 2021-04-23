using System;
using System.Collections.Generic;

namespace Store.BusinessLogicLayer
{
    public class CustomExeption : Exception
    {
        public List<string> ErrorList { get; set; }
        public int StatusCode { get; set; }

        public CustomExeption(string error, int statusCode)
        {
            ErrorList = new List<string>() {error};
            StatusCode = statusCode;
        }

        public CustomExeption(List<string> errorList, int statusCode)
        {
            ErrorList = errorList;
            StatusCode = statusCode;
        }
    }
}

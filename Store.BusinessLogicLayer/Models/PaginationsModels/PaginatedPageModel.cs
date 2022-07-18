using Store.Sharing.Constants;
using System;

namespace Store.BusinessLogicLayer.Models.PaginationsModels//TODO wrong spelling
{
    public class PaginatedPageModel
    {
        public int PageNumber { get; private set; }//TODO why private set?
        public int TotalPages { get; private set; }//TODO why private set?

        public PaginatedPageModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public bool HasPreviousPage//TODO please simplify HasPreviousPage=>PageNumber > Constants.Variables.ONE_PAGE;
        {
            get => PageNumber > Constants.Variables.ONE_PAGE;
        }
        public bool HasNextPage//TODO please simplify 
        {
            get => PageNumber < TotalPages;
        }
    }
}


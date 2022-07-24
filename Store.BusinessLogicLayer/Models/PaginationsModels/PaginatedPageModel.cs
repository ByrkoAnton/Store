using Store.Sharing.Constants;
using System;

namespace Store.BusinessLogicLayer.Models.PaginationsModels//TODO wrong spelling---
{
    public class PaginatedPageModel
    {
        public int PageNumber { get;}//TODO why private set?+++
        public int TotalPages { get;}//TODO why private set?+++

        public PaginatedPageModel(int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        }
        public bool HasPreviousPage => PageNumber > Constants.PaginationParams.CURRENT_OR_ONE_PAGE;//TODO please simplify HasPreviousPage=>PageNumber > Constants.Variables.ONE_PAGE;+++
        
        public bool HasNextPage => PageNumber < TotalPages;//TODO please simplify ++++
       
    }
}


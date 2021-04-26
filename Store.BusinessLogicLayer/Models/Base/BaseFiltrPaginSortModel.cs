using Store.Sharing.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Models.Base
{
    public class BaseFiltrPaginSortModel
    {
        public long? Id { get; set; }
        public string PropForSort { get; set; }
        public bool IsAsc { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public BaseFiltrPaginSortModel()
        {
            IsAsc = true;
            CurrentPage = Constants.PaginationParams.CURRENT_PAGE;
            PageSize = Constants.PaginationParams.PAGE_SIZE;
        }
    }
}


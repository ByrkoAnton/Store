using Store.BusinessLogicLayer.Models.Authors;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;
using Type = Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums.Type;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class EditionFiltPaginSortModel
    {
        public long? Id { get; set; }
        public string Description { get; set; }
        public double? Prise { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency? Currency { get; set; }
        public Type? Type { get; set; }
        public string AuthorName { get; set; }
        public string PropForSort { get; set; }
        public bool IsAsc { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public EditionFiltPaginSortModel()
        {
            IsAsc = true;
            CurrentPage = Constants.Constants.PaginationParams.CURRENT_PAGE;
            PageSize = Constants.Constants.PaginationParams.PAGE_SIZE;
            PropForSort = Constants.Constants.SortingParams.EDITION_DEF_SORT_PROP;
        }
    }

}

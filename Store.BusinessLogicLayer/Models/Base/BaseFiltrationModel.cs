using Store.Sharing.Constants;

namespace Store.BusinessLogicLayer.Models.Base
{
    public class BaseFiltrationModel
    {
        public long? Id { get; set; }
        public string PropertyForSort { get; set; }
        public bool IsAscending { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

        public BaseFiltrationModel()
        {
            IsAscending = true;
            CurrentPage = Constants.PaginationParams.CURRENT_OR_ONE_PAGE;
            PageSize = Constants.PaginationParams.PAGE_SIZE;
        }
    }
}


using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Models.PaginationsModels//TODO wrong spelling+++
{
    public class NavigationModelBase<T>
    {
        public IEnumerable<T> Models { get; set; }
        public PaginatedPageModel PageModel { get; set; }
    }
}

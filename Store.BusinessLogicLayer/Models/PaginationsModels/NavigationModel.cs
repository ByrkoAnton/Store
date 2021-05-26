using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Models.PaginationsModels
{
    public class NavigationModel<T>
    {
        public IEnumerable<T> Models { get; set; }
        public PaginatedPageModel PageModel { get; set; }
    }
}

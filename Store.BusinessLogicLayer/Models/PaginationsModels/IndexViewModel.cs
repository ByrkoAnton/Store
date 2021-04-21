using System.Collections.Generic;

namespace Store.BusinessLogicLayer.Models.PaginationsModels
{
    public class IndexViewModel<T>
    {
        public IEnumerable<T> EntitiModels { get; set; }
        public PageViewModel PageViewModel { get; set; }
    }
}

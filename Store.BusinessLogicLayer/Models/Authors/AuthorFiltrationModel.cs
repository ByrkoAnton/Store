using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;

namespace Store.BusinessLogicLayer.Models.Authors
{
    public class AuthorFiltrationModel : BaseFiltrationModel
    {
        public string Name { get; set; }
        public string EditionDescription { get; set; }

        public AuthorFiltrationModel()
        {
            PropertyForSort = Constants.SortingParams.AUTHOR_DEF_SORT_PROP;
        }
    }
}

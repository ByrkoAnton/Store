using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;
using Type = Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums.Type;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class EditionFiltrPaginSortModel : BaseFiltrPaginSortModel
    {
        public string Description { get; set; }
        public double? Prise { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency? Currency { get; set; }
        public Type? Type { get; set; }
        public string AuthorName { get; set; }

        public EditionFiltrPaginSortModel()
        {
            PropForSort = Constants.SortingParams.EDITION_DEF_SORT_PROP;
        }
    }

}

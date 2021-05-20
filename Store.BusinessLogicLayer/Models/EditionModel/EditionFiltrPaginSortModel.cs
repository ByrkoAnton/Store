using Store.BusinessLogicLayer.Models.Base;
using Store.Sharing.Constants;
using static Store.DataAccessLayer.Enums.Enums;

namespace Store.BusinessLogicLayer.Models.EditionModel
{
    public class EditionFiltrPaginSortModel : BaseFiltrPaginSortModel
    {
        public string Description { get; set; }
        public double? Prise { get; set; }
        public int? MinPrise { get; set; }
        public int? MaxPrise { get; set; }
        public bool? IsRemoved { get; set; }
        public string Status { get; set; }
        public Currency? Currency { get; set; }
        public PrintingEditionType? EditionType { get; set; }
        public string AuthorName { get; set; }

        public EditionFiltrPaginSortModel()
        {
            PropertyForSort = Constants.SortingParams.EDITION_DEF_SORT_PROP;
        }
    }

}

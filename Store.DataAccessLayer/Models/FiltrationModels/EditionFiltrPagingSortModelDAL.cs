using Store.DataAccessLayer.Entities;
using System.Collections.Generic;
using static Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums;
using Type = Store.DataAccessLayer.Enums.Enums.PrintingEditionEnums.Type;

namespace Store.DataAccessLayer.FiltrationModels
{
    public class EditionFiltrPagingSortModelDAL
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
    }
}

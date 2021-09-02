namespace Store.DataAccessLayer.Models.FiltrationModels
{
   public class BaseFiltrationModelDAL
    {
        public long? Id { get; set; }
        public string PropertyForSort { get; set; }
        public bool IsAscending { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }

    }
}

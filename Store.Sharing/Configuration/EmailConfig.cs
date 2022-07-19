namespace Store.Sharing.Configuration //TODO wrong namespace +++
{
   public class EmailConfig
    {
        public string NameFrom { get; set; }
        public string AdressFrom { get; set; }//TODO wrong spelling "AddressFrom"+++
        public string NameTo { get; set; }
        public string Smtp { get; set; }
        public string EmailPassword { get; set; }
    }
}

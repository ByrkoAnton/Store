using Store.BusinessLogicLayer.Models.Base;


namespace Store.BusinessLogicLayer.Models.Users
{
    public class UserUpdateModel: BaseModel
    { 
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Role { get; set; }    
    }
}

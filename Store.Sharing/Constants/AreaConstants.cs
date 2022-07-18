//TODO extra lines
//TODO extra lines
namespace Store.Sharing.Constants
{
    public partial class Constants
    {
        public class AreaConstants
        {
            public const string JWT = "jwt";
            public const string ACTION_NAVIGATION = "Navigation";
            public const string CONTROLLER_ADMIN_ACCount = "AdminAccount";
            public const string ACTION_SIGN_IN = "SignIn";
            public const string ACTION_GETPROFILE = "GetProfile";
            public const string VIEW_PROFILE = "Profile";
            public const string VIEW_ADD_AUTHOR = "AddNewAuthor"; //TODO never used constant
            public const string VIEW_ADD_EDITION = "AddNewEdition"; //TODO never used constant
            public const string VIEW_AUTHORS = "Authors";
            public const string VIEW_USERS = "Users";
            public const string VIEW_EDITIONS = "Editions";
            public const string VIEW_ORDERS = "Orders";
            public const string VIEW_ORDER_PROFILE = "OrderProfile";
            public const string VIEW_UPDATE_EDITION = "UpdateEdition";
            public const string VIEW_EDITION_PROFILE = "EditionProfile";
            public const string VIEW_AUTHOR_PROFILE = "AuthorProfile";
            public const string VIEW_PROFILE_UPDATE = "ProfileUpdate";
            public const string AUTHOR_DEF_SORT_PARAMS = "Name";
            public const string USER_DEF_SORT_PARAMS = "LastName";
            public const string USER_L_NAME_COOKIES = "lastNameForSearch";
            public const string USER_F_NAME_COOKIES = "firstNameForSearch";
            public const string USER_EMAIL_COOKIES = "emailForSearch";
            public const string ORDER_DEF_SORT_PARAMS = "Id";
            public const string EDITION_DEF_SORT_PARAMS = "Price";
            public const string AUTHOR_NAME_COOKIES = "authorNameForSearch";
            public const string EDITION_TITLE_COOKIES = "editionTitleForSearch";
            public const string EDITION_AUTHOR_COOKIES = "editionsAuthorForSearch"; //TODO never used constant
            public const string USER_ID_COOKIES = "UserIdInOrder";
            public const string EDITION_ID_COOKIES = "editionIdForSearch"; //TODO never used constant
            public const string EDITION_DESC_COOKIES = "descriptionForSearch";
            public const string SPACE_IN_MODEl = " ";
            public const string WRONG_AUTHORS_MESSAGE = "At first add this authors to DB:";
            public const string WRONG_AUTHORS_DELIMETR_MESSAGE = "Wrong delimetr in model";
            public const char DELIMETR_IN_MODEL = ','; //TODO same constant
            public const string WRONG_AUTHORS_DELIMETR = ", "; //TODO same constant you can use ',' with .Trim() method
            public const string PATH = "Referer";
            public const string RESET_PESSWORD_MSG = "New password send to email";
            public const string AREAS_STYLES_PATH = "./Areas/Administration/Styles";
            public const string AREAS_STYLES_SHORT_PATH = "/Styles";
            public const string AREAS_VIEVS_PATH = "./Areas/Administration/Views";
            public const string AREAS_VIEWS_SHORT_PATH = "/Views";
            public const string AREA_NAME = "MyArea";
            public const string AREA_PATTERN = "{area:exists}/{controller=Home}/{action=Index}/{id?}";
            public const string EDITION_CREATE_MSG = " successfully created ";
            public const int FIRST_PAGE = 1;
        }
    }
}

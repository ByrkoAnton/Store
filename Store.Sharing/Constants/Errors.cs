namespace Store.Sharing.Constants
{
    public partial class Constants
    {
        public class Error
        {
            public const string ERROR = "/Error";
            public const string NO_ORDERS_IN_DB = "no any orders in DB";
            public const string NO_ORDERS_THIS_ID = "no any orders in DB with this ID";
            public const string NO_ANY_PROP_NAME = "no any property with this name";
            public const string WRONG_CONDITIONS_EDITION = "no any editions in db with this conditions";
            public const string WRONG_CONDITIONS_ORDER = "no any orders in db with this conditions";
            public const string ADD_AUTHOR_FIRST = "Add author in DB at fitst. Then add edition";
            public const string NO_AUTHOR = "printing edition must to have any author";
            public const string NO_TITLE = "printing edition must to have title";
            public const string EDITION_NOT_FOUND = "edition with this id not found";
            public const string EDITION_EXISTS_DB = "printing edition with this name already exists in db";
            public const string NO_EDITION_IN_DB = "no any printing edition in db with this id";
            public const string NO_AUTHOR_WITH_CONDITIONS = "no any author in db with this conditions";
            public const string NO_AUTHOR_ID = "no author with this id db";
            public const string AUTHOR_REMOVE_FAILD = "author remove faild - no in db";
            public const string AUTHOR_CREATE_FAILD = "author creation faild - author alredy exists in db";
            public const string PASSWORD_RESET_FAILD = "password reset faild contact admin";
            public const string WRONG_PASSWORD = "Wrong current password";
            public const string PASSWORD_RESET_FAILD_NO_USER = "password reset faild no user with this email";
            public const string WRONG_MODEL = "wrong model";
            public const string LOGIN_FAILD_MODEL = "login faild - model is NULL";
            public const string LOGIN_FAILD_EMAIL = "login faild - no user with this email";
            public const string LOGIN_FAILD_WRONG_PASSWORD = "login faild - wrong password";
            public const string NO_USERROLE = "login faild - no userRole";
            public const string ROLE_EXISTS = "role did not added - role already exists";
            public const string REGISRATION_FAILD_NO_IMAIL = "regisration faild no imail in model";
            public const string REGISRATION_FAILD_NO_PASSWORD = "regisration faild no password in model"; 
            public const string REGISRATION_FAILD_NO_FIRST_NAME = "regisration faild no first name in model";
            public const string REGISRATION_FAILD_NO_LAST_NAME = "regisration faild no last name in model";
            public const string REGISRATION_USER_NOT_CREATED = "regisration faild user did not created";
            public const string NO_USER_ID_IN_DB = "no user with this id";
            public const string REGISRATION_FAILD_NAME_IN_USE = "regisration faild this name is already in use";
            public const string REGISRATION_FAILD_THIS_EMAIL_IS_ALREADY_IN_USE = "regisration faild this email is already in use";
            public const string USER_ROLE_DID_NOT_ADDED = "user role did not added";
            public const string EMAILCONFIRM_CODE_NULL = "emailConfirmationModel - code is null";
            public const string EMAIL_EXIST_DB = "This email in use";
            public const string EMAILCONFIRM_NO_USER = "emailConfirmation - user not found";
            public const string EMAIL_DID_NOT_CONFIRMED = "emailConfirmation - email did not confirmed";
            public const string USER_NOT_FOUND = "update user faild user not found";
            public const string NO_USER_THIS_CONDITIONS = "no user in DB with this conditions";
            public const string CONTACT_ADMIN = "update user faild contact admin";
            public const string DELETE_FAILD_NO_USER = "delete user faild no user id in db";
            public const string DELETE_USER_FAILD = "delete user faild contact admin";
            public const string BLOCKING_FAILD_NO_USER = "blocking user faild no user id in db"; 
            public const string ADD_TO_ROLE_FAILD_NO_USER = "Add user in role faild no user id in db";
            public const string ROLE_IS_NOT_PROVIDED = "Add user in role faild role is not provided";
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.BusinessLogicLayer.Constants
{
    public partial class Constants
    {
        public class Error
        {
            public const string PASSWORD_RESET_FAILD_CONTACT_ADMIN = "password reset faild contact admin";
            public const string PASSWORD_RESET_FAILD_NO_USER_WITH_THIS_EMAIL = "password reset faild no user with this email";
            public const string LOGIN_FAILD_MODELI_IS_NULL = "login faild - model is NULL";
            public const string LOGIN_FAILD_MODEL_IS_NOT_CORECT = "login faild - model is not corect";
            public const string PASSWORD_FAILD_MODEL_IS_NOT_CORECT = "password faild - model is not corect";
            public const string LOGIN_FAILD_NO_USER_WITH_THIS_EMAIL = "login faild - no user with this email";
            public const string LOGIN_FAILD_WRONG_PASSWORD = "login faild - wrong password";
            public const string ERROR_NO_USERROLE = "login faild - no userRole";
            public const string ROLE_ALREADY_EXISTS = "role did not added - role already exists";
            public const string REGISTRATION_FAILD_MODELI_IS_NULL = "registration faild - model is NULL";
            public const string REGISRATION_FAILD_NO_IMAIL_IN_MODEL = "regisration faild no imail in model";
            public const string REGISRATION_FAILD_NO_PASSWORD_IN_MODEL = "regisration faild no password in model";
            public const string REGISRATION_FAILD_NO_PASSWORD_CONFIRMATION_IN_MODEL = "regisration faild no password confirmation in model";
            public const string REGISRATION_FAILD_PASSWORD_AND_PASSWORDCONFIRMATION_ARE_NOt_EQUAL = "regisration faild password and passwordconfirmation are not equal";
            public const string REGISRATION_FAILD_NO_FIRST_NAME_IN_MODEL = "regisration faild no first name in model";
            public const string REGISRATION_FAILD_NO_LAST_NAME_IN_MODEL = "regisration faild no last name in model";
            public const string REGISRATION_FAILD_NO_NAME_IN_MODEL = "regisration faild no name in model";
            public const string REGISRATION_FAILD_USER_DID_NOT_CREATED = "regisration faild user did not created";
            public const string REGISRATION_FAILD_THIS_NAME_IS_ALREADY_IN_USE = "regisration faild this name is already in use";
            public const string REGISRATION_FAILD_THIS_EMAIL_IS_ALREADY_IN_USE = "regisration faild this email is already in use";
            public const string USER_ROLE_DID_NOT_ADDED = "user role did not added";
            public const string EMAILCONFIRMATION_EMAIL_IS_NULL = "emailConfirmation - email is null";
            public const string EMAILCONFIRMATION_CODE_IS_NULL = "emailConfirmationModel - code is null";
            public const string EMAILCONFIRMATION_USER_NOT_FOUND = "emailConfirmation - user not found";
            public const string EMAILCONFIRMATION_EMAIL_DID_NOT_CONFIRMED = "emailConfirmation - email did not confirmed";
            public const string UPDATE_USER_FAILD_USER_NOT_FOUND = "update user faild user not found";
            public const string UPDATE_USER_FAILD_UPDATE_MODEL_NULL = "update user faild update model null";
            public const string UPDATE_USER_FAILD_CONTACT_ADMIN = "update user faild contact admin";
            public const string UPDATE_USER_PASSWORD_FAILD_CONTACT_ADMIN = "update user password faild contact admin";
            public const string DELETE_USER_FAILD_NO_USER_ID_IN_DB = "delete user faild no user id in db";
            public const string DELETE_USER_FAILD_CONTACT_ADMIN = "delete user faild contact admin";
            public const string BLOCKING_USER_FAILD_NO_USER_ID_IN_DB = "blocking user faild no user id in db";
            public const string BLOCKING_USER_FAILD_CONTACT_ADMIN = "blocking user faild contact admin";
            public const string UNBLOCKING_USER_FAILD_CONTACT_ADMIN = "Unblocking user faild contact admin";
            public const string UNBLOCKING_USER_FAILD_NO_USER_ID_IN_DB = "Unblocking user faild no user id in db";
            public const string GET_USER_ROLE_FAILD_NO_USER_ID_IN_DB = "Get user role faild no user id in db";
            public const string GET_USER_ROLE_FAILD_CONTACT_ADMIN = "Get user role faild contact admin";
            public const string GET_USER_ROLE_FAILD_NO_USERROLE = "Get user role faild no userrole";
            public const string ADD_USER_TO_ROLE_FAILD_NO_USER_ID_IN_DB = "Add user in role faild no user id in db";
            public const string ADD_USER_TO_ROLE_FAILD_ROLE_IS_NOT_PROVIDED = "Add user in role faild role is not provided";
            public const string IS_USER_IN_ROLE_FAILD_NO_USER_ID_IN_DB = "is user in role faild no user id in db";
        }
    }
}

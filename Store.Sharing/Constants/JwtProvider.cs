namespace Store.Sharing.Constants
{
    public partial class Constants
    {
        public class JwtProvider
        {
            public const string BEARER = "Bearer";
            public const string ID = "Id";
            public const string METHOD_NAME = "SignInAsync";
            public const string KEY = "Jwt:Key";
            public const string ISSUER = "Jwt:Issuer";//TODO never used constant
            public const string AUDIENCE = "Jwt:AUDIENCE";//TODO never used constant
            public const string LIFETIME = "Jwt:LIFETIME";//TODO never used constant
            public const string JWT_SECTION = "Jwt";
        }
    }
}

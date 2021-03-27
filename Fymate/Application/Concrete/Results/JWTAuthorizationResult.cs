using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Core.Base.Models;

namespace Core.Concrete.Models
{
    public class JWTAuthorizationResult : Result
    {
        internal JWTAuthorizationResult(bool succeeded, IEnumerable<string> errors) : base(succeeded, errors)
        { }

        public string Token { get; set; }


        new public static JWTAuthorizationResult Success(string token)
        {
            return new JWTAuthorizationResult(true, new string[] { }) { Token = token };
        }

        new public static JWTAuthorizationResult Failure(IEnumerable<string> errors)
        {
            return new JWTAuthorizationResult(false, errors);
        }
    }
}

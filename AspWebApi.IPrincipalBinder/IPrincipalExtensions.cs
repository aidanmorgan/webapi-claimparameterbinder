using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AspWebApi.IPrincipalBinder
{
    public static class PrincipalExtensions
    {
        public static Claim GetClaim(this IPrincipal principal, string name)
        {
            ClaimsPrincipal claims = principal as ClaimsPrincipal;
            return claims?.Claims.FirstOrDefault(x => string.Equals(x.Type, name, StringComparison.OrdinalIgnoreCase));
        }
    }
}

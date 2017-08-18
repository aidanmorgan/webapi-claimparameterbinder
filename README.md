# webapi-claimparameterbinder

This is a simple project that allows you to bind values from the *IPrincipal* to arguments to your controller method.

Simply mark method arguments on your controller methods with the *[FromPrinicpal]* attribute to have then load their corresponding value from the *IPrinicpal*.


## Implementing IFromPrincipalConverter

To allow multiple values to be extracted from the *IPrincipal*, or to allow the conversion of Claim types to complex objects you must provide an implementation of *IFromPrincipalConverter* when defining the attribute.

For example, to create an Application User object from claims in a JWT that provide a id and role claim you would implement:

```
    public class JwtUserPrincipalConverter : IFromPrincipalConverter
    {

        public object FromPrincipal(IPrincipal p)
        {
            var userId = GetClaim(p, "id")?.Value;
            var role = GetClaim(p, "role")?.Value;

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return JwtUser.Create(userId, role);
        }

        private static Claim GetClaim(IPrincipal contextUser, string name)
        {
            ClaimsPrincipal claims = contextUser as ClaimsPrincipal;
            return claims?.Claims.FirstOrDefault(x => string.Equals(x.Type, name, StringComparison.OrdinalIgnoreCase));
        }
    }

```

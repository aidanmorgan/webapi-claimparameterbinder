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

## Creating Custom Attributes to Reduce Noise

Specifying the Attribute repeatedly with the type of the converter can get annoying quickly, so it is recommended that you extend *FromPrincipalAttribute* in your application code, providing your converter definition.

```
    public class JwtUserAttribute : FromPrincipalAttribute
    {
        public JwtUserAttribute() : base(typeof(JwtUserPrincipalConverter))
        {
        }
    }
```

Doing this simplifies your code from:

```
public async Task<IHttpActionResult> DoStuff([FromPrincipal(typeof(JwtUserPrincipalConverter))] JwtUser user)
```

to

```
public async Task<IHttpActionResult> DoStuff([JwtUser] JwtUser user)
```

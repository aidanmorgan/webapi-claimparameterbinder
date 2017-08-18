
# webapi-claimparameterbinder

This is a simple project that allows you to bind values from the *IPrincipal* to arguments to your controller method.

Simply mark method arguments on your controller methods with the *[FromClaim]* attribute to have then load their corresponding value from the ```IPrinicpal```. You can specify a name of the claim, use the name of the controller parameter as the claim name or perform complex object conversions.


## Why?

When using JWT I have fallen into a pattern of writing a ```ApiController``` base class in my application that does something like this:

```
        protected Guid? UserId
        {
            get
            {
                var principal = RequestContext.Principal;

                var userId = GetClaim(principal, Claims.ApplicationIdClaimName)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                return Guid.Parse(userId);
            }
        }
```

Then having to have ever controller method write code like this:

```
        public async Task<IHttpActionResult> DoStuff([)
        {
            if (!UserId.HasValue)
            {
                return Unauthorized();
            }

            // do work here

            return Ok();
        }
```

One afternoon, after doing this for about the 100th time I got fed up and decided to learn about ASP Parameter Binding.



## Using ClaimParameterBinder - Simple Case

If you want to bind a simple value to a Claim in the ```IPrincipal``` you can simply do:

```
public async Task<IHttpActionResult> DoStuff([FromClaim("application_id")] Guid id)
```

Or even easier, if you want to take advantage of the matching provided by ASP:

```
public async Task<IHttpActionResult> DoStuff([FromClaim] Guid application_id)
```

## Using ClaimParameterBinder - Complex Objects

Sometimes you might want to map multiple Claim values in the ```IPrincipal``` to a controller parameter, to do this you use a ```IFromClaimTypeConverter```. Implementing this interface is simple:

```
    public class ApplicationUserClaimTypeConverter : IFromClaimTypeConverter
    {
        public object FromPrincipal(IPrincipal principal)
        {
            var id = principal.GetClaim(Claims.ApplicationIdClaimName)?.Value;
            var role = principal.GetClaim(Claims.ApplicationRoleClaimName)?.Value;

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(role))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            return ApplicationUser.Create(id, role);
        }
    }
```

This converter will now be called when a complex value is encountered for the ```[FromClaim]``` attribute.

### Registering IFromClaimTypeConverter

There are two ways to use the custom ```IFromClaimTypeConverter```, either registering them globally, or specifying it as part of the attribute.

**Global Registration**

To register globally simply put the following line in your ```WebApiConfig```:

```
FromClaimTypeConverters.Add(typeof(ApplicationUser), new ApplicationUserClaimTypeConverter());
```

Now you can use a complex object in your Controller definitions:

```
public async Task<IHttpActionResult> DoStuff([FromClaim(BindingType.Complex)] ApplicationUser user)
```

**Attribute Specification**

If you don't want to register your ```IFromClaimTypeConverter``` globally, then you can specify it as part of the attribute declaration.

```
public async Task<IHttpActionResult> DoStuff([FromClaim(typeof(ApplicationUserClaimTypeConverter))] FractionUser user)
```



## Hiding the Parameter from Swagger

Simply open up your Swagger config and add the following line:

```
c.OperationFilter<SwaggerHiddenOperationFilter>();
```

This will hide any parameter to your Controller methods that are marked with the ```FromClaim``` Attribute (or any Attribute that implements ```ISwaggerHiddenAttribute```)

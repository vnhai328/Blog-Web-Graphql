using System.Security.Claims;
using auth_graphql.Models;
using HotChocolate.Resolvers;

namespace auth_graphql.Middlewares.UseUser;

public class UserMiddleware
{
    public const string USER_CONTEXT_DATA_KEY = "User";
    private readonly FieldDelegate _next;
    public UserMiddleware(FieldDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(IMiddlewareContext context)
    {
        if(context.ContextData.TryGetValue("ClaimsPrincipal", out object rawClaimsPrincipal) && rawClaimsPrincipal is ClaimsPrincipal claimsPrincipal)
        {
            var user = new User()
            {
                Id = Guid.Parse(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier)),
                Email = claimsPrincipal.FindFirstValue(ClaimTypes.Email)
            };

            context.ContextData.Add(USER_CONTEXT_DATA_KEY, user);
        }

        await _next(context);
    }
}
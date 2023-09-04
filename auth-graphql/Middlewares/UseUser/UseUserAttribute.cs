using System.Reflection;
using System.Runtime.CompilerServices;
using HotChocolate.Types.Descriptors;

namespace auth_graphql.Middlewares.UseUser;

public class UseUserAttribute : ObjectFieldDescriptorAttribute
{
    public UseUserAttribute([CallerLineNumber] int order = 0)
    {
        Order = order;
    }

    protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, MemberInfo member)
    {
        descriptor.Use<UserMiddleware>();
    }
}
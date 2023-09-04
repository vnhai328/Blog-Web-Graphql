using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace auth_graphql;

public partial class Startup
{
    internal static byte[] SharedSecret = Encoding.ASCII.GetBytes("helloiamwanchoiandthisissharedsecrethehe123456");

    private void ConfigureAuthenticationServices(IServiceCollection services)
    {
        services
            .AddAuthentication(a =>
            {
                a.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                a.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(a => {
                a.RequireHttpsMetadata = false;
                a.SaveToken = true;
                a.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(SharedSecret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
                a.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.HttpContext.Request.Query.ContainsKey("token"))
                        {
                            context.Token = context.HttpContext.Request.Query["token"];
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }
}
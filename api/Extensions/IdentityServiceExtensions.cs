using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace api.Extensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration configuration)
    {
        #region Authentication & Authorization

        string tokenValue = configuration["TokenKey"]!;

        if (!string.IsNullOrEmpty(tokenValue))
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenValue)),

                    ValidateIssuer = false,

                    ValidateAudience = false,

                    ValidateLifetime = true
                };
            });
        }

        #endregion Authentication & Authorization

        return services;
    }
}

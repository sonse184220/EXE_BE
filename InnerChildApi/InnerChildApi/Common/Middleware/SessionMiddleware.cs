using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Service.Interfaces;
using System.Security.Claims;
using Contract.Dtos.Enums;

namespace InnerChildApi.Common.Middleware
{
    public class SessionMiddleware
    {
        private readonly RequestDelegate _next;
        public SessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {

                var tokenType = context.User.FindFirst("TokenType")?.Value;
                if (tokenType != JwtTypeEnum.FinalLogin.ToString())
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorize: Invalid token type");
                    return;
                }
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var profileId = context.User.FindFirst("ProfileId")?.Value;
                var sessionId = context.User.FindFirst("SessionId")?.Value;
                if (!string.IsNullOrEmpty(userId) &&!string.IsNullOrEmpty(profileId) &&!string.IsNullOrEmpty(sessionId))
                {
                    
                    var authService = context.RequestServices.GetRequiredService<IAuthService>();
                    var result = await authService.IsSessionValidAsync(userId, profileId, sessionId);
                    if (!result)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync("Session is invalid or expired.");
                        return;
                    }
                }

            }
            await _next(context);
        }
    }
}

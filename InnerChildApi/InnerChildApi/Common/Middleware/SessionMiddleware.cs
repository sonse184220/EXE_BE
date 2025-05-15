using Contract.Common.Constant;
using Contract.Common.Enums;
using Service.Interfaces;
using System.Security.Claims;

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

                var tokenType = context.User.FindFirst(JwtClaimTypeConstant.TokenType)?.Value;
                if (tokenType != JwtTypeEnum.FinalLogin.ToString())
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Invalid token type");
                    return;
                }
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var profileId = context.User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
                var sessionId = context.User.FindFirst(JwtClaimTypeConstant.SessionId)?.Value;
                if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(profileId) && !string.IsNullOrEmpty(sessionId))
                {

                    var sessionService = context.RequestServices.GetRequiredService<ISessionService>();
                    var result = await sessionService.IsSessionValidAsync(userId, profileId, sessionId);
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

using Contract.Common.Constant;
using Contract.Dtos.HubModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace InnerChildApi.Common.Hubs
{
    [Authorize]
    public class SessionHub : Hub
    {
        private static readonly Dictionary<string, ConnectionModel> _connections = new();


        public async Task RegisterConnection()
        {
            var userId = Context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var profileId = Context.User.FindFirst(JwtClaimTypeConstant.ProfileId)?.Value;
            var sessionId = Context.User.FindFirst(JwtClaimTypeConstant.SessionId)?.Value;

            _connections[Context.ConnectionId] = new ConnectionModel
            {
                UserId = userId,
                ProfileId = profileId,
                SessionId = sessionId
            };
            await Groups.AddToGroupAsync(Context.ConnectionId, $"profile_{profileId}");

        }
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            if (_connections.TryGetValue(Context.ConnectionId, out var sessionInfo))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"profile_{sessionInfo.ProfileId}");
                _connections.Remove(Context.ConnectionId);
            }

            await base.OnDisconnectedAsync(exception);
        }
    }
}

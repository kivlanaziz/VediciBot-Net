using Discord;
using Discord.WebSocket;
using Microsoft.Graph.Models;
using VediciBot_Net.Core.Common;
using VediciBot_Net.Core.Util.Constants;

namespace VediciBot_Net.Core.Services
{
    public class UserStatusHandler : IUserStatusHandler
    {
        private readonly DiscordShardedClient _client;
        public UserStatusHandler(DiscordShardedClient client)
        {
            _client = client;
        }

        public async Task InitializeAsync()
        {
            _client.UserJoined += _client_UserJoined;
            _client.UserLeft += _client_UserLeft;
        }

        private async Task _client_UserLeft(SocketGuild arg1, SocketUser arg2)
        {
            await Logger.Log(LogSeverity.Debug, "| Event", $"User '{arg2.Username}' Left the Server.");

            var channel = arg1.GetTextChannel(FindArrivalTextChannel(arg1));
            await channel!.SendMessageAsync($"User '{arg2.Username}' Left the Server.");
        }

        private async Task _client_UserJoined(SocketGuildUser arg2)
        {
            await Logger.Log(LogSeverity.Debug, "| Event", $"User '{arg2.Username}' Joined the Server.");

            var channel = arg2.Guild.GetTextChannel(FindArrivalTextChannel(arg2.Guild));
            await channel!.SendMessageAsync($"User '{arg2.Username}' Joined the Server.");
        }

        private static ulong FindArrivalTextChannel(SocketGuild arg)
        {
            var channels = arg.TextChannels;
            foreach (SocketTextChannel channel in channels)
            {
                if (channel.Name.Equals(ChannelName.GENERAL))
                {
                    return channel.Id;
                }
            }
            return 0;
        }
    }
}

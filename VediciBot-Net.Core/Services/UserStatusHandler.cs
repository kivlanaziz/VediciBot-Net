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

            EmbedBuilder embedMessage = new EmbedBuilder();
            embedMessage.WithColor(Color.Red);
            embedMessage.WithTitle("Departure Notification");
            embedMessage.AddField("User: ", arg2.Username, true);
            embedMessage.AddField("Left At: ", DateTime.Now, true);
            embedMessage.WithFooter("Vedici Bot V2", arg1.IconUrl);

            await channel!.SendMessageAsync("", false, embedMessage.Build());
        }

        private async Task _client_UserJoined(SocketGuildUser arg2)
        {
            await Logger.Log(LogSeverity.Debug, "| Event", $"User '{arg2.Username}' Joined the Server.");

            var channel = arg2.Guild.GetTextChannel(FindArrivalTextChannel(arg2.Guild));

            EmbedBuilder embedMessage = new EmbedBuilder();
            embedMessage.WithColor(Color.Blue);
            embedMessage.WithTitle("Arrival Notification");
            embedMessage.AddField("User: ", arg2.Username, true);
            embedMessage.AddField("Joined At: ", DateTime.Now, true);
            embedMessage.WithFooter("Vedici Bot V2", arg2.Guild.IconUrl);

            await channel!.SendMessageAsync("", false, embedMessage.Build());
        }

        private static ulong FindArrivalTextChannel(SocketGuild arg)
        {
            var channels = arg.TextChannels;
            foreach (SocketTextChannel channel in channels)
            {
                if (channel.Name.Equals(ChannelName.GENERAL) || channel.Name.Contains(ChannelName.ARRIVAL))
                {
                    return channel.Id;
                }
            }
            return 0;
        }
    }
}

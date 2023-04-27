using System.Reflection;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using VediciBot_Net.Core.Common;
using VediciBot_Net.Core.Init;
using Microsoft.Extensions.Logging;

namespace VediciBot_Net.Core.Services;
public class CommandHandler : ICommandHandler
{
    private readonly DiscordShardedClient _client;
    private readonly CommandService _commands;

    public CommandHandler(
        DiscordShardedClient client,
        CommandService commands)
    {
        _client = client;
        _commands = commands;
    }

    public async Task InitializeAsync()
    {
        await _commands.AddModulesAsync(Assembly.GetExecutingAssembly(), Bootstrapper.ServiceProvider);

        _client.MessageReceived += HandleCommandAsync;

        _commands.CommandExecuted += async (optional, context, result) =>
        {
            if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
            {
                await context.Channel.SendMessageAsync($"error: {result}");
            }
        };

        foreach (var module in _commands.Modules)
        {
            await Logger.Log(LogSeverity.Info, $"{nameof(CommandHandler)} | Commands", $"Module '{module.Name}' initialized.");
        }
    }

    private async Task HandleCommandAsync(SocketMessage arg)
    {
        if (arg is not SocketUserMessage msg)
            return;

        if (msg.Author.Id == _client.CurrentUser.Id || msg.Author.IsBot)
            return;

        var context = new ShardedCommandContext(_client, msg);

        var markPos = 0;
        await Logger.Log(LogSeverity.Info, $"{nameof(CommandHandler)} | Commands", $"Message Received: '{msg}'.");
        if (msg.HasCharPrefix('!', ref markPos) || msg.HasCharPrefix('?', ref markPos))
        {
            var result = await _commands.ExecuteAsync(context, markPos, Bootstrapper.ServiceProvider);
        }
    }
}
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using VediciBot_Net.Core.Services;
using VediciBot_Net.Core.Common;
using VediciBot_Net.Core.Init;
using Discord.Interactions;

var config = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json")
    .AddEnvironmentVariables()
    .Build();
var client = new DiscordShardedClient(new DiscordSocketConfig
{
    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
});


var commands = new CommandService(new CommandServiceConfig
{
    LogLevel = LogSeverity.Info,

    CaseSensitiveCommands = false,
});

Bootstrapper.Init();
Bootstrapper.RegisterInstance(client);
Bootstrapper.RegisterInstance(commands);
Bootstrapper.RegisterInstance(new InteractionService(client));
Bootstrapper.RegisterType<ICommandHandler, CommandHandler>();
Bootstrapper.RegisterType<IApplicationCommandHandler, ApplicationCommandHandler>();
Bootstrapper.RegisterInstance(config);

await MainAsync();

async Task MainAsync()
{
    await Bootstrapper.ServiceProvider.GetRequiredService<ICommandHandler>().InitializeAsync();
    await Bootstrapper.ServiceProvider.GetRequiredService<IApplicationCommandHandler>().InitializeAsync();

    var appCommand = Bootstrapper.ServiceProvider.GetRequiredService<InteractionService>();

    client.ShardReady += async shard =>
    {
        await Logger.Log(LogSeverity.Info, "ShardReady", $"Shard Number {shard.ShardId} is connected and ready!");
        await appCommand.RegisterCommandsGloballyAsync();
    };

    var token = config.GetRequiredSection("Settings")["DiscordBotToken"];
    if (string.IsNullOrWhiteSpace(token))
    {
        await Logger.Log(LogSeverity.Error, $"{nameof(Program)} | {nameof(MainAsync)}", "Token is null or empty.");
        return;
    }

    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();

    await Task.Delay(Timeout.Infinite);
}
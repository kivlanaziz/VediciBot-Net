﻿using Discord;
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
    // Again, log level:
    LogLevel = LogSeverity.Info,

    // There's a few more properties you can set,
    // for example, case-insensitive commands.
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
    //await appCommand.InitializeAsync();

    client.ShardReady += async shard =>
    {
        await Logger.Log(LogSeverity.Info, "ShardReady", $"Shard Number {shard.ShardId} is connected and ready!");
        await appCommand.RegisterCommandsGloballyAsync();
    };

    // Login and connect.
    var token = config.GetRequiredSection("Settings")["DiscordBotToken"];
    if (string.IsNullOrWhiteSpace(token))
    {
        await Logger.Log(LogSeverity.Error, $"{nameof(Program)} | {nameof(MainAsync)}", "Token is null or empty.");
        return;
    }

    await client.LoginAsync(TokenType.Bot, token);
    await client.StartAsync();

    // Wait infinitely so your bot actually stays connected.
    await Task.Delay(Timeout.Infinite);
}
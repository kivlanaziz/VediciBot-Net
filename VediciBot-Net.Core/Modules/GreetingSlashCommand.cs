using Discord.Interactions;
using VediciBot_Net.Core.Services;

namespace VediciBot_Net.Core.Modules
{
    public class GreetingSlashCommand : InteractionModuleBase<ShardedInteractionContext>
    {
        // dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService Commands { get; set; }

        // our first /command!
        [SlashCommand("hello", "Say hello to BOT")]
        public async Task Hello()
        {
            await RespondAsync("Hi!");
        }
    }
}

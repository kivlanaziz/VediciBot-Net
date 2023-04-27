using Discord.Interactions;
using VediciBot_Net.Core.Services;

namespace VediciBot_Net.Core.Modules
{
    public class GreetingSlashCommand : InteractionModuleBase<ShardedInteractionContext>
    {
        public InteractionService Commands { get; set; }

        [SlashCommand("hello", "Say hello to BOT")]
        public async Task Hello()
        {
            await RespondAsync("Hi!");
        }
    }
}

using Discord;
using Discord.Commands;
using RunMode = Discord.Commands.RunMode;

namespace VediciBot_Net.Core.Modules
{
    public class GreetingCommand : ModuleBase<ShardedCommandContext>
    {
        public CommandService CommandService { get; set; }

        [Command("hello", RunMode = RunMode.Async)]
        public async Task Hello()
        {
            await Context.Message.ReplyAsync($"Hello {Context.User.Username}. Nice to meet you!");
        }
    }
}

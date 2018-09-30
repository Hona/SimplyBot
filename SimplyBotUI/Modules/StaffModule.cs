using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SimplyBotUI.Modules
{
    [Summary("Commands to help the staff")]
    // TODO: Create a custom precondition for staff
    [RequireUserPermission(GuildPermission.BanMembers)]
    public class StaffModule : ExtraModuleBase
    {
        private readonly DiscordSocketClient _client;

        public StaffModule(DiscordSocketClient client)
        {
            _client = client;
        }

        [Command("update")]
        [Summary("Sends an embed into the updates channel")]
        public async Task SendUpdate(string title, [Remainder] string description)
        {
            if (_client.GetChannel(Constants.Constants.UpdateChannelId) is IMessageChannel channel)
                await channel.SendMessageAsync("",
                    embed: EmbedHelper.CreateEmbed($"**__[{title.ToUpper()}]__**", description, Color.Green));
        }
    }
}
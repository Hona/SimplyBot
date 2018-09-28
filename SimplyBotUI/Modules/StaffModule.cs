using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace SimplyBotUI.Modules
{
    // TODO: Create a custom precondition for staff
    [RequireUserPermission(GuildPermission.BanMembers)]
    public class StaffModule : ExtraModuleBase
    {
        private DiscordSocketClient _client;

        public StaffModule(DiscordSocketClient client)
        {
            _client = client;
        }

        //[Command("update")]
        //public async Task Embed(string title, [Remainder]string description)
        //{
        //    if (_client.GetChannel(Constants.RankChannelId) is IMessageChannel channel)
        //        await channel.SendMessageAsync("", embed:);
        //}
    }
}
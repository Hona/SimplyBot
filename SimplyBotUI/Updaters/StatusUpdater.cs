using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SimplyBotUI.Modules;

namespace SimplyBotUI.Updaters
{
    internal class StatusUpdater : BaseUpdater
    {
        private readonly DiscordSocketClient _client;

        public StatusUpdater(DiscordSocketClient client)
        {
            _client = client;
        }

        internal async Task UpdateStatus()
        {
            if (!(_client.GetChannel(Constants.Constants.StatusChannelId) is IMessageChannel channel)) return;

            try
            {
                await DeleteMessages(channel);
            }
            catch (Exception)
            {
                await channel.SendMessageAsync("Could not delete messages");
            }

            try
            {
                await SendStatus(channel);
            }
            catch (Exception e)
            {
                await channel.SendMessageAsync(e.Message + Environment.NewLine + e.Source);
            }
        }

        private async Task SendStatus(IMessageChannel channel)
        {
            var serverModule = new ServerModule();
            await channel.SendMessageAsync("",
                embed: serverModule.JustJumpEmbed.WithFooter("Updated " + DateTime.Now.ToShortTimeString()));
            await channel.SendMessageAsync("",
                embed: serverModule.HightowerEmbed.WithFooter("Updated " + DateTime.Now.ToShortTimeString()));
            await channel.SendMessageAsync("",
                embed: serverModule.GmodEmbed.WithFooter("Updated " + DateTime.Now.ToShortTimeString()));
            await channel.SendMessageAsync("",
                embed: (await serverModule.GetMinecraftEmbed()).WithFooter(
                    "Updated " + DateTime.Now.ToShortTimeString()));
        }
    }
}
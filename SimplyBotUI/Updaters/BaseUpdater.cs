using System.Threading.Tasks;
using Discord;

namespace SimplyBotUI.Updaters
{
    internal class BaseUpdater
    {
        protected async Task DeleteAllMessages(IMessageChannel channel)
        {
            var messages = channel.GetMessagesAsync().Flatten().Result;
            await channel.DeleteMessagesAsync(messages);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace SimplyBotUI.Updaters
{
    internal class BaseUpdater
    {
        protected async Task DeleteMessages(IMessageChannel channel)
        {
            var messages = channel.GetMessagesAsync().Flatten().Result;
            await channel.DeleteMessagesAsync(messages);
        }
    }
}

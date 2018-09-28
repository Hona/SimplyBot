using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SimplyBotUI
{
    public class ExtraModuleBase : ModuleBase
    {
        public async Task ReplyNewEmbed(string text)
        {
            await ReplyEmbed(EmbedHelper.CreateEmbed(text));
        }

        public async Task ReplyEmbed(Embed embed)
        {
            await ReplyAsync("", embed: embed);
        }

        public async Task ReplyEmbed(EmbedBuilder embed)
        {
            if (embed.Description != null && embed.Description.Length > 2048)
                embed.WithDescription(embed.Description.Take(2000) + "...");
            await ReplyAsync("", embed: embed);
        }
    }
}
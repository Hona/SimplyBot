using Discord;

namespace SimplyBotUI
{
    internal static class EmbedHelper
    {
        internal static Embed CreateEmbed(string text)
        {
            var builder = new EmbedBuilder();
            builder.WithDescription(text);
            return builder;
        }
        internal static Embed CreateEmbed(string title,string text)
        {
            var builder = new EmbedBuilder();
            builder.WithDescription(text)
                .WithTitle(title);
            return builder;
        }
    }
}
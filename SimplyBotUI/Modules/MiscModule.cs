using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace SimplyBotUI.Modules
{
    public class MiscModule : ExtraModuleBase
    {
        public string MemoryUsage => $"{Math.Round(GC.GetTotalMemory(true) / (1024.0 * 1024.0), 2) * 10}MB";
        public string Uptime => $"{(DateTime.Now - Process.GetCurrentProcess().StartTime).Days}d {(DateTime.Now - Process.GetCurrentProcess().StartTime).Hours}h {(DateTime.Now - Process.GetCurrentProcess().StartTime).Minutes}m {(DateTime.Now - Process.GetCurrentProcess().StartTime).Seconds}s";

        [Command("stats")]
        public async Task Stats()
        {
            var builder = new EmbedBuilder{Title = "SimplyBot - Stats"};
            builder.AddInlineField("Uptime", Uptime)
                .AddInlineField("Memory Usage", MemoryUsage)
                .AddInlineField("Latency", Context.Client)
                .WithFooter($"Discord.Net ({DiscordConfig.Version}");
            await ReplyEmbed(builder);
        }
    }
}

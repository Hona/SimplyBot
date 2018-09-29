using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using QueryMaster;
using QueryMaster.GameServer;
using SimplyBotUI.Minecraft;
using Game = QueryMaster.Game;

namespace SimplyBotUI.Modules
{
    [Summary("Gets server info")]
    public class ServerModule : ExtraModuleBase
    {
        [Alias("si")]
        [Command("serverinfo")]
        [Summary("Source engine server info")]
        public async Task JustJumpInfo(string address)
        {
            var ip = address.Split(':')[0];
            if (!ushort.TryParse(address.Split(':')[1], out var port))
            {
                await ReplyNewEmbed("Invalid port number");
                return;
            }

            await DisplayInfo(ip, port);
        }

        [Alias("mc")]
        [Command("minecraft")]
        [Summary("Minecraft server info")]
        public async Task Minecraft()
        {
            var ping = await ServerPing.Ping();
            var builder = new EmbedBuilder();

            if (ping.Motd.Contains('§'))
            {
                var split = ping.Motd.Split('§');
                for (var i = 1; i < split.Length; i++) split[i] = string.Join(string.Empty, split[i].Skip(1));

                ping.Motd = string.Join(string.Empty, split);
            }

            builder.WithTitle(ping.Motd);
            builder.AddField("Players Online", $"{ping.PlayersOnline}/{ping.PlayersMax}");
            if (ping.OnlinePlayerList != null) builder.AddField("Players", string.Join(", ", ping.OnlinePlayerList));
            await ReplyEmbed(builder);
        }

        [Alias("jj")]
        [Command("justjump")]
        [Summary("JustJust server info")]
        public async Task JustJumpInfo()
        {
            await DisplayInfo(ServerConstants.JustJumpServerIpAddress, ServerConstants.JustJumpServerPort,
                Game.Team_Fortress_2);
        }

        [Alias("ht")]
        [Command("hightower")]
        [Summary("Hightower server info")]
        public async Task HighTowerInfo()
        {
            await DisplayInfo(ServerConstants.HightowerServerIpAddress, ServerConstants.HightowerServerPort,
                Game.Team_Fortress_2);
        }

        [Command("gmod")]
        [Summary("Gmod server info")]
        public async Task GmodInfo()
        {
            await DisplayInfo(ServerConstants.GmodServerIpAddress, ServerConstants.GmodServerPort, Game.Garrys_Mod);
        }

        private async Task DisplayInfo(string ip, ushort port)
        {
            var server = ServerQuery.GetServerInstance(EngineType.Source, ip, port, sendTimeout: 1000,
                receiveTimeout: 1000, throwExceptions: true);
            await ReplyInfo(server);
        }

        private async Task DisplayInfo(string ip, ushort port, Game game)
        {
            var server = ServerQuery.GetServerInstance(game, ip, port, receiveTimeout: 1000, throwExceptions: true);
            await ReplyInfo(server);
        }

        private async Task ReplyInfo(Server server)
        {
            var info = server.GetInfo();
            var builder = new EmbedBuilder {Title = $"**{info.Name}**"};
            builder.AddInlineField("Description", info.Description)
                .AddInlineField("IP", info.Address)
                .AddInlineField("Map", info.Map)
                .AddInlineField("Ping", info.Ping)
                .AddInlineField("Players Online", info.Players + "/" + info.MaxPlayers);
            if (server.GetPlayers().Any())
                builder.AddInlineField("Player List",
                    server.GetPlayers().OrderBy(x => x.Name).Aggregate("",
                            (currentString, nextPlayer) => currentString + "**" + nextPlayer.Name + "**" + ", ")
                        .TrimEnd(',', ' '));
            await ReplyEmbed(builder);
        }
    }
}
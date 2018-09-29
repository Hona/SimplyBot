using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using SimplyBotUI.Data;

namespace SimplyBotUI.Modules
{
    [Summary("Commands that you you'll never need")]
    [RequireOwner]
    public class OwnerModule : ExtraModuleBase
    {
        private readonly SimplyDataAccess _simplyDataAccess;
        private readonly Program _program;
        public DiscordSocketClient Client;

        internal OwnerModule(Program program, SimplyDataAccess simplyDataAccess)
        {
            _program = program;
            _simplyDataAccess = simplyDataAccess;
        }

        [Command("rankupdate")]
        [Summary("Manually updates the rank channel")]
        public async Task RankUpdate()
        {
            _program.IntervalFunctions(null);
            await ReplyNewEmbed("Done");
        }

        [Command("msql")]
        [Summary("Executes unescaped SQL queries on the MapInfo database")]
        public async Task ExecuteMapInfoSql([Remainder] string sql)
        {
            var result = await _simplyDataAccess.ExecuteMapInfoQuery(sql);
            await ReplyNewEmbed(string.Join(Environment.NewLine, result));
            await ReplyNewEmbed("Done");
        }

        [Command("psql")]
        [Summary("Executes unescaped SQL queries on the PlayerRanks database")]
        public async Task ExecutePlayerRanksSql([Remainder] string sql)
        {
            var result = await _simplyDataAccess.ExecutePlayRanksQuery(sql);
            await ReplyNewEmbed(string.Join(string.Empty, result));
            await ReplyNewEmbed("Done");
        }
    }
}
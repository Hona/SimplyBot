using System;
using System.Threading.Tasks;
using Discord.Commands;
using SimplyBotUI.Data;

namespace SimplyBotUI.Modules
{
    [RequireOwner]
    public class OwnerModule : ExtraModuleBase
    {
        private readonly DataAccess _dataAccess;
        private readonly Program _program;

        internal OwnerModule(Program program, DataAccess dataAccess)
        {
            _program = program;
            _dataAccess = dataAccess;
        }

        [Command("rankupdate")]
        public async Task RankUpdate()
        {
            _program.UpdateRanks(null);
            await ReplyNewEmbed("Done");
        }

        [Command("msql")]
        public async Task ExecuteMapInfoSql([Remainder] string sql)
        {
            var result = await _dataAccess.ExecuteMapInfoQuery(sql);
            await ReplyNewEmbed(string.Join(Environment.NewLine, result));
            await ReplyNewEmbed("Done");
        }

        [Command("psql")]
        public async Task ExecutePlayerRanksSql([Remainder] string sql)
        {
            var result = await _dataAccess.ExecutePlayRanksQuery(sql);
            await ReplyNewEmbed(string.Join(string.Empty, result));
            await ReplyNewEmbed("Done");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using SimplyBotUI.Data;
using Dapper;
using Dapper.FluentMap;
using Dapper.FluentMap.TypeMaps;

namespace SimplyBotUI.Modules
{
    [RequireOwner]
    public class OwnerModule : ExtraModuleBase
    {
        private Program _program;
        private DataAccess _dataAccess;
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
                await ReplyNewEmbed(String.Join(string.Empty, result));
            await ReplyNewEmbed("Done");
        }

    }
}

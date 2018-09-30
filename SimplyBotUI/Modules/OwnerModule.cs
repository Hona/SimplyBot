using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using SimplyBotUI.Data;

namespace SimplyBotUI.Modules
{
    [Summary("Commands that you you'll never need")]
    [RequireOwner]
    public class OwnerModule : ExtraModuleBase
    {
        private readonly Program _program;
        private readonly SimplyDataAccess _simplyDataAccess;

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

        [Command("giveall")]
        [Summary("Executes unescaped SQL queries on the PlayerRanks database")]
        public async Task GiveAll([Remainder] string roleParam)
        {
            var role = Context.Guild.Roles.First(x => x.Name.ToLower().Contains(roleParam.ToLower()));
            var users = (await Context.Guild.GetUsersAsync()).Where(x => !x.IsBot).ToList();
            var count = 0;
            foreach (var user in users)
            {
                count++;
                await user.AddRoleAsync(role);
            }

            await ReplyNewEmbed($"Done added to {count} non-bot users");
        }
    }
}
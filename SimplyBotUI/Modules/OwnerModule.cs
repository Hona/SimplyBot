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
        public Program Program { get; set; }
        internal SimplyDataAccess SimplyDataAccess { get; set; }

        [Command("rankupdate")]
        [Summary("Manually updates the rank channel")]
        public async Task RankUpdate()
        {
            Program.IntervalFunctions(null);
            await ReplyNewEmbed("Done");
        }

        [Command("msql")]
        [Summary("Executes unescaped SQL queries on the MapInfo database")]
        public async Task ExecuteMapInfoSql([Remainder] string sql)
        {
            var result = await SimplyDataAccess.ExecuteMapInfoQuery(sql);
            await ReplyNewEmbed(string.Join(Environment.NewLine, result));
            await ReplyNewEmbed("Done");
        }

        [Command("psql")]
        [Summary("Executes unescaped SQL queries on the PlayerRanks database")]
        public async Task ExecutePlayerRanksSql([Remainder] string sql)
        {
            var result = await SimplyDataAccess.ExecutePlayRanksQuery(sql);
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
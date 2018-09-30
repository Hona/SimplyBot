using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using SimplyBotUI.Data;
using SimplyBotUI.Models.Simply;

namespace SimplyBotUI.Modules
{
    [Summary("Commands that use the database (just jump times/map info), hightower rankings")]
    public class DatabaseModule : ExtraModuleBase
    {
        private readonly SimplyDataAccess _simplyDataAccess;

        internal DatabaseModule(SimplyDataAccess simplyDataAccess)
        {
            _simplyDataAccess = simplyDataAccess;
        }

        [Command("maps")]
        [Summary("Gets a list of maps containing the keyword")]
        public async Task Maps([Remainder] string map)
        {
            var result = await _simplyDataAccess.GetMapsContainingName(map);
            var builder = new EmbedBuilder {Title = $"**{map}**"};

            var mapList = string.Join(Environment.NewLine, result);
            builder.WithDescription(mapList != string.Empty ? mapList : "No maps found");

            await ReplyEmbed(builder);
        }

        [Command("map")]
        [Summary("Gets map details from a partial/full name")]
        public async Task MapInfo([Remainder] string map)
        {
            var details = await _simplyDataAccess.GetMapDetails(map);
            var builder = new EmbedBuilder {Title = $"**{details.Map}**"};

            builder.AddInlineField("Demoman Tier", details.DemomanTier);
            builder.AddInlineField("Soldier Tier", details.SoldierTier);
            builder.AddInlineField("Recommended Class", details.Class);
            builder.AddInlineField("Jumps", details.Jumps);
            builder.AddInlineField("Creator", details.Creator);
            builder.AddInlineField("Times Played", details.Played);

            await ReplyEmbed(builder);
        }

        #region Top Time Commands

        [Command("dtimes")]
        [Summary("Gets the top demoman times on a map")]
        public async Task GetDemoRecords([Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var times = await _simplyDataAccess.GetMapTimes(ClassConstants.Demoman, map);
            await DisplayHighscores(times, map, "demoman");
        }

        [Command("stimes")]
        [Summary("Gets the top soldier times on a map")]
        public async Task GetSollyRecords([Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var times = await _simplyDataAccess.GetMapTimes(ClassConstants.Soldier, map);
            await DisplayHighscores(times, map, "soldier");
        }

        [Command("ctimes")]
        [Summary("Gets the top conc times on a map")]
        public async Task GetConcRecords([Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var times = await _simplyDataAccess.GetMapTimes(ClassConstants.Conc, map);
            await DisplayHighscores(times, map, "conc");
        }

        [Command("etimes")]
        [Summary("Gets the top engineer times on a map")]
        public async Task GetEngiRecords([Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var times = await _simplyDataAccess.GetMapTimes(ClassConstants.Engineer, map);
            await DisplayHighscores(times, map, "engineer");
        }

        [Command("ptimes")]
        [Summary("Gets the top pyro times on a map")]
        public async Task GetPyroRecords([Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var times = await _simplyDataAccess.GetMapTimes(ClassConstants.Pyro, map);
            await DisplayHighscores(times, map, "pyro");
        }

        private async Task DisplayHighscores(List<HighscoreModel> results, string map, string className)
        {
            var sortedResults = results.OrderBy(result => result.RunTime).Take(4).ToList();

            var builder = new EmbedBuilder
                {Title = $"Top {className} times on **{await _simplyDataAccess.GetFullMapName(map)}**"};
            for (var i = 1; i <= sortedResults.Count; i++)
                builder.AddField($"#{i} Time",
                    $"**{sortedResults[i - 1].Name}**: {sortedResults[i - 1].GetTimeSpan:c}");

            if (sortedResults.Count == 0)
                builder.WithDescription("No times available");
            await ReplyEmbed(builder);
        }

        #endregion

        #region Time Commands

        [Command("dtime")]
        [Summary("Gets your demoman time on a map")]
        public async Task GetDemoTime(string user, [Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var time = await _simplyDataAccess.GetMapTime(ClassConstants.Demoman, map, user);
            await DisplayHighscore(time, map, "demoman");
        }

        [Command("stime")]
        [Summary("Gets your soldier time on a map")]
        public async Task GetSollyTime(string user, [Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var time = await _simplyDataAccess.GetMapTime(ClassConstants.Soldier, map, user);
            await DisplayHighscore(time, map, "soldier");
        }

        [Command("ctime")]
        [Summary("Gets your conc time on a map")]
        public async Task GetConcTime(string user, [Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var time = await _simplyDataAccess.GetMapTime(ClassConstants.Conc, map, user);
            await DisplayHighscore(time, map, "conc");
        }

        [Command("ptime")]
        [Summary("Gets your pyro time on a map")]
        public async Task GetPyroTime(string user, [Remainder] [Summary("The map name, can be partial")]
            string map)
        {
            var time = await _simplyDataAccess.GetMapTime(ClassConstants.Pyro, map, user);
            await DisplayHighscore(time, map, "pyro");
        }

        private async Task DisplayHighscore(HighscoreModel result, string map, string className)
        {
            var builder = new EmbedBuilder
                {Title = $"**{result.Name}'s** {className} time on **{await _simplyDataAccess.GetFullMapName(map)}**"};
            if (EqualityComparer<HighscoreModel>.Default.Equals(result, default(HighscoreModel)))
                builder.WithDescription("No time available");

            builder.WithDescription($"**{result.Name}**: {result.GetTimeSpan:c}");

            await ReplyEmbed(builder);
        }

        #endregion
    }
}
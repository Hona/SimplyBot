using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using SimplyBotUI.Data;
using SimplyBotUI.Models.Simply;

namespace SimplyBotUI.Updaters
{
    internal class RankUpdater : BaseUpdater
    {
        private readonly DiscordSocketClient _client;
        private readonly SimplyDataAccess _simplyDataAccess;

        public RankUpdater(DiscordSocketClient client, SimplyDataAccess simplyDataAccess)
        {
            _client = client;
            _simplyDataAccess = simplyDataAccess;
        }

        internal async Task UpdateRanks()
        {
            if (!(_client.GetChannel(Constants.Constants.RankChannelId) is IMessageChannel channel)) return;

            try
            {
                await DeleteMessages(channel);
                await SendTopJumpRanks(channel);
                await SendTopHightowerRanks(channel);
                await SendRecentRecords(channel);
                await SendRecentPersonalBests(channel);
            }
            catch (Exception e)
            {
                await channel.SendMessageAsync(e.Message);
            }
        }

        private async Task SendTopJumpRanks(IMessageChannel channel)
        {
            var overallTopString = await GetOverallTopString();
            var soldierTopString = await GetSoldierTopString();
            var demomanTopString = await GetDemomanTopString();
            var concTopString = await GetConcTopString();
            var engiTopString = await GetEngiTopString();
            var pyroTopString = await GetPyroTopString();

            var builder = new EmbedBuilder {Title = "**Top Ranked Jumpers**"};
            builder.AddInlineField("Overall", overallTopString)
                .AddInlineField("Soldier", soldierTopString)
                .AddInlineField("Demoman", demomanTopString)
                .AddInlineField("Engineer", engiTopString)
                .AddInlineField("Pyro", pyroTopString)
                .AddInlineField("Conc", concTopString)
                .WithFooter("Updated " + DateTime.Now.ToShortTimeString());
            await channel.SendMessageAsync("", embed: builder);
        }

        private async Task SendTopHightowerRanks(IMessageChannel channel)
        {
            var topHightowerScore = await _simplyDataAccess.GetTopHightowerRank(15);
            var topHightowerScoreString = "";

            for (var i = 0; i < topHightowerScore.Count; i++)
                topHightowerScoreString +=
                    $"**__#{i + 1}__**: **__{topHightowerScore[i].Nickname}__** {Math.Round(topHightowerScore[i].Points)} points, **{topHightowerScore[i].Kills} kills**, {topHightowerScore[i].Deaths} deaths, **{Math.Round((double) topHightowerScore[i].Kills / topHightowerScore[i].Deaths, 1)} K/D**, {topHightowerScore[i].Headshots} headshots, **{Math.Round(topHightowerScore[i].Playtime / 60 / 60)} hours**{Environment.NewLine}";

            var builder = new EmbedBuilder {Title = "**Top Ranked Hightower Players**"};

            builder.WithDescription(topHightowerScoreString)
                .WithFooter("Updated " + DateTime.Now.ToShortTimeString());
            await channel.SendMessageAsync("", embed: builder);
        }

        private async Task SendRecentRecords(IMessageChannel channel)
        {
            var recentRecords = await _simplyDataAccess.GetRecentRecords(10);
            var recentRecordsString = recentRecords.Aggregate("",
                (currentString, nextHighscore) => currentString +
                                                  $"{ClassConstants.ToString(nextHighscore.Class)} **#{nextHighscore.Position + 1}** on **{nextHighscore.Map}** in **__{nextHighscore.GetTimeSpan:c}__** run by **{nextHighscore.Name}**" +
                                                  Environment.NewLine);

            var builder = new EmbedBuilder {Title = "**Recent Map Records**"};

            builder.WithDescription(recentRecordsString)
                .WithFooter("Updated " + DateTime.Now.ToShortTimeString());
            await channel.SendMessageAsync("", embed: builder);
        }

        private async Task SendRecentPersonalBests(IMessageChannel channel)
        {
            // TODO: Rename these
            var recentPersonalBest = await _simplyDataAccess.GetRecentPersonalBests(15);
            var recentRecordsString = recentPersonalBest.Aggregate("",
                (currentString, nextHighscore) => currentString +
                                                  $"**{nextHighscore.Name}** got a **{ClassConstants.ToString(nextHighscore.Class)}** personal best of **{nextHighscore.GetTimeSpan:c}** on **{nextHighscore.Map}**" +
                                                  Environment.NewLine);

            var builder = new EmbedBuilder {Title = "**Recent Personal Bests**"};

            builder.WithDescription(recentRecordsString)
                .WithFooter("Updated " + DateTime.Now.ToShortTimeString());
            await channel.SendMessageAsync("", embed: builder);
        }

        private async Task<string> GetPyroTopString()
        {
            var pyroTop = await _simplyDataAccess.GetTopPyro(Constants.Constants.TopRankCount);
            return TopRankToString(pyroTop, "PyroRank");
        }

        private async Task<string> GetEngiTopString()
        {
            var engiTop = await _simplyDataAccess.GetTopEngi(Constants.Constants.TopRankCount);
            return TopRankToString(engiTop, "EngineerRank");
        }

        private async Task<string> GetConcTopString()
        {
            var concTop = await _simplyDataAccess.GetTopConc(Constants.Constants.TopRankCount);
            return TopRankToString(concTop, "ConcRank");
        }

        private async Task<string> GetDemomanTopString()
        {
            var demoTop = await _simplyDataAccess.GetTopDemo(Constants.Constants.TopRankCount);
            return TopRankToString(demoTop, "DemomanRank");
        }

        private async Task<string> GetOverallTopString()
        {
            var overallTop = await _simplyDataAccess.GetTopOverall(Constants.Constants.TopRankCount);
            return TopRankToString(overallTop, "OverallRank");
        }

        private async Task<string> GetSoldierTopString()
        {
            var soldierTop = await _simplyDataAccess.GetTopSolly(Constants.Constants.TopRankCount);
            return TopRankToString(soldierTop, "SoldierRank");
        }

        private string TopRankToString(List<JumpRankModel> list, string property)
        {
            var outputString = "";
            for (var i = 0; i < list.Count; i++) outputString += FormatLine(list, i, property);
            return outputString;
        }

        private string FormatLine(List<JumpRankModel> list, int index, string property)
        {
            return
                $"**#{index + 1}**: **{list[index].Name}** - {GetPropValue(list[index], property)} points{Environment.NewLine}";
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
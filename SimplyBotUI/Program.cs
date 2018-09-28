using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SimplyBotUI.Data;
using SimplyBotUI.Models;

namespace SimplyBotUI
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private Timer _rankUpdateTimer;
        private DataAccess _dataAccess;

        private int FromMinutes(int minutes)
        {
            return 1000 * 60 * minutes;
        }

        // Starts the program as an async instance
        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();
            _dataAccess = new DataAccess();

            AddClientEvents();

            await Login();

            _services = new ServiceCollection()
                .AddSingleton(_dataAccess)
                .AddSingleton(_client)
                .AddSingleton(this)
                .BuildServiceProvider();

            await InstallCommands();

            await _client.StartAsync();

            _rankUpdateTimer = new Timer(UpdateRanks, null, 0, FromMinutes(5));


            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void AddClientEvents()
        {
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Connected += Connected;
        }

        private async Task Login()
        {
            // TODO: Add error handling here
            var token = File.ReadAllText(Constants.TokenPath);
            await _client.LoginAsync(TokenType.Bot, token);
        }

        public async Task InstallCommands()
        {
            // Discover all of the commands in this assembly and load them.
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task MessageReceived(SocketMessage messageParam)
        {
            // Don't process the command if it was a System Message
            if (!(messageParam is SocketUserMessage message)) return;

            // Create a number to track where the prefix ends and the command begins
            var commandPosition = 0;

            // Determine if the message is a command, based on if it starts with '!' or a mention prefix
            if (!(message.HasCharPrefix(Constants.CommandPrefix, ref commandPosition) ||
                  message.HasMentionPrefix(_client.CurrentUser, ref commandPosition))) return;

            // Create a Command Context
            var context = new CommandContext(_client, message);

            // Execute the command. (result does not indicate a return value, 
            // rather an object stating if the command executed successfully)
            var result = await _commands.ExecuteAsync(context, commandPosition, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync("", embed: EmbedHelper.CreateEmbed(result.ErrorReason));
        }

        private Task Connected()
        {
            UpdateRanks(null);
            return Task.CompletedTask;
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        internal async void UpdateRanks(object state)
        {
            if (!(_client.GetChannel(Constants.RankChannelId) is IMessageChannel channel)) return;

            try
            {
                await DeleteRankMessages(channel);
                await SendTopJumpRanks(channel);
                await SendTopHightowerRanks(channel);
                await SendRecentRecords(channel);
            }
            catch (Exception e)
            {
                await channel.SendMessageAsync(e.Message);
            }
        }

        private async Task DeleteRankMessages(IMessageChannel channel)
        {
            var messages = channel.GetMessagesAsync().Flatten().Result;
            await channel.DeleteMessagesAsync(messages);
        }

        private async Task SendTopJumpRanks(IMessageChannel channel)
        {
            var overallTopString = await GetOverallTopString();
            var soldierTopString = await GetSoldierTopString();
            var demomanTopString = await GetDemomanTopString();
            var concTopString = await GetConcTopString();
            var engiTopString = await GetEngiTopString();
            var pyroTopString = await GetPyroTopString();

            var builder = new EmbedBuilder {Title = "Top Ranked Jumpers"};
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
            var topHightowerScore = await _dataAccess.GetTopHightowerRank(15);
            var topHightowerScoreString = "";

            for (var i = 0; i < topHightowerScore.Count; i++)
                topHightowerScoreString +=
                    $"**__#{i + 1}__**: **__{topHightowerScore[i].Nickname}__** {Math.Round(topHightowerScore[i].Points)} points, **{topHightowerScore[i].Kills} kills**, {topHightowerScore[i].Deaths} deaths, **{Math.Round((double) topHightowerScore[i].Kills / topHightowerScore[i].Deaths, 1)} K/D**, {topHightowerScore[i].Headshots} headshots, **{Math.Round(topHightowerScore[i].Playtime / 60 / 60)} hours**{Environment.NewLine}";

            var builder = new EmbedBuilder {Title = "Top Ranked Hightower Players"};

            builder.WithDescription(topHightowerScoreString)
                .WithFooter("Updated " + DateTime.Now.ToShortTimeString());
            await channel.SendMessageAsync("", embed: builder);
        }
        private async Task SendRecentRecords(IMessageChannel channel)
        {
            var recentRecords = await _dataAccess.GetRecentRecords(10);
            var recentRecordsString = recentRecords.Aggregate("", (currentString, nextHighscore) => currentString + $"{ClassConstants.ToString(nextHighscore.Class)} **#{nextHighscore.Position + 1}** on **{nextHighscore.Map}** in **__{nextHighscore.GetTimeSpan:c}__** run by **{nextHighscore.Name}**" + Environment.NewLine);

            var builder = new EmbedBuilder { Title = "Recent Map Records" };

            builder.WithDescription(recentRecordsString)
                .WithFooter("Updated " + DateTime.Now.ToShortTimeString());
            await channel.SendMessageAsync("", embed: builder);
        }

        private async Task<string> GetPyroTopString()
        {
            var pyroTop = await _dataAccess.GetTopPyro(Constants.TopRankCount);
            return TopRankToString(pyroTop, "PyroRank");
        }

        private async Task<string> GetEngiTopString()
        {
            var engiTop = await _dataAccess.GetTopEngi(Constants.TopRankCount);
            return TopRankToString(engiTop, "EngineerRank");
        }

        private async Task<string> GetConcTopString()
        {
            var concTop = await _dataAccess.GetTopConc(Constants.TopRankCount);
            return TopRankToString(concTop, "ConcRank");
        }

        private async Task<string> GetDemomanTopString()
        {
            var demoTop = await _dataAccess.GetTopDemo(Constants.TopRankCount);
            return TopRankToString(demoTop, "DemomanRank");
        }

        private async Task<string> GetOverallTopString()
        {
            var overallTop = await _dataAccess.GetTopOverall(Constants.TopRankCount);
            return TopRankToString(overallTop, "OverallRank");
        }

        private async Task<string> GetSoldierTopString()
        {
            var soldierTop = await _dataAccess.GetTopSolly(Constants.TopRankCount);
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
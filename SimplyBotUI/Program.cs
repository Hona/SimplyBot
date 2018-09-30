using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using SimplyBotUI.Data;
using SimplyBotUI.Updaters;

namespace SimplyBotUI
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commands;
        private RankUpdater _rankUpdater;
        private Timer _rankUpdateTimer;
        private IServiceProvider _services;
        private SimplyDataAccess _simplyDataAccess;
        private StatusUpdater _statusUpdater;
        private TempusDataAccess _tempusDataAccess;

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
            _client = new DiscordSocketClient(new DiscordSocketConfig {AlwaysDownloadUsers = true});
            _commands = new CommandService(new CommandServiceConfig {DefaultRunMode = RunMode.Async});
            _simplyDataAccess = new SimplyDataAccess();
            _tempusDataAccess = new TempusDataAccess();
            _rankUpdater = new RankUpdater(_client, _simplyDataAccess);
            _statusUpdater = new StatusUpdater(_client);

            AddClientEvents();

            await Login();

            _services = new ServiceCollection()
                .AddSingleton(_simplyDataAccess)
                .AddSingleton(_client)
                .AddSingleton(this)
                .AddSingleton(_commands)
                .AddSingleton(_tempusDataAccess)
                .BuildServiceProvider();

            await InstallCommands();

            await _client.StartAsync();

            _rankUpdateTimer = new Timer(IntervalFunctions, null, 0, FromMinutes(5));


            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void AddClientEvents()
        {
            _client.Log += Log;
            _client.MessageReceived += MessageReceived;
            _client.Connected += Connected;
            _client.Ready += Ready;
            _client.UserJoined += UserJoined;
        }

        private Task UserJoined(SocketGuildUser user)
        {
            if (user.Guild.Id == Constants.Constants.SimplyGuildId)
                user.AddRoleAsync(user.Guild.GetRole(Constants.Constants.SimplyMemberRoleId));

            return Task.CompletedTask;
        }

        private Task Ready()
        {
            IntervalFunctions(null);
            return Task.CompletedTask;
        }

        private async Task Login()
        {
            try
            {
                Console.WriteLine(Constants.Constants.TokenPath);

                var token = File.ReadAllText(Constants.Constants.TokenPath);
                await _client.LoginAsync(TokenType.Bot, token);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
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
            if (!(message.HasCharPrefix(Constants.Constants.CommandPrefix, ref commandPosition) ||
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
            IntervalFunctions(null);
            return Task.CompletedTask;
        }

        private static Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        internal async void IntervalFunctions(object state)
        {
            // TODO: Uncomment this when the tempus module is complete
            //await _tempusDataAccess.UpdateMapList();

            await _rankUpdater.UpdateRanks();
            await _statusUpdater.UpdateStatus();
        }
    }
}
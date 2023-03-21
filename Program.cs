global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using Discord;
global using Discord.WebSocket;
global using System.Linq;
global using System.Text.Json;
global using System.IO;
global using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Net;
using Discord.Net;
using Discord.Rest;
using MinecraftConnection;
using MinecraftConnection.RCON;

namespace Discord_Bot
{
    class Program
    {
        public static Program instance = null;
        public DiscordSocketClient client;
        public SocketGuild edenor;
        static Timer googleTimer;
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        static string configDir = (Environment.CurrentDirectory + "/config.json");

        private bool ready = false;

        private BotConfig config = null;

        public MinecraftCommands rcon;

        public Program()
        {
            instance = this;

            googleTimer = new Timer(GoogleSheetsHelper.timer, new AutoResetEvent(true), 1000, 1800000);

            string stream = File.ReadAllText(configDir);
            config = JsonSerializer.Deserialize<BotConfig>(stream);

            try
            {
                rcon = new MinecraftCommands(config.rconIP, Convert.ToUInt16(config.rconPort), config.rconPassword);
            }
            catch (Exception e)
            {
                Program.logError(e.Message + e.StackTrace);
            }
            
            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.MessageContent,
                MessageCacheSize = 50
            };

            client = new DiscordSocketClient(socketConfig);
            client.MessageReceived += MessagesHandler;
            client.Log += Log;
            client.Ready += onReady;
            client.SlashCommandExecuted += handlers.CommandsHandler.onCommand;
            client.ButtonExecuted += handlers.ButtonsHandler.onButton;
            client.MessageDeleted += onMessageDeleted;
            client.ModalSubmitted += handlers.ModalsHandler.onModal;
            client.SelectMenuExecuted += handlers.SelectMenuHandler.onSelect;
            client.UserBanned += handlers.BanHandler.onBan;
            client.UserJoined += handlers.OnGuildJoin.onJoin;

            GoogleSheetsHelper.setupHelper();
        }
        private async Task MainAsync()
        {
            var token = config.token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            await Task.Delay(Timeout.Infinite);
            await GoogleSheetsHelper.reloadInfos();
        }
        private async Task onReady()
        {
            ready = true;
            logTrace("Ready to work, bitches!");
            var edenGame = new Game("Эденор", ActivityType.Playing, ActivityProperties.Join, "https://edenor.ru/");       
            await client.SetActivityAsync(edenGame);
            edenor = client.CurrentUser.MutualGuilds.First(); //Easy access to edenor guild
            await handlers.CommandsHandler.setupCommands();

            await NumberCountingModule.loadAll();
        }

        public void start(object stateInfo)
        {
            client.StartAsync();
        }

        private Task Log(LogMessage arg)
        {
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    logCritical(arg.ToString(), arg.Source);
                    break;
                case LogSeverity.Error:
                    logError(arg.ToString(), arg.Source);
                    break;
                case LogSeverity.Warning:
                    logWarn(arg.ToString(), arg.Source);
                    break;
                case LogSeverity.Info:
                    logInfo(arg.ToString(), arg.Source);
                    break;
                default:
                    logTrace(arg.ToString(), arg.Source);
                    break;
            }
            return Task.CompletedTask;
        }
        private Task MessagesHandler(SocketMessage msg)
        {
            if (msg.Channel.Id == 1062273336354275348)
            {
                return NumberCountingModule.doWork(msg);
            }

            return Task.CompletedTask;
        }

        private Task onMessageDeleted(Cacheable<IMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2)
        {
            try
            {
                if (arg2.Value.Id == 1062273336354275348)
                {
                    NumberCountingModule.onMessageDeleted(arg1.Value, arg2.Value);
                }
                return Task.CompletedTask;
            }
            catch (Exception e)
            {
                logError(e.Message);
                return Task.CompletedTask;
            }
        }

        private static string Timestamp => $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}";
        public static async Task logTrace(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (!instance.ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)instance.edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }        
        }

        public static async Task logError(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (!instance.ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)instance.edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
        }

        public static async Task logInfo(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (!instance.ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)instance.edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
        }

        public static async Task logWarn(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (((string)msg).Contains("Rate limit triggered"))
            {
                instance.client.StopAsync();
                var waitTimer = new Timer(GoogleSheetsHelper.timer, new AutoResetEvent(false), 10000, 10000);
            }

            if (!instance.ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)instance.edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
        }

        public static async Task logCritical (object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (!instance.ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)instance.edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
        }

        public void timer(object stateInfo)
        {
            client.StartAsync();
        }
    }

    class BotConfig
    {
        public string token { set; get; }
        public string rconIP { set; get; }
        public string rconPort { set; get;}
        public string rconPassword { set; get; }
    }
}

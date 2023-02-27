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

        public Program()
        {
            instance = this;

            googleTimer = new Timer(GoogleSheetsHelper.timer, new AutoResetEvent(true), 1000, 1800000);

            string stream = File.ReadAllText(configDir);
            config = JsonSerializer.Deserialize<BotConfig>(stream);

            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.MessageContent,
                MessageCacheSize = 50
            };

            client = new DiscordSocketClient(socketConfig);
            client.MessageReceived += MessagesHandler;
            client.Log += Log;
            client.Ready += onReady;
            client.SlashCommandExecuted += CommandsHandler.onCommand;
            client.ButtonExecuted += ButtonsHandler.onButton;
            client.MessageDeleted += onMessageDeleted;
            client.ModalSubmitted += ModalsHandler.onModal;
            client.SelectMenuExecuted += SelectMenuModule.onSelect;

            GoogleSheetsHelper.setupHelper();
        }
        private async Task MainAsync()
        {
            var token = config.token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            await Task.Delay(Timeout.Infinite);
        }
        private async Task onReady()
        {
            ready = true;
            logTrace("Ready to work, bitches!");
            await client.SetActivityAsync(new StreamingGame("Эденор", "https://edenor.ru/"));
            edenor = client.CurrentUser.MutualGuilds.First(); //Easy access to edenor guild
            CommandsHandler.setupCommands();
            GoogleSheetsHelper.timer(null);
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
                    logCritical(arg.ToString());
                    break;
                case LogSeverity.Error:
                    logError(arg.ToString());
                    break;
                case LogSeverity.Warning:
                    logWarn(arg.ToString());
                    break;
                case LogSeverity.Info:
                    logInfo(arg.ToString());
                    break;
                default:
                    logTrace(arg.ToString());
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
        public async Task logTrace(string msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }        
        }

        public async Task logError(string msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
        }

        public async Task logInfo(string msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
        }

        public async Task logWarn(string msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
        }

        public async Task logCritical (string msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (!ready)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
            else
            {
                await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
        }
    }

    class BotConfig
    {
        public string token { set; get; }
    }
}

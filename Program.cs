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
using MinecraftConnection;
using Discord_Bot.handlers;
using Discord.Webhook;

namespace Discord_Bot
{
    class Program
    {
        public static Program instance = null;
        private DiscordWebhookClient loggerWebhook;
        public DiscordSocketClient client;
        public SocketGuild edenor;
        public UserDatabase userDatabase;
        static Timer googleTimer;
        static Timer databaseUpdater;
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        static string configDir = (Environment.CurrentDirectory + "/config.json");

        private BotConfig config = null;

        public MinecraftCommands rcon;

        public Program()
        {
            instance = this;

            string stream = File.ReadAllText(configDir);
            config = System.Text.Json.JsonSerializer.Deserialize<BotConfig>(stream);

            config.enableRconFunctions = config.enableRconFunctions != null && config.enableRconFunctions;

            if (config.loggerWebhookURL != null)
            {
                loggerWebhook = new DiscordWebhookClient(config.loggerWebhookURL);
            }

            googleTimer = new Timer(GoogleSheetsHelper.timer, new AutoResetEvent(true), 1000, 1800000);
            databaseUpdater = new Timer(UserDatabase.timer, new AutoResetEvent(true), 300000, 300000);

            if (config.enableRconFunctions)
            {
                if (config.rconIP != null && config.rconPort != null && config.rconPassword != null)
                {
                    try
                    {
                        rcon = new MinecraftCommands(config.rconIP, Convert.ToUInt16(config.rconPort), config.rconPassword);
                    }
                    catch (Exception e)
                    {
                        logError(e.Message + e.StackTrace);
                    }
                }
            }

            var socketConfig = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.MessageContent,
                MessageCacheSize = 50,
                AlwaysDownloadUsers = true
            };

            client = new DiscordSocketClient(socketConfig);
            client.MessageReceived += MessagesHandler;
            client.Log += Log;
            client.Ready += onReady;
            client.SlashCommandExecuted += CommandsHandler.onCommand;
            client.ButtonExecuted += ButtonsHandler.onButton;
            client.MessageDeleted += onMessageDeleted;
            client.ModalSubmitted += ModalsHandler.onModal;
            client.SelectMenuExecuted += SelectMenuHandler.onSelect;
            client.UserBanned += BanHandler.onBan;
            client.UserJoined += OnGuildJoin.onJoin;
            client.ThreadCreated += TicketHandler.onNewThread;
            client.Disconnected += onDisconnected;
            client.GuildMemberUpdated += OnUserUpdated.onUpdate;

            userDatabase = new UserDatabase(677860751695806515);
        }
        private async Task MainAsync()
        {
            if (config.token == null)
            {
                await logError("Invalid token!");
                Process.GetCurrentProcess().Kill();
            }

            var token = config.token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            GoogleSheetsHelper.setupHelper();
            await GoogleSheetsHelper.reloadInfos();

            await NumberCountingModule.loadAll();

            await Task.Delay(Timeout.Infinite);
        }
        private async Task onReady()
        {

            var edenGame = new Game("Эденор", ActivityType.Playing, ActivityProperties.Join, "https://edenor.ru/");
            await client.SetActivityAsync(edenGame);

            edenor = client.CurrentUser.MutualGuilds.First(); //Easy access to edenor guild

            await CommandsHandler.setupCommands();

            await userDatabase.initDatabase();
        }
        private async Task onDisconnected(Exception arg)
        {
            await logError(arg.Message + arg.StackTrace);
            await userDatabase.saveData();
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
        private async Task MessagesHandler(SocketMessage msg)
        {
            if (msg.Channel.Id == 1062273336354275348)
            {
                await NumberCountingModule.doWork(msg);
                return;
            }

            return;
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
            if (instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }
            else
            {
                await instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [TRACE]:  {msg}");
            }        
        }

        public static async Task logError(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
            else
            {
                await instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [ERROR]:  {msg}");
            }
        }

        public static async Task logInfo(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
            else
            {
                await instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [INFO]:  {msg}");
            }
        }

        public static async Task logWarn(object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            if (instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
            else
            {
                await instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [WARN]:  {msg}");
            }
        }

        public static async Task logCritical (object msg, [CallerMemberName] string caller = "", [CallerLineNumber] int lineNumber = 0)
        {
            msg = msg.ToString();
            if (instance.loggerWebhook == null)
            {
                Console.WriteLine($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
            }
            else
            {
                await instance.loggerWebhook.SendMessageAsync($"[{Timestamp}] {caller} line: {lineNumber}: [CRITICAL ERROR]:  {msg}");
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
        public bool enableRconFunctions { set; get; }
        public string rconIP { set; get; }
        public string rconPort { set; get;}
        public string rconPassword { set; get; }
        public string openAIAPIKey { set; get; }

        public string loggerWebhookURL { set; get; }
    }
}

global using System;
global using System.Threading;
global using System.Threading.Tasks;
global using Discord;
global using Discord.WebSocket;
global using System.Linq;
global using System.Text.Json;
global using System.IO;
global using System.Collections.Generic;

namespace Discord_Bot
{
    class Program
    {
        public static Program instance = null;
        public DiscordSocketClient client;
        public SocketGuild edenor;
        static Timer googleTimer = new Timer(GoogleSheetsHelper.timer, new AutoResetEvent(true), 1000, 1800000);
        static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        static string configDir = (Environment.CurrentDirectory + "/config.json");

        public Program()
        {
            var config = new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.All,
                MessageCacheSize = 50
            };

            client = new DiscordSocketClient(config);
            client.MessageReceived += MessagesHandler;
            client.Log += Log;
            client.Ready += onReady;
            client.SlashCommandExecuted += CommandsHandler.onCommand;
            client.ButtonExecuted += ButtonsHandler.onButton;
            client.MessageDeleted += onMessageDeleted;
            client.ModalSubmitted += ModalsHandler.onModal;

            GoogleSheetsHelper.setupHelper();

            instance = this;
        }
        private async Task MainAsync()
        {
            using FileStream stream = File.OpenRead(configDir);
            BotConfig config = await JsonSerializer.DeserializeAsync<BotConfig>(stream);

            var token = config.token;

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();
            
            await Task.Delay(Timeout.Infinite);
        }
        private async Task onReady()
        {
            logTrace("Ready to work, bitches!");
            await client.SetActivityAsync(new StreamingGame("Эденор", "https://edenor.ru/"));
            edenor = client.CurrentUser.MutualGuilds.First(); //Easy access to edenor guild
            CommandsHandler.setupCommands();
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
            if (msg.Author.Id == 941460895912034335 && msg.Channel.Id == 1055783105916571658)
            {
                var enumerator = msg.Embeds.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (GoogleSheetsHelper.checkAccepted(/*enumerator.Current.Author.Value.Name, */enumerator.Current.Fields[0].Value))
                    {
                        var globalUser = client.GetUser(Convert.ToUInt64(enumerator.Current.Footer.Value.Text));
                        SocketGuild edenGuild = globalUser.MutualGuilds.ElementAt(0);
                        var enumerator2 = edenGuild.Users.GetEnumerator();
                        while (enumerator2.MoveNext())
                        {
                            if (enumerator2.Current.Id == Convert.ToUInt64(enumerator.Current.Footer.Value.Text))
                            {
                                enumerator2.Current.AddRoleAsync(802248363503648899);
                                msg.AddReactionAsync(new Emoji("\u2705"));
                            }
                        }   
                    }
                    else
                    {
                        msg.AddReactionAsync(new Emoji("\u274C"));
                    }
                }
            }

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
        public async Task logTrace(string msg)
        {
            await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] [TRACE] :  {msg}");
        }

        public async Task logError(string msg)
        {
            await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] [ERROR] :  {msg}");
        }

        public async Task logInfo(string msg)
        {
            await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] [INFO] :  {msg}");
        }

        public async Task logWarn(string msg)
        {
            await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] [WARN] :  {msg}");
        }
        public async Task logCritical (string msg)
        {
            await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync($"[{Timestamp}] [CRITICAL ERROR] :  {msg}");
        }
    }

    class BotConfig
    {
        public string token { get;}
    }
}

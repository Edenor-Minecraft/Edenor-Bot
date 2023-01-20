using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Text.Json;
using System.IO;

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
                GatewayIntents = GatewayIntents.All
            };

            client = new DiscordSocketClient(config);
            client.MessageReceived += MessagesHandler;
            client.Log += Log;
            client.Ready += onReady;
            client.SlashCommandExecuted += CommandsHandler.onCommand;
            client.ButtonExecuted += ButtonsHandler.onButton;

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
           // await client.SetActivityAsync();
            await client.SetGameAsync("Эденор!", null, ActivityType.Listening);
            edenor = client.CurrentUser.MutualGuilds.First(); //Easy access to edenor guild
            CommandsHandler.setupCommands();
        }

        private Task Log(LogMessage arg)
        {
            logTrace(arg.ToString());
            return Task.CompletedTask;
        }
        private Task MessagesHandler(SocketMessage msg)
        {
            if (msg.Author.Id == 941460895912034335)
            {
                var enumerator = msg.Embeds.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    if (GoogleSheetsHelper.checkAccepted(enumerator.Current.Author.Value.Name, enumerator.Current.Fields[0].Value))
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
            else {
                return Task.CompletedTask;
            }
        }

        public async Task logTrace(string msg)
        {
            await ((SocketTextChannel)edenor.GetChannel(1065968855878475777)).SendMessageAsync(msg);
        }
    }

    class BotConfig
    {
        public string token { get; set; }
    }
}

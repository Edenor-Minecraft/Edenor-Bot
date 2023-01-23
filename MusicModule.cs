using Discord.Audio;
using System.Diagnostics;

namespace Discord_Bot
{
    class MusicModule
    { 
        static string defaultPath = (Environment.CurrentDirectory + "/test.mp3");
        public static async Task onCommand(SocketSlashCommand command)
        {
            var options = command.Data.Options.ToArray();
            switch (command.CommandName)
            {
                /*case "join":
                    var channel = Program.instance.edenor.GetUser(command.User.Id).VoiceChannel;
                    if (channel != null)
                    {
                        audioClient = await channel.ConnectAsync(true, false, true);
                        if (wrappedAudioClient == null) wrappedAudioClient = new WrappedAudioClient(audioClient);
                        await SendAsync(audioClient, command, options[0].Value.ToString());
                        command.RespondAsync("Успешно зашли в канал!");
                    }
                    else
                    {
                        command.RespondAsync("Не смогли зайти в канал!");
                    }
                    break;*/
                case "play":
                    var channel = Program.instance.edenor.GetUser(command.User.Id).VoiceChannel;
                    var audioClient = await channel.ConnectAsync(true, false, true);
                    await SendAsync(audioClient, options[0].Value.ToString());
                    command.RespondAsync("Включаю");
                    break;
            }

        }
        public static async Task SendAsync(IAudioClient client, string path)
        {
            Program.instance.logTrace(defaultPath);
            using (var ffmpeg = CreateStream(defaultPath))
            using (var output = ffmpeg.StandardOutput.BaseStream)
            using (var discord = client.CreatePCMStream(AudioApplication.Mixed))
            {
                try { await output.CopyToAsync(discord); }
                finally { await discord.FlushAsync(); }
            }
        }

        private static Process CreateStream(string path)
        {
            return Process.Start(new ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = $"-hide_banner -loglevel panic -i \"{path}\" -ac 2 -f s16le -ar 48000 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
            });
        }
    }  
}

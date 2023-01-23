using Discord.Audio;
using Discord.Interactions;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using Lavalink4NET;
using System.Diagnostics;

namespace Discord_Bot
{
    class MusicModule
    {
        private IAudioService _audioService;

        public static MusicModule instance;

        public MusicModule(IAudioService audioService)
            => new MusicModule(audioService).create(audioService);

        public IAudioService create(IAudioService audioService)
        {
            _audioService = audioService ?? throw new ArgumentNullException(nameof(audioService));

            instance = this;

            return _audioService;
        }
        public static async Task Disconnect(SocketSlashCommand command)
        {
            var player = await GetPlayerAsync(command);

            if (player == null)
            {
                return;
            }

            await player.StopAsync(true);
            await command.RespondAsync("Disconnected.");
        }
        public static async Task Play(SocketSlashCommand command, string query)
        {
            var player = await GetPlayerAsync(command);

            if (player == null)
            {
                return;
            }

            var track = await instance._audioService.GetTrackAsync(query, SearchMode.YouTube);

            if (track == null)
            {
                await command.RespondAsync("😖 No results.");
                return;
            }

            var position = await player.PlayAsync(track, enqueue: true);

            if (position == 0)
            {
                await command.RespondAsync("🔈 Playing: " + track.Source);
            }
            else
            {
                await command.RespondAsync("🔈 Added to queue: " + track.Source);
            }
        }

        /*[SlashCommand("position", description: "Shows the track position", runMode: RunMode.Async)]
        public async Task Position()
        {
            var player = await GetPlayerAsync();

            if (player == null)
            {
                return;
            }

            if (player.CurrentTrack == null)
            {
                await ReplyAsync("Nothing playing!");
                return;
            }

            await ReplyAsync($"Position: {player.Position.Position} / {player.CurrentTrack.Duration}.");
        }*/

        public static async Task Stop(SocketSlashCommand command)
        {
            var player = await GetPlayerAsync(command, connectToVoiceChannel: false);

            if (player == null)
            {
                return;
            }

            if (player.CurrentTrack == null)
            {
                await command.RespondAsync("Nothing playing!");
                return;
            }

            await player.StopAsync();
            await command.RespondAsync("Stopped playing.");
        }

        /*[SlashCommand("volume", description: "Sets the player volume (0 - 1000%)", runMode: RunMode.Async)]
        public async Task Volume(int volume = 100)
        {
            if (volume is > 1000 or < 0)
            {
                await ReplyAsync("Volume out of range: 0% - 1000%!");
                return;
            }

            var player = await GetPlayerAsync();

            if (player == null)
            {
                return;
            }

            await player.SetVolumeAsync(volume / 100f);
            await ReplyAsync($"Volume updated: {volume}%");
        }*/

        private static async ValueTask<VoteLavalinkPlayer> GetPlayerAsync(SocketSlashCommand command, bool connectToVoiceChannel = true)
        {
            var player = instance._audioService.GetPlayer<VoteLavalinkPlayer>(Program.instance.edenor.Id);

            if (player != null
                && player.State != PlayerState.NotConnected
                && player.State != PlayerState.Destroyed)
            {
                return player;
            }

            var user = Program.instance.edenor.GetUser(command.User.Id);

            if (!user.VoiceState.HasValue)
            {
                await command.RespondAsync("You must be in a voice channel!");
                return null;
            }

            if (!connectToVoiceChannel)
            {
                await command.RespondAsync("The bot is not in a voice channel!");
                return null;
            }

            return await instance._audioService.JoinAsync<VoteLavalinkPlayer>(user.Guild.Id, user.VoiceChannel.Id);
        }
    }  
}

namespace Discord_Bot.commands.funny
{
    public class BaseFunnyCommand : BaseCommandClass
    {
        public new async static Task onCommand(SocketSlashCommand command, string replText, string url)
        {
            embed.WithImageUrl(url);
            embeds[0] = embed.Build();
            await command.RespondAsync(replText, embeds: embeds);
        }
    }
}

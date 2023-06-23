namespace Discord_Bot.commands
{
    public abstract class BaseCommandClass
    {
        public static EmbedBuilder embed = new EmbedBuilder().WithColor(new Color(0, 255, 255));
        public static Embed[] embeds = new Embed[1];
        public static IDictionary<string, string> locale = new Dictionary<string, string>();

        internal SlashCommandProperties commandProperties;
        public abstract Task onCommand(SocketSlashCommand command);
    }
}
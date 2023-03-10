namespace Discord_Bot.commands.other
{
    public class SaveChannelMessagesCommand : BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            try
            {
                await ChannelSaver.channelToHTMLCommand(command);
            }
            catch (Exception e)
            {
                await command.RespondAsync("Не удалось сохранить сообщения канала!");
                Program.logError(e.Message + e.StackTrace);
            }
        }
    }
}

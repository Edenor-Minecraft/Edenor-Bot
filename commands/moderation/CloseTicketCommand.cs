namespace Discord_Bot.commands.moderation
{
    internal class CloseTicketCommand : BaseCommandClass
    {
        public static new async Task onCommand(SocketSlashCommand command)
        {
            if (command.Channel is not SocketThreadChannel)
            {
                await command.RespondAsync("Невозможно использовать эту команду вне топика форума!");
            }

            if (command.Channel.Id != 1099416318140223588)
            {
                await command.RespondAsync("Невозможно использовать эту команду вне форума жалоб!");
            }

            await command.RespondAsync("Закрываем жалобу!");

            try
            {
                SocketThreadChannel channel = command.Channel as SocketThreadChannel;

                await channel.ModifyAsync(x =>
                {
                    x.Locked = true;
                });
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Жалоба закрыта!";
                });
            }
            catch(Exception e)
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Не удалось закрыть жалобу!";
                });

                Program.logError(e.Message + e.StackTrace);
            }
        }
    }
}

namespace Discord_Bot.commands.admin
{
    public class RefreshGoogleSheetDataCommand : BaseCommandClass
    {
        public static async new Task onCommand(SocketSlashCommand command)
        {
            try
            {
                GoogleSheetsHelper.timer(null);
                await command.RespondAsync("Успешно перезагрузили данные таблицы");
            }
            catch (Exception e)
            {
                Program.logTrace(e.Message);
                await command.RespondAsync("Не удалось перезагрузить данные таблицы");
            }
        }
    }
}

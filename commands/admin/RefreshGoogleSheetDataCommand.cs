namespace Discord_Bot.commands.admin
{
    public class RefreshGoogleSheetDataCommand : BaseCommandClass
    {
        public static async new Task onCommand(SocketSlashCommand command)
        {
            await command.RespondAsync("Перезагружаем данные таблицы");
            try
            {   
                GoogleSheetsHelper.reloadInfos();
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Успешно перезагрузили данные таблицы";
                });
            }
            catch (Exception e)
            {
                Program.logTrace(e.Message);
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Не удалось перезагрузить данные таблицы";
                });
            }
        }
    }
}

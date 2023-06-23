using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    public class RefreshGoogleSheetDataCommand : BaseCommandClass
    {
        public RefreshGoogleSheetDataCommand() {
            var refreshGoogleSheetData = new SlashCommandBuilder();
            locale.Add("ru", "перезагрузитьданныетаблицы");
            refreshGoogleSheetData.WithName("refreshgooglesheetdata");
            refreshGoogleSheetData.WithNameLocalizations(locale);
            refreshGoogleSheetData.WithDescription("Принудительно перезагружает данные таблицы вайт листа");
            refreshGoogleSheetData.WithDefaultMemberPermissions(GuildPermission.Administrator);
            locale.Clear();

            commandProperties = refreshGoogleSheetData.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "refreshgooglesheetdata") return;
            await command.ModifyOriginalResponseAsync(x => {
                x.Content = "Перезагружаем данные таблицы";
            });
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

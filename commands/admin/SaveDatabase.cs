using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    internal class SaveDatabase : BaseCommandClass
    {
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.User.Id != 324794944042565643)
            {
                await command.ModifyOriginalResponseAsync(x => {
                    x.Content = "Тебе сюда нельзя!";
                });
                return;
            }
            else
            {
                await command.ModifyOriginalResponseAsync(x => {
                    x.Content = "Сохраняем базу!";
                });
                await Program.instance.userDatabase.saveData();
            }
        }
    }
}

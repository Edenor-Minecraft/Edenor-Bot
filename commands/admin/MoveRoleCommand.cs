using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    public class MoveRoleCommand :BaseCommandClass
    {
        public MoveRoleCommand() {
            var moveRole = new SlashCommandBuilder();
            locale.Add("ru", "переместитьроль");
            moveRole.WithNameLocalizations(locale);
            moveRole.WithName("moverole");
            moveRole.WithDescription("Перемещает указанную роль на указанное место в списке ролей");
            moveRole.AddOption("роль", ApplicationCommandOptionType.Role, "Роль, которую надо переместить", true);
            moveRole.AddOption("позиция", ApplicationCommandOptionType.Integer, "Новая позиция роли", true);
            moveRole.WithDefaultMemberPermissions(GuildPermission.Administrator);
            locale.Clear();

            commandProperties = moveRole.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "moverole") return;
            try
            {
                foreach (var role in Program.instance.edenor.Roles)
                {
                    if (role.Id == ((SocketRole)command.Data.Options.ToList()[0].Value).Id)
                    {
                        role.ModifyAsync(x =>
                        {
                            x.Position = Convert.ToInt32(command.Data.Options.ToList()[1].Value);
                        });
                        await command.ModifyOriginalResponseAsync(x => {
                            x.Content = "Успешно изменили порядок ролей!";
                        });
                    }
                }
            }
            catch (Exception ex) {
                await command.ModifyOriginalResponseAsync(x => {
                    x.Content = "Не удалось изменить порядок ролей!";
                });
                Logger.logError(ex.Message); 
            }
        }
    }
}

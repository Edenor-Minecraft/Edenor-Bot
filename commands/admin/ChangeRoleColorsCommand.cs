using Discord_Bot.handlers;

namespace Discord_Bot.commands.admin
{
    public class ChangeRoleColorsCommand : BaseCommandClass
    { 
        public ChangeRoleColorsCommand() {
            var changeRoleColor = new SlashCommandBuilder();
            locale.Add("ru", "изменитьцветроли");
            changeRoleColor.WithNameLocalizations(locale);
            changeRoleColor.WithName("changerolecolor");
            changeRoleColor.WithDescription("Изменяет цвет указанной роли");
            changeRoleColor.AddOption("роль", ApplicationCommandOptionType.Role, "Роль, цвет которой надо изменить", true);
            changeRoleColor.AddOption("цвет", ApplicationCommandOptionType.String, "Новый цвет роли в hex (без #!)", true);
            changeRoleColor.WithDefaultMemberPermissions(GuildPermission.Administrator);
            locale.Clear();
            commandProperties = changeRoleColor.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "changerolecolor") { return; }
            var color = System.Drawing.ColorTranslator.FromHtml($"#{command.Data.Options.ToList()[1].Value.ToString()}");
            foreach (var role in Program.instance.edenor.Roles)
            {
                if (role.Id == ((SocketRole)command.Data.Options.ToList()[0].Value).Id)
                {
                    role.ModifyAsync(x =>
                    {
                        x.Color = new Color(color.R, color.G, color.B);
                    });

                    await command.ModifyOriginalResponseAsync(x => {
                        x.Content = "Успешно изменили цвет роли!";
                    });
                }
            }
        }
    }
}

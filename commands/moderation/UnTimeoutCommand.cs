using Discord;
using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    public class UnTimeoutCommand : BaseCommandClass
    {
        public UnTimeoutCommand() {
            var unTimeout = new SlashCommandBuilder();
            locale.Add("ru", "убратьтаймаут");
            unTimeout.WithName("untimeout");
            unTimeout.WithNameLocalizations(locale);
            unTimeout.WithDescription("Убирает тайм-аут с пользователя");
            unTimeout.WithDefaultMemberPermissions(GuildPermission.ModerateMembers);
            unTimeout.AddOption("user", ApplicationCommandOptionType.User, "Пользователь, с которого надо убрать тайм-аут", true);
            unTimeout.AddOption("reason", ApplicationCommandOptionType.String, "Причина снятия тайм-аута", false);
            locale.Clear();

            commandProperties = unTimeout.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "untimeout") return;

            IUser userToUntimeout = command.Data.Options.First().Value as IUser;
            string reason = $"{command.User.Username}";
            if (command.Data.Options.ElementAtOrDefault(1) != null) 
            {
                reason = command.Data.Options.ElementAtOrDefault(1).Value.ToString() + $"\n{command.User.Username}";
            }
            if (ModerationFunctions.unTimeOutUser(userToUntimeout, reason))
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Успешно убрали тайм-аут с пользователя " + userToUntimeout.Username;
                }); 
            }
            else {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Не удалось убрать тайм-аут с  пользователя " + userToUntimeout.Username;
                });
            }
        }
    }
}

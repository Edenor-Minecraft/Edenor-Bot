using Discord;
using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    public class UnbanCommand :BaseCommandClass
    {

        public UnbanCommand() {
            var unban = new SlashCommandBuilder();
            locale.Add("ru", "разбан");
            unban.WithName("unban");
            unban.WithNameLocalizations(locale);
            unban.WithDescription("Разбанивает человека");
            unban.WithDefaultMemberPermissions(GuildPermission.BanMembers);
            unban.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет разбанен", true);
            unban.AddOption("reason", ApplicationCommandOptionType.String, "Причина разбана", false);
            locale.Clear();

            commandProperties = unban.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "unban") return;
            IUser usetToUnban = command.Data.Options.First().Value as IUser;
            string reason = $"{command.User.Username}";
            if (command.Data.Options.ElementAtOrDefault(1) != null)
            {
                reason = command.Data.Options.ElementAtOrDefault(1).Value.ToString() + $"\n{command.User.Username}";
            }
            if (ModerationFunctions.unBanUser(usetToUnban, reason))
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Пользователь " + usetToUnban.Username + " успешно разбанен!";
                });
            }
            else 
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Не удалось разбанить пользователя " + usetToUnban.Username;
                });
            }
        }
    }
}

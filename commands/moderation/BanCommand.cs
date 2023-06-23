using Discord_Bot.handlers;

namespace Discord_Bot.commands.moderation
{
    public class BanCommand : BaseCommandClass
    {
        public BanCommand() {
            var ban = new SlashCommandBuilder();
            locale.Add("ru", "бан");
            ban.WithName("ban");
            ban.WithNameLocalizations(locale);
            ban.WithDescription("Банит участника сервера");
            ban.WithDefaultMemberPermissions(GuildPermission.BanMembers);
            ban.AddOption("user", ApplicationCommandOptionType.User, "Участник сервера, который будет забанен", true);
            ban.AddOption("days", ApplicationCommandOptionType.Integer, "Количество дней для удаления сообщений от этого пользователя", false);
            ban.AddOption("reason", ApplicationCommandOptionType.String, "Причина бана", false);
            ban.AddOption("sendreason", ApplicationCommandOptionType.Boolean, "Отправить причину бана нарушителю?", false);
            locale.Clear();

            commandProperties = ban.Build();

            CommandsHandler.OnCommand += onCommand;
        }
        public override async Task onCommand(SocketSlashCommand command)
        {
            if (command.CommandName != "ban") return;
            IUser userToBan = command.Data.Options.ToList()[0].Value as IUser;
            int days = 0;
            string reason = $"{command.User.Username}";
            bool showReason = true;
            foreach (var option in command.Data.Options)
            {
                var val = option.Value;
                if (val is int)
                {
                    days = (int)val;
                }
                else if (val is string) 
                {
                    reason = (string)val + $"\n{command.User.Username}";
                }
                else if (val is bool)
                {
                    showReason = (bool)val;
                }
            }
            if (ModerationFunctions.getMaxUserRolePosition(command.User.Id) > ModerationFunctions.getMaxUserRolePosition(userToBan.Id))
            {
                if (ModerationFunctions.banUser(userToBan, days, reason, showReason))
                {
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Пользователь " + userToBan.Username + " успешно забанен!";
                    });
                }
                else 
                {
                    await command.ModifyOriginalResponseAsync(x =>
                    {
                        x.Content = "Не удалось забанить пользователя " + userToBan.Username;
                    });
                }
            }
            else
            {
                await command.ModifyOriginalResponseAsync(x =>
                {
                    x.Content = "Вы не можете забанить данного пользователя!";
                });
            }
        }
    }
}
